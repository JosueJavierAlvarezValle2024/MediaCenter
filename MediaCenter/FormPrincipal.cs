using MediaCenter.Vistas;

namespace MediaCenter
{
    public partial class FormPrincipal : Form
    {

        public FormPrincipal()
        {
            InitializeComponent();
            AplicarTemaVisual();
        }

        private void btnFotos_Click(object sender, EventArgs e)
        {
            MarcarBotonActivo(btnFotos, "📷", "Fotos");

            var vista = new VistaFotos();

            vista.ArchivoAgregado += (s, ev) => ActualizarBadges();

            
            MostrarVista(vista);

        }

        private void btnMusica_Click(object sender, EventArgs e)
        {
            MarcarBotonActivo(btnMusica, "🎵", "Música");
            var vista = new VistaMusica();
            vista.ArchivoAgregado += (s, ev) => ActualizarBadges();
            MostrarVista(vista);
        }

        private void btnVideos_Click(object sender, EventArgs e)
        {
            MarcarBotonActivo(btnVideos, "🎬", "Videos");
            var vista = new VistaVideos();
            vista.ArchivoAgregado += (s, ev) => ActualizarBadges();
            MostrarVista(vista);
        }

        private void btnBaseDatos_Click(object sender, EventArgs e)
        {
            MarcarBotonActivo(btnBaseDatos, "🗄️", "Base de Datos");
            MostrarVista(new VistaBaseDatos());
        }

        private void btnConfiguracion_Click(object sender, EventArgs e)
        {
            MarcarBotonActivo(btnConfiguracion, "⚙️", "Configuración");
            MostrarVista(new VistaConfiguracion());
        }




        private List<Button> _botonesMenu = new List<Button>();
        private Button _botonActivo = null;

        private void AplicarTemaVisual()
        {



            this.Text = "MediaCenter";  
            toolStrip1.Visible = false;   
                                          
                                         



            this.BackColor = UITheme.SidebarBg;
            panelMenu.BackColor = UITheme.SidebarBg;
            panelContenido.BackColor = UITheme.ContentBg;

            lblTitulo.Text = "MediaCenter";
            lblTitulo.ForeColor = UITheme.TextPrimary;
            lblTitulo.Font = UITheme.TitleFont;
            lblTitulo.BackColor = UITheme.SidebarBg;

            _botonesMenu = new List<Button>
    {

        btnInicio,
        btnFotos,
        btnMusica,
        btnVideos,
        btnBaseDatos,
        btnConfiguracion
    };


            btnInicio.Text = "  🏠  Inicio";
            btnFotos.Text = "  📷  Fotos";
            btnMusica.Text = "  🎵  Música";
            btnVideos.Text = "  🎬  Videos";
            btnBaseDatos.Text = "  🗄️  Base de Datos";
            btnConfiguracion.Text = "  ⚙️  Configuración";

            foreach (Button btn in _botonesMenu)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.FlatAppearance.MouseOverBackColor = UITheme.SidebarHover;
                btn.BackColor = UITheme.SidebarBg;
                btn.ForeColor = UITheme.TextSecondary;
                btn.Font = UITheme.MenuFont;
                btn.TextAlign = ContentAlignment.MiddleLeft;
                btn.Padding = new Padding(10, 0, 0, 0);
                btn.Cursor = Cursors.Hand;
                btn.Height = 42;

                btn.MouseEnter += (s, e) =>
                {
                    if (s != _botonActivo)
                        ((Button)s).BackColor = UITheme.SidebarHover;
                };
                btn.MouseLeave += (s, e) =>
                {
                    if (s != _botonActivo)
                        ((Button)s).BackColor = UITheme.SidebarBg;
                };
            }

            CrearTopBar();
            MarcarBotonActivo(btnFotos, "📷", "Fotos");
            MostrarInicio(); 
            ActualizarBadges(); 
            CrearStatusBar();
            ActualizarStatusBar();
            CrearPerfilMenu(); 
        }

