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
using static MediaCenter.Vistas.VistaMusica;

namespace MediaCenter.Vistas
{

    public partial class VistaVideos : UserControl
    {

        public event EventHandler ArchivoAgregado;
        public VistaVideos()
        {
            InitializeComponent();
            AplicarTemaVideos();
            CargarVideosDesdeDB(); // ← agrega esta línea


        }

        private void btnAgregarVideo_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialogo = new OpenFileDialog();
            dialogo.Filter = "Videos|*.mp4;*.avi;*.wmv;*.mkv;*.mov";
            dialogo.Multiselect = true;

            if (dialogo.ShowDialog() == DialogResult.OK)
            {
                int agregados = 0;
                int rechazados = 0;
                int duplicados = 0; // ← nuevo
                string listaRechazos = "";

                foreach (string ruta in dialogo.FileNames)
                {
                    string mensajeError;
                    if (!ValidarArchivos.EsVideoValido(ruta, out mensajeError))
                    {
                        rechazados++;
                        listaRechazos += "- " + Path.GetFileName(ruta) +
                                         " → " + mensajeError + Environment.NewLine;
                        continue;
                    }

                    // ── Verificar duplicado ──────────────────────
                    if (ArchivoYaExisteEnBD(ruta))
                    {
                        duplicados++;
                        continue;
                    }

                    string nombre = Path.GetFileNameWithoutExtension(ruta);
                    lstVideos.Items.Add(new ItemCancion(nombre, ruta));
                    GuardarArchivoEnBD(ruta, "Video");
                    ArchivoAgregado?.Invoke(this, EventArgs.Empty);
                    agregados++;
                }

                if (rechazados > 0 || duplicados > 0)
                {
                    MessageBox.Show(
                        $"Resultado:{Environment.NewLine}" +
                        $"✅ Agregados:  {agregados}{Environment.NewLine}" +
                        $"⚠️ Duplicados: {duplicados}{Environment.NewLine}" +
                        $"❌ Rechazados: {rechazados}" +
                        (listaRechazos != "" ? Environment.NewLine + Environment.NewLine +
                         "Archivos rechazados:" + Environment.NewLine + listaRechazos : ""),
                        "Resultado de importación",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }

        }

        private void lstVideos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstVideos.SelectedItem == null) return;

            ItemCancion item = (ItemCancion)lstVideos.SelectedItem;
            string ruta = item.Ruta;

