using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using static MediaCenter.Vistas.VistaMusica;

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
                foreach (string ruta in dialogo.FileNames)
                {
                    // Reusamos ItemCancion porque guarda igual: texto + ruta
                    string nombre = Path.GetFileNameWithoutExtension(ruta);
                    lstVideos.Items.Add(new ItemCancion(nombre, ruta));
                }
            }

        }

        private void lstVideos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstVideos.SelectedItem == null) return;

            ItemCancion item = (ItemCancion)lstVideos.SelectedItem;
            string ruta = item.Ruta;

            // Reproducir el video
            wmVideo.URL = ruta;

            // Mostrar información del archivo
            FileInfo info = new FileInfo(ruta);

            string texto = "Archivo: " + info.Name + "\n";
            texto += "Ruta: " + info.FullName + "\n";
            texto += "Tamaño: " + (info.Length / 1024.0 / 1024.0).ToString("F2") + " MB\n";
            texto += "Formato: " + info.Extension.ToUpper().Replace(".", "") + "\n";
            texto += "Fecha creación: " + info.CreationTime.ToString("dd/MM/yyyy HH:mm") + "\n";
            texto += "Última modificación: " + info.LastWriteTime.ToString("dd/MM/yyyy HH:mm");

            lblInfoVideo.Text = texto;

        }






    }
}
