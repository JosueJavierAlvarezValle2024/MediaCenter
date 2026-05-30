using MediaCenter.Servicios;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MediaCenter.Vistas
{

    public partial class VistaMusica : UserControl
    {
        // Diccionario que guarda cada lista con sus canciones
        private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<ItemCancion>> listasReproduccion
            = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<ItemCancion>>();

        public event EventHandler ArchivoAgregado;
        public VistaMusica()
        {
            InitializeComponent();
            AplicarTemaMusica();
            CargarMusicaDesdeDB(); // ← agrega esta línea

        }

        private void btnAgregarCancion_Click(object sender, EventArgs e)
        {

            OpenFileDialog dialogo = new OpenFileDialog();
            dialogo.Filter = "Música|*.mp3;*.wav;*.wma";
            dialogo.Multiselect = true;

            if (dialogo.ShowDialog() == DialogResult.OK)
            {
                int agregadas = 0;
                int rechazadas = 0;
                int duplicadas = 0; // ← nuevo
                string listaRechazos = "";

                foreach (string ruta in dialogo.FileNames)
                {
                    string mensajeError;

                    if (!ValidarArchivos.EsAudioValido(ruta, out mensajeError))
                    {
                        rechazadas++;
                        listaRechazos += "- " + Path.GetFileName(ruta) +
                                         " → " + mensajeError + Environment.NewLine;
                        continue;
                    }

                    // ── Verificar duplicado ──────────────────────
                    if (ArchivoYaExisteEnBD(ruta))
                    {
                        duplicadas++;
                        continue;
                    }

                    string textoMostrar;
                    try
                    {
                        var archivo = TagLib.File.Create(ruta);
                        string titulo = string.IsNullOrEmpty(archivo.Tag.Title)
                            ? Path.GetFileNameWithoutExtension(ruta)
                            : archivo.Tag.Title;
                        string artista = string.IsNullOrEmpty(archivo.Tag.FirstPerformer)
                            ? "Desconocido"
                            : archivo.Tag.FirstPerformer;
                        textoMostrar = titulo + " - " + artista;
                    }
                    catch { textoMostrar = Path.GetFileName(ruta); }

                    lstCanciones.Items.Add(new ItemCancion(textoMostrar, ruta));
                    GuardarArchivoEnBD(ruta, "Musica");
                    ArchivoAgregado?.Invoke(this, EventArgs.Empty);
                    agregadas++;
                }

                if (rechazadas > 0 || duplicadas > 0)
                {
                    MessageBox.Show(
                        $"Resultado:{Environment.NewLine}" +
                        $"✅ Agregadas:  {agregadas}{Environment.NewLine}" +
                        $"⚠️ Duplicadas: {duplicadas}{Environment.NewLine}" +
                        $"❌ Rechazadas: {rechazadas}" +
                        (listaRechazos != "" ? Environment.NewLine + Environment.NewLine +
                         "Archivos rechazados:" + Environment.NewLine + listaRechazos : ""),
                        "Resultado de importación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
        }

        // Clase auxiliar para guardar texto visible + ruta real
        public class ItemCancion
        {
            public string Texto { get; set; }
            public string Ruta { get; set; }

            public ItemCancion(string texto, string ruta)
            {
                Texto = texto;
                Ruta = ruta;
            }

            // Esto define cómo se muestra en el ListBox
            public override string ToString()
            {
                return Texto;
            }
        }

        private void lstCanciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstCanciones.SelectedItem == null) return;

            ItemCancion item = (ItemCancion)lstCanciones.SelectedItem;
            string ruta = item.Ruta;

            // Verificar que el archivo sigue existiendo antes de reproducir
            if (!System.IO.File.Exists(ruta))
            {
                MessageBox.Show("El archivo ya no existe en:\n" + ruta,
                    "Archivo no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lblInfoCancion.Text = "Archivo no encontrado.";
                picCaratula.Image = null;
                return;
            }

            // Reproducir
            try
            {
                wmPlayer.URL = ruta;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al reproducir: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Leer y mostrar metadatos
            try
            {
                var archivo = TagLib.File.Create(ruta);

                string info = "Título: " + (archivo.Tag.Title ?? "Desconocido") + Environment.NewLine;
                info += "Artista: " + (archivo.Tag.FirstPerformer ?? "Desconocido") + Environment.NewLine;
                info += "Álbum: " + (archivo.Tag.Album ?? "Desconocido") + Environment.NewLine;
                info += "Año: " + (archivo.Tag.Year == 0 ? "Desconocido" : archivo.Tag.Year.ToString()) + Environment.NewLine;
                info += "Género: " + (archivo.Tag.FirstGenre ?? "Desconocido") + Environment.NewLine;
                info += "Duración: " + archivo.Properties.Duration.ToString(@"mm\:ss");

                lblInfoCancion.Text = info;

                // Mostrar caratula si existe
                if (archivo.Tag.Pictures.Length > 0)
                {
                    var picData = archivo.Tag.Pictures[0].Data.Data;
                    using (var ms = new System.IO.MemoryStream(picData))
                    {
                        picCaratula.Image = System.Drawing.Image.FromStream(ms);
                    }
                }
                else
                {
                    picCaratula.Image = null;
                }
            }
            catch (Exception ex)
            {
                lblInfoCancion.Text = "Error al leer metadatos: " + ex.Message;
            }

        }

        private void btnNuevaLista_Click(object sender, EventArgs e)
        {
            string nombre = Microsoft.VisualBasic.Interaction.InputBox(
             "Nombre de la nueva lista:",
             "Crear lista", "Mi lista");

            if (string.IsNullOrWhiteSpace(nombre)) return;

            // Agregar al ComboBox
            cmbListas.Items.Add(nombre);
            cmbListas.SelectedItem = nombre;

            MessageBox.Show("Lista '" + nombre + "' creada");
        }

        private void btnAgregarALista_Click(object sender, EventArgs e)
        {
            if (cmbListas.SelectedItem == null)
            {
                MessageBox.Show("Primero selecciona una lista del menú desplegable");
                return;
            }

            if (lstCanciones.SelectedItem == null)
            {
                MessageBox.Show("Primero selecciona una canción de la lista de arriba");
                return;
            }

            ItemCancion cancion = (ItemCancion)lstCanciones.SelectedItem;
            string nombreLista = cmbListas.SelectedItem.ToString();

            // Guardar en el diccionario de listas
            if (!listasReproduccion.ContainsKey(nombreLista))
                listasReproduccion[nombreLista] = new System.Collections.Generic.List<ItemCancion>();

            listasReproduccion[nombreLista].Add(cancion);

            MessageBox.Show("Canción agregada a la lista '" + nombreLista + "'");
        }

        private void btnVerLista_Click(object sender, EventArgs e)
        {
            if (cmbListas.SelectedItem == null)
            {
                MessageBox.Show("Selecciona una lista primero");
                return;
            }

            string nombreLista = cmbListas.SelectedItem.ToString();

            if (!listasReproduccion.ContainsKey(nombreLista) || listasReproduccion[nombreLista].Count == 0)
            {
                MessageBox.Show("La lista '" + nombreLista + "' está vacía");
                return;
            }

            // Reemplazar las canciones del ListBox con las de esta lista
            lstCanciones.Items.Clear();
            foreach (var cancion in listasReproduccion[nombreLista])
            {
                lstCanciones.Items.Add(cancion);
            }

            MessageBox.Show("Mostrando lista: " + nombreLista);
        }



        private void AplicarTemaMusica()
        {
            this.BackColor = UITheme.ContentBg;

            // ── LISTBOX CANCIONES ────────────────────────────
            lstCanciones.BackColor = Color.FromArgb(10, 22, 40);
            lstCanciones.ForeColor = UITheme.TextSecondary;
            lstCanciones.BorderStyle = BorderStyle.None;
            lstCanciones.Font = new Font("Segoe UI", 10f);
            lstCanciones.ItemHeight = 28;

            // ── CARÁTULA ─────────────────────────────────────
            picCaratula.BackColor = Color.FromArgb(6, 14, 26);
            picCaratula.BorderStyle = BorderStyle.None;
            picCaratula.SizeMode = PictureBoxSizeMode.Zoom;

            // ── INFO CANCIÓN ─────────────────────────────────
            lblInfoCancion.BackColor = Color.FromArgb(10, 22, 40);
            lblInfoCancion.ForeColor = UITheme.TextSecondary;
            lblInfoCancion.Font = new Font("Segoe UI", 9.5f);
            lblInfoCancion.BorderStyle = BorderStyle.None; // ← esta línea quita el borde

            // ── ETIQUETA LISTAS ──────────────────────────────
            lblListas.ForeColor = UITheme.TextMuted;
            lblListas.BackColor = UITheme.ContentBg;
            lblListas.Font = new Font("Segoe UI", 9f, FontStyle.Bold);

            // ── COMBOBOX LISTAS ──────────────────────────────
            cmbListas.BackColor = Color.FromArgb(10, 22, 40);
            cmbListas.ForeColor = UITheme.TextSecondary;
            cmbListas.FlatStyle = FlatStyle.Flat;
            cmbListas.Font = new Font("Segoe UI", 10f);

            // ── REPRODUCTOR ──────────────────────────────────
            // El fondo del WMP no se puede cambiar directamente,
            // pero lo rodeamos con el color del tema
            wmPlayer.BackColor = Color.FromArgb(6, 14, 26);

            // ── BOTONES ──────────────────────────────────────
            EstilarBoton(btnAgregarCancion, "  🎵  Agregar Canción", UITheme.SidebarActive);
            EstilarBoton(btnNuevaLista, "  ➕  Nueva Lista", Color.FromArgb(13, 71, 161));
            EstilarBoton(btnAgregarALista, "  📋  Agregar a Lista", Color.FromArgb(13, 71, 161));
            EstilarBoton(btnVerLista, "  👁️  Ver Lista", Color.FromArgb(13, 71, 161));

            // ── COMBOBOX OSCURO ──────────────────────────────────
            cmbListas.BackColor = Color.FromArgb(10, 22, 40);
            cmbListas.ForeColor = UITheme.TextSecondary;
            cmbListas.FlatStyle = FlatStyle.Flat;
            cmbListas.Font = new Font("Segoe UI", 10f);

            // Panel que rodea al ComboBox simulando borde de color
            Panel pnlCombo = new Panel();
            pnlCombo.BackColor = UITheme.AccentBlue;
            pnlCombo.Bounds = new Rectangle(
                cmbListas.Left - 1,
                cmbListas.Top - 1,
                cmbListas.Width + 2,
                cmbListas.Height + 2);

            this.Controls.Add(pnlCombo);
            pnlCombo.BringToFront();
            cmbListas.BringToFront();

            btnImportarCarpeta.Location = new Point(
     btnAgregarCancion.Left + btnAgregarCancion.Width + 10,
     btnAgregarCancion.Top);
            btnImportarCarpeta.Size = btnAgregarCancion.Size;
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
            btn.Font = new Font("Segoe UI", 10f);
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
                       (Nombre, RutaCompleta, Tipo, Extension, TamanoKB, FechaAgregado, EstaCorrupto)
                       VALUES 
                       (@nombre, @ruta, @tipo, @extension, @tamano, @fecha, 0)";

                using (var conn = MediaCenter.Datos.ConexionSQL.ObtenerConexion())
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
            dialogo.Description = "Selecciona la carpeta con tu música";

            if (dialogo.ShowDialog() == DialogResult.OK)
            {
                string carpeta = dialogo.SelectedPath;

                string[] extensiones = { "*.mp3", "*.wav", "*.wma" };
                List<string> archivos = new List<string>();

                foreach (string ext in extensiones)
                    archivos.AddRange(Directory.GetFiles(carpeta, ext));

                if (archivos.Count == 0)
                {
                    MessageBox.Show("No se encontraron canciones en esa carpeta.",
                                    "Sin archivos", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    return;
                }

                int importadas = 0;
                int rechazadas = 0;
                int duplicadas = 0; // ← nuevo
                string listaRechazos = "";

                foreach (string ruta in archivos)
                {
                    string mensajeError;
                    if (!ValidarArchivos.EsAudioValido(ruta, out mensajeError))
                    {
                        rechazadas++;
                        listaRechazos += "- " + Path.GetFileName(ruta) +
                                         " → " + mensajeError + Environment.NewLine;
                        continue;
                    }

                    // ── Verificar duplicado ──────────────────────
                    if (ArchivoYaExisteEnBD(ruta))
                    {
                        duplicadas++;
                        continue;
                    }

                    string textoMostrar;
                    try
                    {
                        var archivo = TagLib.File.Create(ruta);
                        string titulo = string.IsNullOrEmpty(archivo.Tag.Title)
                            ? Path.GetFileNameWithoutExtension(ruta)
                            : archivo.Tag.Title;
                        string artista = string.IsNullOrEmpty(archivo.Tag.FirstPerformer)
                            ? "Desconocido"
                            : archivo.Tag.FirstPerformer;
                        textoMostrar = titulo + " - " + artista;
                    }
                    catch { textoMostrar = Path.GetFileName(ruta); }

                    lstCanciones.Items.Add(new ItemCancion(textoMostrar, ruta));
                    GuardarArchivoEnBD(ruta, "Musica");
                    importadas++;
                }

                ArchivoAgregado?.Invoke(this, EventArgs.Empty);

                MessageBox.Show(
                    $"Importación completada:{Environment.NewLine}" +
                    $"✅ Importadas:  {importadas}{Environment.NewLine}" +
                    $"⚠️ Duplicadas:  {duplicadas}{Environment.NewLine}" +
                    $"❌ Rechazadas:  {rechazadas}",
                    "Importar Carpeta",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

        }


        private bool ArchivoYaExisteEnBD(string rutaCompleta)
        {
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



        private void CargarMusicaDesdeDB()
        {
            lstCanciones.Items.Clear();

            string connStr = "Server=HPCOMPUTER18\\SQLEXPRESS01;" +
                             "Database=MediaCenterDB;" +
                             "Integrated Security=True;" +
                             "TrustServerCertificate=True;";

            var servicio = new MediaCenter.Servicios.EstadisticasServicio(connStr);
            var canciones = servicio.ObtenerArchivosPorTipo("Musica");

            foreach (var cancion in canciones)
            {
                if (File.Exists(cancion.RutaCompleta))
                {
                    // Leer metadatos con TagLib
                    string textoMostrar;
                    try
                    {
                        var archivo = TagLib.File.Create(cancion.RutaCompleta);
                        string titulo = string.IsNullOrEmpty(archivo.Tag.Title)
                            ? Path.GetFileNameWithoutExtension(cancion.RutaCompleta)
                            : archivo.Tag.Title;
                        string artista = string.IsNullOrEmpty(archivo.Tag.FirstPerformer)
                            ? "Desconocido"
                            : archivo.Tag.FirstPerformer;
                        textoMostrar = titulo + " - " + artista;
                    }
                    catch { textoMostrar = cancion.Nombre; }

                    lstCanciones.Items.Add(new ItemCancion(textoMostrar, cancion.RutaCompleta));
                }
                else
                {
                    lstCanciones.Items.Add(new ItemCancion(
                        "⚠️ " + cancion.Nombre + " (no encontrado)",
                        cancion.RutaCompleta));
                }
            }
        }




    }
}
