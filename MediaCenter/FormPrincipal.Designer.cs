namespace MediaCenter
{
    partial class FormPrincipal
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            toolStrip1 = new ToolStrip();
            panelMenu = new Panel();
            btnInicio = new Button();
            lblTitulo = new Label();
            btnBaseDatos = new Button();
            btnMusica = new Button();
            btnConfiguracion = new Button();
            btnVideos = new Button();
            btnFotos = new Button();
            panelContenido = new Panel();
            panelMenu.SuspendLayout();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new Size(20, 20);
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(1401, 25);
            toolStrip1.TabIndex = 0;
            toolStrip1.Text = "MediaCenter - Gestor Multimedia Personal";
            // 
            // panelMenu
            // 
            panelMenu.BackColor = Color.FromArgb(44, 62, 80);
            panelMenu.Controls.Add(btnInicio);
            panelMenu.Controls.Add(lblTitulo);
            panelMenu.Controls.Add(btnBaseDatos);
            panelMenu.Controls.Add(btnMusica);
            panelMenu.Controls.Add(btnConfiguracion);
            panelMenu.Controls.Add(btnVideos);
            panelMenu.Controls.Add(btnFotos);
            panelMenu.Dock = DockStyle.Left;
            panelMenu.Location = new Point(0, 25);
            panelMenu.Name = "panelMenu";
            panelMenu.Size = new Size(200, 848);
            panelMenu.TabIndex = 1;
            // 
            // btnInicio
            // 
            btnInicio.Dock = DockStyle.Top;
            btnInicio.Location = new Point(0, 0);
            btnInicio.Name = "btnInicio";
            btnInicio.Size = new Size(200, 29);
            btnInicio.TabIndex = 6;
            btnInicio.Text = "Inicio";
            btnInicio.UseVisualStyleBackColor = true;
            btnInicio.Click += btnInicio_Click;
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = Color.White;
            lblTitulo.Location = new Point(15, 15);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(146, 31);
            lblTitulo.TabIndex = 5;
            lblTitulo.Text = "MediaCenter";
            lblTitulo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnBaseDatos
            // 
            btnBaseDatos.BackColor = Color.FromArgb(52, 152, 219);
            btnBaseDatos.FlatStyle = FlatStyle.Flat;
            btnBaseDatos.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnBaseDatos.ForeColor = Color.White;
            btnBaseDatos.Location = new Point(0, 210);
            btnBaseDatos.Name = "btnBaseDatos";
            btnBaseDatos.Size = new Size(200, 50);
            btnBaseDatos.TabIndex = 4;
            btnBaseDatos.Text = "Base de Datos";
            btnBaseDatos.TextAlign = ContentAlignment.MiddleLeft;
            btnBaseDatos.UseVisualStyleBackColor = false;
            btnBaseDatos.Click += btnBaseDatos_Click;
            // 
            // btnMusica
            // 
            btnMusica.BackColor = Color.FromArgb(52, 152, 219);
            btnMusica.FlatStyle = FlatStyle.Flat;
            btnMusica.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnMusica.ForeColor = Color.White;
            btnMusica.Location = new Point(0, 110);
            btnMusica.Name = "btnMusica";
            btnMusica.Size = new Size(200, 50);
            btnMusica.TabIndex = 3;
            btnMusica.Text = "Musica";
            btnMusica.TextAlign = ContentAlignment.MiddleLeft;
            btnMusica.UseVisualStyleBackColor = false;
            btnMusica.Click += btnMusica_Click;
            // 
            // btnConfiguracion
            // 
            btnConfiguracion.BackColor = Color.FromArgb(52, 152, 219);
            btnConfiguracion.FlatStyle = FlatStyle.Flat;
            btnConfiguracion.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnConfiguracion.ForeColor = Color.White;
            btnConfiguracion.Location = new Point(0, 260);
            btnConfiguracion.Name = "btnConfiguracion";
            btnConfiguracion.Size = new Size(200, 50);
            btnConfiguracion.TabIndex = 2;
            btnConfiguracion.Text = "Configuracion";
            btnConfiguracion.TextAlign = ContentAlignment.MiddleLeft;
            btnConfiguracion.UseVisualStyleBackColor = false;
            btnConfiguracion.Click += btnConfiguracion_Click;
            // 
            // btnVideos
            // 
            btnVideos.BackColor = Color.FromArgb(52, 152, 219);
            btnVideos.FlatStyle = FlatStyle.Flat;
            btnVideos.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnVideos.ForeColor = Color.White;
            btnVideos.Location = new Point(0, 160);
            btnVideos.Name = "btnVideos";
            btnVideos.Size = new Size(200, 50);
            btnVideos.TabIndex = 1;
            btnVideos.Text = "Videos";
            btnVideos.TextAlign = ContentAlignment.MiddleLeft;
            btnVideos.UseVisualStyleBackColor = false;
            btnVideos.Click += btnVideos_Click;
            // 
            // btnFotos
            // 
            btnFotos.BackColor = Color.FromArgb(52, 152, 219);
            btnFotos.FlatStyle = FlatStyle.Flat;
            btnFotos.Font = new Font("Segoe UI", 10.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnFotos.ForeColor = Color.White;
            btnFotos.Location = new Point(0, 60);
            btnFotos.Name = "btnFotos";
            btnFotos.Size = new Size(200, 50);
            btnFotos.TabIndex = 0;
            btnFotos.Text = "Fotos";
            btnFotos.TextAlign = ContentAlignment.MiddleLeft;
            btnFotos.UseVisualStyleBackColor = false;
            btnFotos.Click += btnFotos_Click;
            // 
            // panelContenido
            // 
            panelContenido.BackColor = Color.White;
            panelContenido.Dock = DockStyle.Fill;
            panelContenido.Location = new Point(200, 25);
            panelContenido.Name = "panelContenido";
            panelContenido.Size = new Size(1201, 848);
            panelContenido.TabIndex = 2;
            // 
            // FormPrincipal
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1401, 873);
            Controls.Add(panelContenido);
            Controls.Add(panelMenu);
            Controls.Add(toolStrip1);
            Name = "FormPrincipal";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            
            panelMenu.ResumeLayout(false);
            panelMenu.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip toolStrip1;
        private Panel panelMenu;
        private Panel panelContenido;
        private Button btnFotos;
        private Button btnBaseDatos;
        private Button btnMusica;
        private Button btnConfiguracion;
        private Button btnVideos;
        private Label lblTitulo;
        private Button btnInicio;
    }
}
