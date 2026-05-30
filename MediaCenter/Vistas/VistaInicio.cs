using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel; // ← verifica que este using esté arriba del archivo


namespace MediaCenter.Vistas
{
    public partial class VistaInicio : UserControl
    {
        // Estas propiedades reciben los datos desde FormPrincipal
        // Estas propiedades reciben los datos desde FormPrincipal
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TotalFotos { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TotalMusica { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TotalVideos { get; set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<(string Nombre, string Tipo, DateTime Fecha)> ArchivosRecientes { get; set; }
    = new List<(string, string, DateTime)>();
        public int TotalArchivos => TotalFotos + TotalMusica + TotalVideos;

        public VistaInicio()
        {
            InitializeComponent();
            this.BackColor = UITheme.ContentBg;
        }

        // ── Se llama después de asignar los totales ──────────
        public void Construir()
        {
            this.Controls.Clear();
            int anchoTarjeta = 200;
            int altoTarjeta = 110;
            int margenX = 30;
            int margenY = 30;

            // ── TÍTULO DE BIENVENIDA ─────────────────────────
            Label lblBienvenida = new Label();
            lblBienvenida.Text = "Bienvenido a MediaCenter";
            lblBienvenida.Font = new Font("Segoe UI", 18f, FontStyle.Bold);
            lblBienvenida.ForeColor = UITheme.TextPrimary;
            lblBienvenida.BackColor = Color.Transparent;
            lblBienvenida.AutoSize = true;
            lblBienvenida.Location = new Point(margenX, margenY);
            this.Controls.Add(lblBienvenida);

            Label lblSub = new Label();
            lblSub.Text = "Gestor Multimedia Personal — resumen de tu biblioteca";
            lblSub.Font = new Font("Segoe UI", 10f);
            lblSub.ForeColor = UITheme.TextMuted;
            lblSub.BackColor = Color.Transparent;
            lblSub.AutoSize = true;
            lblSub.Location = new Point(margenX, margenY + 44);
            this.Controls.Add(lblSub);

            // ── LÍNEA SEPARADORA ─────────────────────────────
            Panel linea = new Panel();
            linea.BackColor = UITheme.DividerLine;
            linea.Height = 1;
            linea.Width = this.Width - (margenX * 2);
            linea.Location = new Point(margenX, margenY + 80);
            this.Controls.Add(linea);

            // ── TARJETAS DE ESTADÍSTICAS ─────────────────────
            int yTarjetas = margenY + 100;

            CrearTarjeta("📷", "Fotos", TotalFotos.ToString(),
                         UITheme.SidebarActive,
                         new Point(margenX, yTarjetas),
                         anchoTarjeta, altoTarjeta);

            CrearTarjeta("🎵", "Música", TotalMusica.ToString(),
                         Color.FromArgb(13, 71, 161),
                         new Point(margenX + anchoTarjeta + 16, yTarjetas),
                         anchoTarjeta, altoTarjeta);

            CrearTarjeta("🎬", "Videos", TotalVideos.ToString(),
                         Color.FromArgb(20, 60, 120),
                         new Point(margenX + (anchoTarjeta + 16) * 2, yTarjetas),
                         anchoTarjeta, altoTarjeta);

            CrearTarjeta("📁", "Total", TotalArchivos.ToString(),
                         Color.FromArgb(10, 50, 100),
                         new Point(margenX + (anchoTarjeta + 16) * 3, yTarjetas),
                         anchoTarjeta, altoTarjeta);

            // ── MENSAJE MOTIVACIONAL ─────────────────────────
            Label lblMensaje = new Label();
            lblMensaje.Text = "Selecciona una sección del menú lateral para comenzar.";
            lblMensaje.Font = new Font("Segoe UI", 10f, FontStyle.Italic);
            lblMensaje.ForeColor = UITheme.TextMuted;
            lblMensaje.BackColor = Color.Transparent;
            lblMensaje.AutoSize = true;
            lblMensaje.Location = new Point(margenX, yTarjetas + altoTarjeta + 24);
            this.Controls.Add(lblMensaje);
            // ── FILA DE PANELES INFERIORES ───────────────────────
            int yPaneles = yTarjetas + altoTarjeta + 60;

            CrearPanelActividad(margenX, yPaneles);
            CrearPanelDistribucion(margenX + 420, yPaneles);



        }

        // ── Crea una tarjeta de estadística ──────────────────
        private void CrearTarjeta(string icono, string titulo,
                                   string valor, Color colorFondo,
                                   Point ubicacion, int ancho, int alto)
        {
            Panel tarjeta = new Panel();
            tarjeta.BackColor = colorFondo;
            tarjeta.Size = new Size(ancho, alto);
            tarjeta.Location = ubicacion;
            tarjeta.Cursor = Cursors.Default;

            // Ícono
            Label lblIcono = new Label();
            lblIcono.Text = icono;
            lblIcono.Font = new Font("Segoe UI", 22f);
            lblIcono.ForeColor = Color.White;
            lblIcono.BackColor = Color.Transparent;
            lblIcono.AutoSize = true;
            lblIcono.Location = new Point(14, 12);

            // Número grande
            Label lblValor = new Label();
            lblValor.Text = valor;
            lblValor.Font = new Font("Segoe UI", 22f, FontStyle.Bold);
            lblValor.ForeColor = Color.White;
            lblValor.BackColor = Color.Transparent;
            lblValor.AutoSize = true;
            lblValor.Location = new Point(ancho - 70, 10);

            // Título de la tarjeta
            Label lblTitulo = new Label();
            lblTitulo.Text = titulo;
            lblTitulo.Font = new Font("Segoe UI", 9.5f);
            lblTitulo.ForeColor = Color.FromArgb(200, 220, 240);
            lblTitulo.BackColor = Color.Transparent;
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(14, alto - 30);

            tarjeta.Controls.Add(lblIcono);
            tarjeta.Controls.Add(lblValor);
            tarjeta.Controls.Add(lblTitulo);

            // Bordes redondeados con Paint
            tarjeta.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using (Pen pen = new Pen(Color.FromArgb(40, 255, 255, 255), 1))
                {
                    e.Graphics.DrawRectangle(pen, 0, 0,
                        tarjeta.Width - 1, tarjeta.Height - 1);
                }
            };

            this.Controls.Add(tarjeta);
        }






        private void CrearPanelActividad(int x, int y)
        {
            // ── PANEL CONTENEDOR ─────────────────────────────
            Panel panel = new Panel();
            panel.Size = new Size(390, 160);
            panel.Location = new Point(x, y);
            panel.BackColor = Color.FromArgb(10, 22, 40);

            // Borde del panel
            panel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(UITheme.DividerLine, 1))
                    e.Graphics.DrawRectangle(pen, 0, 0,
                        panel.Width - 1, panel.Height - 1);
            };

            // ── TÍTULO DEL PANEL ─────────────────────────────
            Label lblTitulo = new Label();
            lblTitulo.Text = "🕐  Actividad reciente";
            lblTitulo.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblTitulo.ForeColor = UITheme.TextMuted;
            lblTitulo.BackColor = Color.Transparent;
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(12, 10);
            panel.Controls.Add(lblTitulo);

            // Línea bajo el título
            Panel linea = new Panel();
            linea.BackColor = UITheme.DividerLine;
            linea.Size = new Size(panel.Width - 24, 1);
            linea.Location = new Point(12, 30);
            panel.Controls.Add(linea);

            // ── FILAS DE ACTIVIDAD ───────────────────────────
            if (ArchivosRecientes.Count == 0)
            {
                Label lblVacio = new Label();
                lblVacio.Text = "Sin actividad reciente";
                lblVacio.Font = new Font("Segoe UI", 9f, FontStyle.Italic);
                lblVacio.ForeColor = UITheme.TextMuted;
                lblVacio.BackColor = Color.Transparent;
                lblVacio.AutoSize = true;
                lblVacio.Location = new Point(12, 50);
                panel.Controls.Add(lblVacio);
            }
            else
            {
                int yFila = 38;
                foreach (var archivo in ArchivosRecientes.Take(3))
                {
                    // Punto de color según tipo
                    Panel punto = new Panel();
                    punto.Size = new Size(8, 8);
                    punto.Location = new Point(12, yFila + 4);
                    Color colorTipo = archivo.Tipo == "Foto" ? UITheme.AccentBlue :
                                      archivo.Tipo == "Musica" ? Color.FromArgb(124, 92, 219) :
                                                                 Color.FromArgb(61, 191, 168);
                    punto.Paint += (s, e) =>
                    {
                        e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                        using (var brush = new SolidBrush(colorTipo))
                            e.Graphics.FillEllipse(brush, 0, 0, 7, 7);
                    };

                    Label lblNombre = new Label();
                    lblNombre.Text = archivo.Nombre;
                    lblNombre.Font = new Font("Segoe UI", 9.5f);
                    lblNombre.ForeColor = UITheme.TextSecondary;
                    lblNombre.BackColor = Color.Transparent;
                    lblNombre.Size = new Size(280, 18);
                    lblNombre.Location = new Point(26, yFila + 2);

                    Label lblFecha = new Label();
                    lblFecha.Text = archivo.Fecha.ToString("dd MMM",
                                         new System.Globalization.CultureInfo("es-MX"));
                    lblFecha.Font = new Font("Segoe UI", 8.5f);
                    lblFecha.ForeColor = UITheme.TextMuted;
                    lblFecha.BackColor = Color.Transparent;
                    lblFecha.AutoSize = true;
                    lblFecha.Location = new Point(320, yFila + 2);

                    panel.Controls.Add(punto);
                    panel.Controls.Add(lblNombre);
                    panel.Controls.Add(lblFecha);

                    yFila += 32;
                }
            }

            this.Controls.Add(panel);
        }


