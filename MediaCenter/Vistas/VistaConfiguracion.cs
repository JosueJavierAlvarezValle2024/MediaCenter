using System;
using System.Windows.Forms;
using MediaCenter.Datos;

namespace MediaCenter.Vistas
{
    public partial class VistaConfiguracion : UserControl
    {
        public VistaConfiguracion()
        {
            InitializeComponent();
            MostrarCadenaConexion();
        }

        private void MostrarCadenaConexion()
        {
            try
            {
                lblConexion.Text = "Cadena: " + ConexionSQL.ObtenerCadena();
            }
            catch (Exception ex)
            {
                lblConexion.Text = "Cadena: Error al obtener (" + ex.Message + ")";
            }
        }



        private void btnAcercaDe_Click(object sender, EventArgs e)
        {
            string info =
                "MediaCenter" + Environment.NewLine +
                "Gestor Multimedia Personal" + Environment.NewLine +
                Environment.NewLine +
                "Desarrollado por: Josue Javier Alvarez Valle" + Environment.NewLine +
                "Ingeniería en Informática" + Environment.NewLine +
                "Año: 2026" + Environment.NewLine +
                Environment.NewLine +
                "Tecnologías:" + Environment.NewLine +
                "- C# con Windows Forms (.NET)" + Environment.NewLine +
                "- SQL Server" + Environment.NewLine +
                "- TagLibSharp (metadatos de audio)" + Environment.NewLine +
                "- Windows Media Player (reproducción)" + Environment.NewLine +
                "- WebView2 con Leaflet (mapas GPS)";

            MessageBox.Show(info, "Acerca de MediaCenter",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                int fotos = ConexionSQL.ContarPorTipo("Foto");
                int musica = ConexionSQL.ContarPorTipo("Musica");
                int videos = ConexionSQL.ContarPorTipo("Video");
                int total = fotos + musica + videos;

                lblFotos.Text = "Fotos: " + fotos;
                lblMusica.Text = "Música: " + musica;
                lblVideos.Text = "Videos: " + videos;
                lblTotal.Text = "Total: " + total;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al obtener estadísticas: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