        private void MarcarBotonActivo(Button botonSeleccionado,
                                string icono, string nombreSeccion)
        {
            foreach (Button btn in _botonesMenu)
            {
                btn.BackColor = UITheme.SidebarBg;
                btn.ForeColor = UITheme.TextSecondary;
                btn.Font = UITheme.MenuFont;
            }

            if (botonSeleccionado != null)
            {
                botonSeleccionado.BackColor = UITheme.SidebarActive;
                botonSeleccionado.ForeColor = UITheme.TextPrimary;
                botonSeleccionado.Font = UITheme.MenuFontActive;
            }

            _botonActivo = botonSeleccionado;

            if (_lblIcono != null) _lblIcono.Text = icono;
            if (_lblSeccion != null) _lblSeccion.Text = nombreSeccion;
        }



        private Panel _topBar;
        private Label _lblSeccion;
        private Label _lblIcono;
        private Label _lblHora;   
        private Label _lblFecha;  
        private System.Windows.Forms.Timer _reloj; 
        private Panel _statusBar;
        private Label _lblEstadoSQL;
        private Label _lblTotalArchivos;
        private Label _lblHoraStatus;

        private void CrearTopBar()
        {
            _topBar = new Panel();
            _topBar.Height = 52;
            _topBar.Dock = DockStyle.Top;
            _topBar.BackColor = UITheme.SidebarBg;

            _topBar.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(UITheme.DividerLine, 1))
                    e.Graphics.DrawLine(pen, 0, _topBar.Height - 1,
                                             _topBar.Width, _topBar.Height - 1);
            };

            _lblIcono = new Label();
            _lblIcono.Text = "📷";
            _lblIcono.Font = new Font("Segoe UI", 14f);
            _lblIcono.ForeColor = UITheme.AccentBlue;
            _lblIcono.BackColor = Color.Transparent;
            _lblIcono.AutoSize = true;
            _lblIcono.Location = new Point(18, 13);

            _lblSeccion = new Label();
            _lblSeccion.Text = "Fotos";
            _lblSeccion.Font = new Font("Segoe UI", 13f, FontStyle.Bold);
            _lblSeccion.ForeColor = UITheme.TextPrimary;
            _lblSeccion.BackColor = Color.Transparent;
            _lblSeccion.AutoSize = true;
            _lblSeccion.Location = new Point(62, 15);

            _topBar.Controls.Add(_lblIcono);
            _topBar.Controls.Add(_lblSeccion);

            panelContenido.Controls.Add(_topBar); 
            _topBar.BringToFront();

            Panel pnlReloj = new Panel();
            pnlReloj.BackColor = Color.FromArgb(13, 36, 66);
            pnlReloj.Size = new Size(300, 52);
            pnlReloj.Dock = DockStyle.Right; 

            _lblHora = new Label();
            _lblHora.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
            _lblHora.ForeColor = UITheme.AccentBlue;
            _lblHora.BackColor = Color.Transparent;
            _lblHora.AutoSize = true;
            _lblHora.Location = new Point(10, 14); 
            pnlReloj.Controls.Add(_lblHora);



            pnlReloj.Controls.Add(_lblHora);
            pnlReloj.Controls.Add(_lblFecha);
            _topBar.Controls.Add(pnlReloj);

            

            _reloj = new System.Windows.Forms.Timer();
            _reloj.Interval = 1000;
            _reloj.Tick += (s, e) => ActualizarReloj();
            _reloj.Start();
            ActualizarReloj();

            ActualizarReloj();


        }

        private void ActualizarReloj()
        {
            DateTime ahora = DateTime.Now;
            var cultura = new System.Globalization.CultureInfo("es-MX");

            string hora = ahora.ToString("hh:mm tt", cultura).Replace("a. m.", "am").Replace("p. m.", "pm");
            string fecha = ahora.ToString("ddd dd MMM yyyy", cultura);

            _lblHora.Text = hora + "  ·  " + fecha;
        }



        private void MostrarVista(UserControl vista)
        {
            for (int i = panelContenido.Controls.Count - 1; i >= 0; i--)
            {
                if (panelContenido.Controls[i] != _topBar)
                    panelContenido.Controls.RemoveAt(i);
            }

            vista.Location = new Point(0, 52);
            vista.Size = new Size(
                panelContenido.Width,
                panelContenido.Height - 52);
            vista.Anchor = AnchorStyles.Top | AnchorStyles.Bottom |
                             AnchorStyles.Left | AnchorStyles.Right;

            panelContenido.Controls.Add(vista);
            _topBar.BringToFront(); 

        }



        private void MostrarInicio()
        {
            MarcarBotonActivo(btnInicio, "🏠", "Inicio");

            string connStr = "Server=HPCOMPUTER18\\SQLEXPRESS01;" +
                             "Database=MediaCenterDB;" +
                             "Integrated Security=True;" +
                             "TrustServerCertificate=True;";

            var servicio = new MediaCenter.Servicios.EstadisticasServicio(connStr);
            VistaInicio inicio = new VistaInicio();
            inicio.TotalFotos = servicio.ContarPorTipo("Foto");
            inicio.TotalMusica = servicio.ContarPorTipo("Musica");
            inicio.TotalVideos = servicio.ContarPorTipo("Video");

            inicio.ArchivosRecientes = servicio.ObtenerRecientes(3);

            inicio.Construir();
            MostrarVista(inicio);
        }

        private void btnInicio_Click(object sender, EventArgs e)
        {
            MostrarInicio();
        }


        private void ActualizarBadges()
        {
            string connStr = "Server=HPCOMPUTER18\\SQLEXPRESS01;" +
                             "Database=MediaCenterDB;" +
                             "Integrated Security=True;" +
                             "TrustServerCertificate=True;";

            var servicio = new MediaCenter.Servicios.EstadisticasServicio(connStr);

            int totalFotos = servicio.ContarPorTipo("Foto");
            int totalMusica = servicio.ContarPorTipo("Musica");
            int totalVideos = servicio.ContarPorTipo("Video");

            btnFotos.Text = "  📷  Fotos" +
                                    (totalFotos > 0 ? $"   ({totalFotos})" : "");
            btnMusica.Text = "  🎵  Música" +
                                    (totalMusica > 0 ? $"   ({totalMusica})" : "");
            btnVideos.Text = "  🎬  Videos" +
                                    (totalVideos > 0 ? $"   ({totalVideos})" : "");

            ActualizarStatusBar(); 

        }






        private void CrearStatusBar()
        {
            _statusBar = new Panel();
            _statusBar.Height = 26;
            _statusBar.Dock = DockStyle.Bottom;
            _statusBar.BackColor = Color.FromArgb(6, 14, 26);

            _statusBar.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(UITheme.DividerLine, 1))
                    e.Graphics.DrawLine(pen, 0, 0, _statusBar.Width, 0);
            };

            Panel puntoDB = new Panel();
            puntoDB.Size = new Size(8, 8);
            puntoDB.BackColor = Color.FromArgb(26, 107, 58); 
            puntoDB.Location = new Point(14, 9);

            puntoDB.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (var brush = new SolidBrush(Color.FromArgb(26, 107, 58)))
                    e.Graphics.FillEllipse(brush, 0, 0, 7, 7);
            };

            _lblEstadoSQL = new Label();
            _lblEstadoSQL.Text = "SQL Server conectado";
            _lblEstadoSQL.Font = new Font("Segoe UI", 8.5f);
            _lblEstadoSQL.ForeColor = Color.FromArgb(61, 106, 154);
            _lblEstadoSQL.BackColor = Color.Transparent;
            _lblEstadoSQL.AutoSize = true;
            _lblEstadoSQL.Location = new Point(28, 5);

            Label sep1 = new Label();
            sep1.Text = "|";
            sep1.ForeColor = Color.FromArgb(26, 48, 80);
            sep1.BackColor = Color.Transparent;
            sep1.AutoSize = true;
            sep1.Location = new Point(170, 5);
            sep1.Font = new Font("Segoe UI", 8.5f);

            _lblTotalArchivos = new Label();
            _lblTotalArchivos.Text = "0 archivos cargados";
            _lblTotalArchivos.Font = new Font("Segoe UI", 8.5f);
            _lblTotalArchivos.ForeColor = Color.FromArgb(61, 106, 154);
            _lblTotalArchivos.BackColor = Color.Transparent;
            _lblTotalArchivos.AutoSize = true;
            _lblTotalArchivos.Location = new Point(184, 5);

            Label sep2 = new Label();
            sep2.Text = "|";
            sep2.ForeColor = Color.FromArgb(26, 48, 80);
            sep2.BackColor = Color.Transparent;
            sep2.AutoSize = true;
            sep2.Location = new Point(320, 5);
            sep2.Font = new Font("Segoe UI", 8.5f);

            Label lblVersion = new Label();
            lblVersion.Text = "MediaCenter v1.0.0";
            lblVersion.Font = new Font("Segoe UI", 8.5f);
            lblVersion.ForeColor = Color.FromArgb(42, 80, 128);
            lblVersion.BackColor = Color.Transparent;
            lblVersion.AutoSize = true;
            lblVersion.Dock = DockStyle.Right;
            lblVersion.TextAlign = ContentAlignment.MiddleRight;
            lblVersion.Padding = new Padding(0, 0, 14, 0);

            _statusBar.Controls.Add(puntoDB);
            _statusBar.Controls.Add(_lblEstadoSQL);
            _statusBar.Controls.Add(sep1);
            _statusBar.Controls.Add(_lblTotalArchivos);
            _statusBar.Controls.Add(sep2);
            _statusBar.Controls.Add(lblVersion);

            this.Controls.Add(_statusBar);
        }



        public void ActualizarStatusBar()
        {
            if (_lblTotalArchivos == null) return;

            string connStr = "Server=HPCOMPUTER18\\SQLEXPRESS01;" +
                             "Database=MediaCenterDB;" +
                             "Integrated Security=True;" +
                             "TrustServerCertificate=True;";

            var servicio = new MediaCenter.Servicios.EstadisticasServicio(connStr);
            int total = servicio.ContarPorTipo("Foto") +
                           servicio.ContarPorTipo("Musica") +
                           servicio.ContarPorTipo("Video");

            _lblTotalArchivos.Text = total + " archivos cargados";
        }




        private void CrearPerfilMenu()
        {
            Panel pnlPerfil = new Panel();
            pnlPerfil.Dock = DockStyle.Bottom;
            pnlPerfil.Height = 70;
            pnlPerfil.BackColor = UITheme.SidebarBg;

            pnlPerfil.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(UITheme.DividerLine, 1))
                    e.Graphics.DrawLine(pen, 0, 0, pnlPerfil.Width, 0);
            };

            Panel avatar = new Panel();
            avatar.Size = new Size(36, 36);
            avatar.Location = new Point(14, 17);
            avatar.BackColor = UITheme.SidebarActive;

            
            avatar.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (var brush = new SolidBrush(UITheme.SidebarActive))
                    e.Graphics.FillEllipse(brush, 0, 0, 35, 35);

                
                using (var font = new Font("Segoe UI", 11f, FontStyle.Bold))
                using (var brush = new SolidBrush(Color.White))
                {
                    var size = e.Graphics.MeasureString("MC", font);
                    float x = (35 - size.Width) / 2;
                    float y = (35 - size.Height) / 2;
                    e.Graphics.DrawString("MC", font, brush, x, y);
                }
            };

            
            Label lblNombre = new Label();
            lblNombre.Text = "Josue J.";
            lblNombre.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            lblNombre.ForeColor = UITheme.TextSecondary;
            lblNombre.BackColor = Color.Transparent;
            lblNombre.AutoSize = true;
            lblNombre.Location = new Point(58, 18);

            Label lblVersion = new Label();
            lblVersion.Text = "Desktop · v1.0";
            lblVersion.Font = new Font("Segoe UI", 8f);
            lblVersion.ForeColor = UITheme.TextMuted;
            lblVersion.BackColor = Color.Transparent;
            lblVersion.AutoSize = true;
            lblVersion.Location = new Point(58, 38);

            pnlPerfil.Controls.Add(avatar);
            pnlPerfil.Controls.Add(lblNombre);
            pnlPerfil.Controls.Add(lblVersion);

            panelMenu.Controls.Add(pnlPerfil);
        }
    }


}

    