            // Verificar que el archivo sigue existiendo
            if (!System.IO.File.Exists(ruta))
            {
                MessageBox.Show("El archivo ya no existe en:\n" + ruta,
                    "Archivo no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lblInfoVideo.Text = "Archivo no encontrado.";
                return;
            }

            // Reproducir
            try
            {
                wmVideo.URL = ruta;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al reproducir: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Mostrar información del archivo
            try
            {
                FileInfo info = new FileInfo(ruta);

                string texto = "Archivo: " + info.Name + Environment.NewLine;
                texto += "Ruta: " + info.FullName + Environment.NewLine;
                texto += "Tamaño: " + (info.Length / 1024.0 / 1024.0).ToString("F2") + " MB" + Environment.NewLine;
                texto += "Formato: " + info.Extension.ToUpper().Replace(".", "") + Environment.NewLine;
                texto += "Fecha creación: " + info.CreationTime.ToString("dd/MM/yyyy HH:mm") + Environment.NewLine;
                texto += "Última modificación: " + info.LastWriteTime.ToString("dd/MM/yyyy HH:mm");

                lblInfoVideo.Text = texto;
            }
            catch (Exception ex)
            {
                lblInfoVideo.Text = "Error al leer información: " + ex.Message;
            }
        }



        private void AplicarTemaVideos()
        {
            this.BackColor = UITheme.ContentBg;

            // ── LISTBOX VIDEOS ───────────────────────────────
            lstVideos.BackColor = Color.FromArgb(10, 22, 40);
            lstVideos.ForeColor = UITheme.TextSecondary;
            lstVideos.BorderStyle = BorderStyle.None;
            lstVideos.Font = new Font("Segoe UI", 10f);
            lstVideos.ItemHeight = 28;

            // ── INFO VIDEO ───────────────────────────────────
            lblInfoVideo.BackColor = Color.FromArgb(10, 22, 40);
            lblInfoVideo.ForeColor = UITheme.TextSecondary;
            lblInfoVideo.Font = new Font("Segoe UI", 9.5f);

            // ── REPRODUCTOR ──────────────────────────────────
            wmVideo.BackColor = Color.FromArgb(6, 14, 26);

            // ── BOTÓN ────────────────────────────────────────
            EstilarBoton(btnAgregarVideo, "  🎬  Agregar Video", UITheme.SidebarActive);

            btnImportarCarpeta.Location = new Point(
     btnAgregarVideo.Left + btnAgregarVideo.Width + 10,
     btnAgregarVideo.Top);
            btnImportarCarpeta.Size = btnAgregarVideo.Size;
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
            dialogo.Description = "Selecciona la carpeta con tus videos";

            if (dialogo.ShowDialog() == DialogResult.OK)
            {
                string carpeta = dialogo.SelectedPath;

                string[] extensiones = { "*.mp4", "*.avi", "*.wmv", "*.mkv", "*.mov" };
                List<string> archivos = new List<string>();

                foreach (string ext in extensiones)
                    archivos.AddRange(Directory.GetFiles(carpeta, ext));

                if (archivos.Count == 0)
                {
                    MessageBox.Show("No se encontraron videos en esa carpeta.",
                                    "Sin archivos", MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                    return;
                }

                int importados = 0;
                int rechazados = 0;
                int duplicados = 0; // ← nuevo
                string listaRechazos = "";

                foreach (string ruta in archivos)
                {
                    string mensajeError;
                    if (!ValidarArchivos.EsVideoValido(ruta, out mensajeError))
                    {
                        rechazados++;
                        listaRechazos += "- " + Path.GetFileName(ruta) +
                                         " → " + mensajeError + Environment.NewLine;
                        continue;
                    }

                    // ── Verificar duplicado ──────────────────────
                    if (ArchivoYaExisteEnBD(ruta))
                    {
                        duplicados++;
                        continue;
                    }

                    string nombre = Path.GetFileNameWithoutExtension(ruta);
                    lstVideos.Items.Add(new ItemCancion(nombre, ruta));
                    GuardarArchivoEnBD(ruta, "Video");
                    importados++;
                }

                ArchivoAgregado?.Invoke(this, EventArgs.Empty);

                MessageBox.Show(
                    $"Importación completada:{Environment.NewLine}" +
                    $"✅ Importados:  {importados}{Environment.NewLine}" +
                    $"⚠️ Duplicados:  {duplicados}{Environment.NewLine}" +
                    $"❌ Rechazados:  {rechazados}" +
                    (listaRechazos != "" ? Environment.NewLine + Environment.NewLine +
                     "Archivos rechazados:" + Environment.NewLine + listaRechazos : ""),
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


        private void CargarVideosDesdeDB()
        {
            lstVideos.Items.Clear();

            string connStr = "Server=HPCOMPUTER18\\SQLEXPRESS01;" +
                             "Database=MediaCenterDB;" +
                             "Integrated Security=True;" +
                             "TrustServerCertificate=True;";

            var servicio = new MediaCenter.Servicios.EstadisticasServicio(connStr);
            var videos = servicio.ObtenerArchivosPorTipo("Video");

            foreach (var video in videos)
            {
                if (File.Exists(video.RutaCompleta))
                {
                    string nombre = Path.GetFileNameWithoutExtension(video.RutaCompleta);
                    lstVideos.Items.Add(new ItemCancion(nombre, video.RutaCompleta));
                }
                else
                {
                    lstVideos.Items.Add(new ItemCancion(
                        "⚠️ " + video.Nombre + " (no encontrado)",
                        video.RutaCompleta));
                }
            }
        }






    }
}
