using MediaCenter.Servicios;
using Microsoft.Data.SqlClient;
using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace MediaCenter.Vistas
{
    public partial class VistaFotos : UserControl
    {
        
        public event EventHandler ArchivoAgregado;
        public VistaFotos()
        {
            InitializeComponent();

        }


        private void btnAgregarFoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialogo = new OpenFileDialog();
            dialogo.Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.bmp";
            dialogo.Multiselect = true;

            if (dialogo.ShowDialog() == DialogResult.OK)
            {
                int agregadas = 0;
                int rechazadas = 0;
                int duplicadas = 0;
                string listaRechazos = "";

                foreach (string ruta in dialogo.FileNames)
                {
                    string mensajeError;

                    if (ValidarArchivos.EsImagenValida(ruta, out mensajeError))
                    {
                        
                        if (ArchivoYaExisteEnBD(ruta))
                        {
                            duplicadas++;
                            continue; 
                        }

                        lstFotos.Items.Add(ruta);
                        GuardarArchivoEnBD(ruta, "Foto");
                        ArchivoAgregado?.Invoke(this, EventArgs.Empty);
                        agregadas++;
                    }
                    else
                    {
                        rechazadas++;
                        listaRechazos += "- " + Path.GetFileName(ruta) +
                                         " → " + mensajeError + Environment.NewLine;
                    }
                }

                
                if (rechazadas > 0 || duplicadas > 0)
                {
                    MessageBox.Show(
                        $"Resultado:{Environment.NewLine}" +
                        $"✅ Agregadas:   {agregadas}{Environment.NewLine}" +
                        $"⚠️ Duplicadas:  {duplicadas}{Environment.NewLine}" +
                        $"❌ Rechazadas:  {rechazadas}{Environment.NewLine}" +
                        (listaRechazos != "" ? Environment.NewLine +
                         "Archivos rechazados:" + Environment.NewLine + listaRechazos : ""),
                        "Resultado de importación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }

        }



        private async void lstFotos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFotos.SelectedItem == null) return;

            string ruta = lstFotos.SelectedItem.ToString();

            
            if (!File.Exists(ruta))
            {
                MessageBox.Show("El archivo ya no existe en:\n" + ruta,
                    "Archivo no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                picVisor.Image = null;
                lblInfoFoto.Text = "Archivo no encontrado.";
                return;
            }

            try
            {
                picVisor.Image = Image.FromFile(ruta);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir la imagen: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string info = "Archivo: " + Path.GetFileName(ruta) + Environment.NewLine;

                using (Image img = Image.FromFile(ruta))
                {
                    double? lat = ObtenerCoordenada(img, 0x0002, 0x0001);
                    double? lon = ObtenerCoordenada(img, 0x0004, 0x0003);

                    if (lat.HasValue && lon.HasValue)
                    {
                        info += "Latitud: " + lat.Value.ToString("F6") + Environment.NewLine;
                        info += "Longitud: " + lon.Value.ToString("F6") + Environment.NewLine;

                        string lugar = await ObtenerNombreLugarAsync(lat.Value, lon.Value);
                        info += "Lugar: " + lugar;

                        MostrarMapa(lat.Value, lon.Value);
                    }
                    else
                    {
                        info += "Esta foto no tiene datos GPS";
                        webMapa.Source = new Uri("about:blank");
                    }
                }

                lblInfoFoto.Text = info;
            }
            catch (Exception ex)
            {
                lblInfoFoto.Text = "Error al leer metadatos: " + ex.Message;
            }
        }

        private double? ObtenerCoordenada(Image img, int idCoord, int idRef)
        {
            try
            {
                var propCoord = img.GetPropertyItem(idCoord);
                var propRef = img.GetPropertyItem(idRef);

                uint gradosNum = BitConverter.ToUInt32(propCoord.Value, 0);
                uint gradosDen = BitConverter.ToUInt32(propCoord.Value, 4);
                uint minNum = BitConverter.ToUInt32(propCoord.Value, 8);
                uint minDen = BitConverter.ToUInt32(propCoord.Value, 12);
                uint segNum = BitConverter.ToUInt32(propCoord.Value, 16);
                uint segDen = BitConverter.ToUInt32(propCoord.Value, 20);

                double grados = (double)gradosNum / gradosDen;
                double minutos = (double)minNum / minDen;
                double segundos = (double)segNum / segDen;

                double resultado = grados + (minutos / 60) + (segundos / 3600);

                char referencia = (char)propRef.Value[0];
                if (referencia == 'S' || referencia == 'W')
                    resultado = -resultado;

                return resultado;
            }
            catch
            {
                return null;
            }
        }


        
        private async Task<string> ObtenerNombreLugarAsync(double lat, double lon)
        {
            try
            {
                using (HttpClient cliente = new HttpClient())
                {
                    // Nominatim requiere un User-Agent identificable
                    cliente.DefaultRequestHeaders.Add("User-Agent", "MediaCenter-App");

                    string url = $"https://nominatim.openstreetmap.org/reverse?format=json&lat={lat}&lon={lon}&accept-language=es";
                    string respuesta = await cliente.GetStringAsync(url);

                    // Buscar el campo "display_name" en la respuesta JSON
                    int inicio = respuesta.IndexOf("\"display_name\":\"") + 16;
                    int fin = respuesta.IndexOf("\"", inicio);
                    string lugar = respuesta.Substring(inicio, fin - inicio);

                    return lugar;
                }
            }
            catch
            {
                return "No se pudo obtener el nombre del lugar";
            }
        }

        private async void MostrarMapa(double lat, double lon)
        {
            try
            {
                
                if (webMapa.CoreWebView2 == null)
                    await webMapa.EnsureCoreWebView2Async();

                
                string html = $@"
        <!DOCTYPE html>
        <html>
        <head>
            <link rel='stylesheet' href='https://unpkg.com/leaflet/dist/leaflet.css' />
            <style>body, html, #mapa {{ margin:0; height:100%; }}</style>
        </head>
        <body>
            <div id='mapa'></div>
            <script src='https://unpkg.com/leaflet/dist/leaflet.js'></script>
            <script>
                var mapa = L.map('mapa').setView([{lat.ToString("F6", System.Globalization.CultureInfo.InvariantCulture)}, {lon.ToString("F6", System.Globalization.CultureInfo.InvariantCulture)}], 14);
                L.tileLayer('https://tile.openstreetmap.org/{{z}}/{{x}}/{{y}}.png').addTo(mapa);
                L.marker([{lat.ToString("F6", System.Globalization.CultureInfo.InvariantCulture)}, {lon.ToString("F6", System.Globalization.CultureInfo.InvariantCulture)}]).addTo(mapa);
            </script>
        </body>
        </html>";

                webMapa.NavigateToString(html);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el mapa: " + ex.Message);
            }
        }

        private void btnEditarGPS_Click(object sender, EventArgs e)
        {
            if (lstFotos.SelectedItem == null)
            {
                MessageBox.Show("Primero selecciona una foto de la lista");
                return;
            }

            string ruta = lstFotos.SelectedItem.ToString();

            string latStr = Microsoft.VisualBasic.Interaction.InputBox(
                "Ingresa la nueva LATITUD (ejemplo: 25.432100):",
                "Editar coordenadas", "0.0");

            if (string.IsNullOrEmpty(latStr)) return;

            string lonStr = Microsoft.VisualBasic.Interaction.InputBox(
                "Ingresa la nueva LONGITUD (ejemplo: -100.987600):",
                "Editar coordenadas", "0.0");

            if (string.IsNullOrEmpty(lonStr)) return;

            if (!double.TryParse(latStr, System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out double nuevaLat) ||
                !double.TryParse(lonStr, System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out double nuevaLon))
            {
                MessageBox.Show("Las coordenadas deben ser números válidos");
                return;
            }

            GuardarCoordenadas(ruta, nuevaLat, nuevaLon);

            MessageBox.Show("Coordenadas actualizadas. Vuelve a seleccionar la foto para ver el cambio.");
        }

        private void GuardarCoordenadas(string ruta, double lat, double lon)
        {
            byte[] bytesOriginales = File.ReadAllBytes(ruta);

            using (MemoryStream ms = new MemoryStream(bytesOriginales))
            using (Image img = Image.FromStream(ms))
            {
                if (picVisor.Image != null)
                {
                    picVisor.Image.Dispose();
                    picVisor.Image = null;
                }

                var propLatRef = (System.Drawing.Imaging.PropertyItem)System.Runtime.Serialization.FormatterServices
                    .GetUninitializedObject(typeof(System.Drawing.Imaging.PropertyItem));
                var propLat = (System.Drawing.Imaging.PropertyItem)System.Runtime.Serialization.FormatterServices
                    .GetUninitializedObject(typeof(System.Drawing.Imaging.PropertyItem));
                var propLonRef = (System.Drawing.Imaging.PropertyItem)System.Runtime.Serialization.FormatterServices
                    .GetUninitializedObject(typeof(System.Drawing.Imaging.PropertyItem));
                var propLon = (System.Drawing.Imaging.PropertyItem)System.Runtime.Serialization.FormatterServices
                    .GetUninitializedObject(typeof(System.Drawing.Imaging.PropertyItem));

                propLatRef.Id = 0x0001;
                propLatRef.Type = 2;
                propLatRef.Value = new byte[] { (byte)(lat >= 0 ? 'N' : 'S'), 0 };
                propLatRef.Len = 2;

                propLat.Id = 0x0002;
                propLat.Type = 5;
                propLat.Value = ConvertirAGradosEXIF(Math.Abs(lat));
                propLat.Len = 24;

                propLonRef.Id = 0x0003;
                propLonRef.Type = 2;
                propLonRef.Value = new byte[] { (byte)(lon >= 0 ? 'E' : 'W'), 0 };
                propLonRef.Len = 2;

                propLon.Id = 0x0004;
                propLon.Type = 5;
                propLon.Value = ConvertirAGradosEXIF(Math.Abs(lon));
                propLon.Len = 24;

                img.SetPropertyItem(propLatRef);
                img.SetPropertyItem(propLat);
                img.SetPropertyItem(propLonRef);
                img.SetPropertyItem(propLon);

                img.Save(ruta);
            }
        }


        private byte[] ConvertirAGradosEXIF(double valor)
        {
            int grados = (int)valor;
            double restoMinutos = (valor - grados) * 60;
            int minutos = (int)restoMinutos;
            int segundos = (int)((restoMinutos - minutos) * 60 * 1000);

            byte[] resultado = new byte[24];

            BitConverter.GetBytes((uint)grados).CopyTo(resultado, 0);
            BitConverter.GetBytes((uint)1).CopyTo(resultado, 4);

            // Minutos
            BitConverter.GetBytes((uint)minutos).CopyTo(resultado, 8);
            BitConverter.GetBytes((uint)1).CopyTo(resultado, 12);

            // Segundos (con 3 decimales de precisión)
            BitConverter.GetBytes((uint)segundos).CopyTo(resultado, 16);
            BitConverter.GetBytes((uint)1000).CopyTo(resultado, 20);

            return resultado;
        }

        private void VistaFotos_Load(object sender, EventArgs e)
        {
            AplicarTemaFotos(); // ← agrega esta línea
            CargarFotosDesdeDB(); // ← agrega esta línea

        }

        private void AplicarTemaFotos()
        {
            
            this.BackColor = UITheme.ContentBg;

            lstFotos.BackColor = Color.FromArgb(10, 22, 40);    
            lstFotos.ForeColor = UITheme.TextSecondary;
            lstFotos.BorderStyle = BorderStyle.None;
            lstFotos.Font = new Font("Segoe UI", 10f);
            lstFotos.ItemHeight = 28;                            

            picVisor.BackColor = Color.FromArgb(6, 14, 26);    
            picVisor.BorderStyle = BorderStyle.None;
            picVisor.SizeMode = PictureBoxSizeMode.Zoom;       

            lblInfoFoto.BackColor = Color.FromArgb(10, 22, 40);
            lblInfoFoto.ForeColor = UITheme.TextSecondary;
            lblInfoFoto.Font = new Font("Segoe UI", 9.5f);
            lblInfoFoto.BorderStyle = BorderStyle.None;

            EstilarBoton(btnAgregarFoto, "  📷  Agregar Foto", UITheme.SidebarActive);

            EstilarBoton(btnEditarGPS, "  📍  Editar GPS", Color.FromArgb(13, 71, 161));


            this.Padding = new Padding(0, 8, 0, 0);


            Panel pnlListaBorde = new Panel();
            pnlListaBorde.BackColor = Color.FromArgb(26, 48, 80); 
            pnlListaBorde.Bounds = new Rectangle(
                lstFotos.Left - 1,
                lstFotos.Top - 1,
                lstFotos.Width + 2,
                lstFotos.Height + 2);
            pnlListaBorde.Controls.Add(lstFotos);

            lstFotos.Location = new Point(1, 1);
            lstFotos.Width = pnlListaBorde.Width - 2;
            lstFotos.Height = pnlListaBorde.Height - 2;

            this.Controls.Add(pnlListaBorde);
            pnlListaBorde.BringToFront(); 

           

            
            lstFotos.DrawMode = DrawMode.OwnerDrawFixed;
            lstFotos.DrawItem += (s, e) =>
            {
                if (lstFotos.Items.Count == 0)
                {
                    e.Graphics.DrawString(
                        "  Sin fotos cargadas",
                        new Font("Segoe UI", 9f, FontStyle.Italic),
                        new SolidBrush(UITheme.TextMuted),
                        e.Bounds);
                    return;
                }
                e.DrawBackground();
                if (e.Index >= 0)
                {
                    bool seleccionado = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                    e.Graphics.FillRectangle(
                        new SolidBrush(seleccionado ? UITheme.SidebarActive : lstFotos.BackColor),
                        e.Bounds);
                    e.Graphics.DrawString(
                        lstFotos.Items[e.Index].ToString(),
                        lstFotos.Font,
                        new SolidBrush(seleccionado ? Color.White : UITheme.TextSecondary),
                        e.Bounds.X + 8, e.Bounds.Y + 6);
                }
            };


            btnImportarCarpeta.Location = new Point(
                btnAgregarFoto.Left + btnAgregarFoto.Width + 10,
                btnAgregarFoto.Top);
            btnImportarCarpeta.Size = btnAgregarFoto.Size;
            EstilarBoton(btnImportarCarpeta, "  📂  Importar Carpeta",
                         Color.FromArgb(13, 71, 161));

        }

        
        private void EstilarBoton(Button btn, string texto, Color colorFondo)
        {
            btn.Text = texto;
            btn.BackColor = colorFondo;
            btn.ForeColor = UITheme.TextPrimary;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(
                    Math.Min(colorFondo.R + 20, 255),
                    Math.Min(colorFondo.G + 20, 255),
                    Math.Min(colorFondo.B + 20, 255));  
            btn.Font = new Font("Segoe UI", 10f, FontStyle.Regular);
            btn.Cursor = Cursors.Hand;
            btn.Height = 36;
        }



        private void GuardarArchivoEnBD(string rutaCompleta, string tipo)
        {
            try
            {
                string nombre = Path.GetFileName(rutaCompleta);
                string extension = Path.GetExtension(rutaCompleta).TrimStart('.');
                long tamanoKB = new FileInfo(rutaCompleta).Length / 1024;

                string sql = @"INSERT INTO dbo.Archivos 
                       (Nombre, RutaCompleta, Tipo, Extension, TamanoKB, FechaAgregado, Estacorrupto)
                       VALUES 
                       (@nombre, @ruta, @tipo, @extension, @tamano, @fecha, 0)";

                using (var conn = Datos.ConexionSQL.ObtenerConexion())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@nombre", nombre);
                        cmd.Parameters.AddWithValue("@ruta", rutaCompleta);
                        cmd.Parameters.AddWithValue("@tipo", tipo);
                        cmd.Parameters.AddWithValue("@extension", extension);
                        cmd.Parameters.AddWithValue("@tamano", tamanoKB);
                        cmd.Parameters.AddWithValue("@fecha", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar en BD: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImportarCarpeta_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialogo = new FolderBrowserDialog();
            dialogo.Description = "Selecciona la carpeta con tus fotos";

            if (dialogo.ShowDialog() == DialogResult.OK)
            {
                string carpeta = dialogo.SelectedPath;

                string[] extensiones = { "*.jpg", "*.jpeg", "*.png", "*.bmp" };
                List<string> archivos = new List<string>();

                foreach (string ext in extensiones)
                    archivos.AddRange(Directory.GetFiles(carpeta, ext));

                if (archivos.Count == 0)
                {
                    MessageBox.Show("No se encontraron imágenes en esa carpeta.",
                                    "Sin archivos", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    return;
                }

                int importadas = 0;
                int rechazadas = 0;
                int duplicadas = 0; 
                string listaRechazos = "";

                foreach (string ruta in archivos)
                {
                    string mensajeError;
                    if (ValidarArchivos.EsImagenValida(ruta, out mensajeError))
                    {
                        if (ArchivoYaExisteEnBD(ruta))
                        {
                            duplicadas++;
                            continue; 
                        }

                        lstFotos.Items.Add(ruta);
                        GuardarArchivoEnBD(ruta, "Foto");
                        importadas++;
                    }
                    else
                    {
                        rechazadas++;
                        listaRechazos += "- " + Path.GetFileName(ruta) +
                                         " → " + mensajeError + Environment.NewLine;
                    }
                }

                ArchivoAgregado?.Invoke(this, EventArgs.Empty);

                MessageBox.Show(
                    $"Importación completada:{Environment.NewLine}" +
                    $"✅ Importadas:  {importadas}{Environment.NewLine}" +
                    $"⚠️ Duplicadas:  {duplicadas}{Environment.NewLine}" +
                    $"❌ Rechazadas:  {rechazadas}" +
                    (listaRechazos != "" ? Environment.NewLine + Environment.NewLine +
                     "Archivos rechazados:" + Environment.NewLine + listaRechazos : ""),
                    "Importar Carpeta",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

        }

        private void CargarFotosDesdeDB()
        {
            lstFotos.Items.Clear();

            string connStr = "Server=HPCOMPUTER18\\SQLEXPRESS01;" +
                             "Database=MediaCenterDB;" +
                             "Integrated Security=True;" +
                             "TrustServerCertificate=True;";

            var servicio = new MediaCenter.Servicios.EstadisticasServicio(connStr);
            var fotos = servicio.ObtenerArchivosPorTipo("Foto");

            foreach (var foto in fotos)
            {
                if (File.Exists(foto.RutaCompleta))
                    lstFotos.Items.Add(foto.RutaCompleta);
                else
                    lstFotos.Items.Add("⚠️ " + foto.Nombre + " (no encontrado)");
            }
        }

        private bool ArchivoYaExisteEnBD(string rutaCompleta)
        {
            string connStr = "Server=HPCOMPUTER18\\SQLEXPRESS01;" +
                             "Database=MediaCenterDB;" +
                             "Integrated Security=True;" +
                             "TrustServerCertificate=True;";

            using (var conn = MediaCenter.Datos.ConexionSQL.ObtenerConexion())
            {
                conn.Open();
                string sql = "SELECT COUNT(*) FROM dbo.Archivos WHERE RutaCompleta = @ruta";
                using (var cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ruta", rutaCompleta);
                    return (int)cmd.ExecuteScalar() > 0;
                }
            }
        }

    }
}
