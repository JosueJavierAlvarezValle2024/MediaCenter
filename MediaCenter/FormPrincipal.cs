using MediaCenter.Vistas;

namespace MediaCenter
{
    public partial class FormPrincipal : Form
    {
        public FormPrincipal()
        {
            InitializeComponent();
        }

        private void btnFotos_Click(object sender, EventArgs e)
        {
            panelContenido.Controls.Clear();
            VistaFotos vista = new VistaFotos();
            vista.Dock = DockStyle.Fill;
            panelContenido.Controls.Add(vista);
        }

        private void btnMusica_Click(object sender, EventArgs e)
        {
            panelContenido.Controls.Clear();
            VistaMusica vista = new VistaMusica();
            vista.Dock = DockStyle.Fill;
            panelContenido.Controls.Add(vista);
        }

        private void btnVideos_Click(object sender, EventArgs e)
        {
            panelContenido.Controls.Clear();
            VistaVideos vista = new VistaVideos();
            vista.Dock = DockStyle.Fill;
            panelContenido.Controls.Add(vista);
        }

        private void btnBaseDatos_Click(object sender, EventArgs e)
        {
            panelContenido.Controls.Clear();
            VistaBaseDatos vista = new VistaBaseDatos();
            vista.Dock = DockStyle.Fill;
            panelContenido.Controls.Add(vista);
        }

        private void btnConfiguracion_Click(object sender, EventArgs e)
        {
            panelContenido.Controls.Clear();
            Label lbl = new Label();
            lbl.Text = "⚙️ Aquí va la Vista de Configuración.";
            lbl.Font = new Font("Segoe UI", 20);
            lbl.AutoSize = true;
            lbl.Location = new Point(50, 50);
            panelContenido.Controls.Add(lbl);
        }


    }
}
