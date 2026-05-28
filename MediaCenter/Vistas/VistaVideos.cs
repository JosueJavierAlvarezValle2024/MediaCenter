using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using static MediaCenter.Vistas.VistaMusica;
using MediaCenter.Servicios;

namespace MediaCenter.Vistas
{
    public partial class VistaVideos : UserControl
    {
        public VistaVideos()
        {
            InitializeComponent();
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
                string listaRechazos = "";

                foreach (string ruta in dialogo.FileNames)
                {
                    string mensajeError;

                    if (!ValidarArchivos.EsVideoValido(ruta, out mensajeError))
                    {
                        rechazados++;
                        listaRechazos += "- " + Path.GetFileName(ruta) + " -> " + mensajeError + Environment.NewLine;
                        continue;
                    }

                    string nombre = Path.GetFileNameWithoutExtension(ruta);
                    lstVideos.Items.Add(new ItemCancion(nombre, ruta));
                    agregados++;
                }

                if (rechazados > 0)
                {
                    MessageBox.Show(
                        "Resultado:" + Environment.NewLine +
                        "Agregados: " + agregados + Environment.NewLine +
                        "Rechazados: " + rechazados + Environment.NewLine + Environment.NewLine +
                        "Archivos rechazados:" + Environment.NewLine + listaRechazos,
                        "Archivos corruptos detectados",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
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






    }
}
