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
            AplicarTemaConfiguracion(); 

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


        private void AplicarTemaConfiguracion()
        {
            this.BackColor = UITheme.ContentBg;

            lblTitulo.ForeColor = UITheme.TextPrimary;
            lblTitulo.BackColor = UITheme.ContentBg;

            EstilarGroupBox(gbEstadisticas);

            lblFotos.ForeColor = UITheme.TextSecondary;
            lblFotos.BackColor = Color.Transparent;
            lblMusica.ForeColor = UITheme.TextSecondary;
            lblMusica.BackColor = Color.Transparent;
            lblVideos.ForeColor = UITheme.TextSecondary;
            lblVideos.BackColor = Color.Transparent;

            lblTotal.ForeColor = UITheme.AccentBlue;
            lblTotal.BackColor = Color.Transparent;

            EstilarBoton(btnActualizar, "  🔄  Actualizar estadísticas", UITheme.SidebarActive);

            EstilarGroupBox(gbConexion);

            lblConexion.ForeColor = UITheme.TextMuted;
            lblConexion.BackColor = Color.Transparent;
            lblConexion.Font = new Font("Segoe UI", 9f);

            EstilarBoton(btnAcercaDe, "  ℹ️  Acerca de", Color.FromArgb(13, 71, 161));
        }

        private void EstilarGroupBox(GroupBox gb)
        {
            gb.ForeColor = UITheme.AccentBlue;   
            gb.BackColor = Color.FromArgb(10, 22, 40);
            gb.Font = new Font("Segoe UI", 10f, FontStyle.Bold);

            
            foreach (Control ctrl in gb.Controls)
            {
                ctrl.BackColor = Color.Transparent;
                if (ctrl is Label lbl)
                    lbl.ForeColor = UITheme.TextSecondary;
            }
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










    }
}