        private void CrearPanelDistribucion(int x, int y)
        {
            int total = TotalArchivos == 0 ? 1 : TotalArchivos;

            Panel panel = new Panel();
            panel.Size = new Size(300, 160);
            panel.Location = new Point(x, y);
            panel.BackColor = Color.FromArgb(10, 22, 40);

            panel.Paint += (s, e) =>
            {
                using (Pen pen = new Pen(UITheme.DividerLine, 1))
                    e.Graphics.DrawRectangle(pen, 0, 0,
                        panel.Width - 1, panel.Height - 1);
            };

            Label lblTitulo = new Label();
            lblTitulo.Text = "📊  Distribución por tipo";
            lblTitulo.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            lblTitulo.ForeColor = UITheme.TextMuted;
            lblTitulo.BackColor = Color.Transparent;
            lblTitulo.AutoSize = true;
            lblTitulo.Location = new Point(12, 10);
            panel.Controls.Add(lblTitulo);

            Panel linea = new Panel();
            linea.BackColor = UITheme.DividerLine;
            linea.Size = new Size(panel.Width - 24, 1);
            linea.Location = new Point(12, 30);
            panel.Controls.Add(linea);

            // ── BARRAS DE DISTRIBUCIÓN ───────────────────────
            var items = new[]
            {
        ("📷 Fotos",   TotalFotos,  UITheme.AccentBlue),
        ("🎵 Música",  TotalMusica, Color.FromArgb(124, 92, 219)),
        ("🎬 Videos",  TotalVideos, Color.FromArgb(61, 191, 168))
    };

            int yBarra = 40;
            foreach (var (etiqueta, cantidad, color) in items)
            {
                Label lbl = new Label();
                lbl.Text = etiqueta;
                lbl.Font = new Font("Segoe UI", 9f);
                lbl.ForeColor = UITheme.TextSecondary;
                lbl.BackColor = Color.Transparent;
                lbl.AutoSize = true;
                lbl.Location = new Point(12, yBarra);
                panel.Controls.Add(lbl);

                // Track (fondo)
                Panel track = new Panel();
                track.BackColor = Color.FromArgb(13, 36, 66);
                track.Size = new Size(200, 6);
                track.Location = new Point(12, yBarra + 20);
                panel.Controls.Add(track);

                // Fill (progreso)
                int anchoFill = (int)(200 * ((float)cantidad / total));
                Panel fill = new Panel();
                fill.BackColor = color;
                fill.Size = new Size(Math.Max(anchoFill, 2), 6);
                fill.Location = new Point(0, 0);
                track.Controls.Add(fill);

                // Número
                Label lblNum = new Label();
                lblNum.Text = cantidad.ToString();
                lblNum.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
                lblNum.ForeColor = UITheme.AccentBlue;
                lblNum.BackColor = Color.Transparent;
                lblNum.AutoSize = true;
                lblNum.Location = new Point(220, yBarra + 15);
                panel.Controls.Add(lblNum);

                yBarra += 38;
            }

            this.Controls.Add(panel);
        }




    }
}