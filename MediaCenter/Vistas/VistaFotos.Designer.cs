namespace MediaCenter.Vistas
{
    partial class VistaFotos
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            btnAgregarFoto = new Button();
            lstFotos = new ListBox();
            picVisor = new PictureBox();
            lblInfoFoto = new Label();
            btnEditarGPS = new Button();
            webMapa = new Microsoft.Web.WebView2.WinForms.WebView2();
            btnImportarCarpeta = new Button();
            ((System.ComponentModel.ISupportInitialize)picVisor).BeginInit();
            ((System.ComponentModel.ISupportInitialize)webMapa).BeginInit();
            SuspendLayout();
            // 
            // btnAgregarFoto
            // 
            btnAgregarFoto.Location = new Point(15, 15);
            btnAgregarFoto.Name = "btnAgregarFoto";
            btnAgregarFoto.Size = new Size(150, 35);
            btnAgregarFoto.TabIndex = 0;
            btnAgregarFoto.Text = "Agregar Foto";
            btnAgregarFoto.UseVisualStyleBackColor = true;
            btnAgregarFoto.Click += btnAgregarFoto_Click;
            // 
            // lstFotos
            // 
            lstFotos.FormattingEnabled = true;
            lstFotos.Location = new Point(15, 60);
            lstFotos.Name = "lstFotos";
            lstFotos.Size = new Size(200, 604);
            lstFotos.TabIndex = 1;
            lstFotos.SelectedIndexChanged += lstFotos_SelectedIndexChanged;
            // 
            // picVisor
            // 
            picVisor.BorderStyle = BorderStyle.FixedSingle;
            picVisor.Location = new Point(230, 60);
            picVisor.Name = "picVisor";
            picVisor.Size = new Size(650, 250);
            picVisor.SizeMode = PictureBoxSizeMode.Zoom;
            picVisor.TabIndex = 2;
            picVisor.TabStop = false;
            // 
            // lblInfoFoto
            // 
            lblInfoFoto.BorderStyle = BorderStyle.FixedSingle;
            lblInfoFoto.Location = new Point(230, 320);
            lblInfoFoto.Name = "lblInfoFoto";
            lblInfoFoto.Size = new Size(650, 80);
            lblInfoFoto.TabIndex = 3;
            lblInfoFoto.Text = "Seleccionar una foto para ver sus datos";
            // 
            // btnEditarGPS
            // 
            btnEditarGPS.Location = new Point(230, 410);
            btnEditarGPS.Name = "btnEditarGPS";
            btnEditarGPS.Size = new Size(180, 30);
            btnEditarGPS.TabIndex = 4;
            btnEditarGPS.Text = "Editar coordenadas";
            btnEditarGPS.UseVisualStyleBackColor = true;
            btnEditarGPS.Click += btnEditarGPS_Click;
            // 
            // webMapa
            // 
            webMapa.AllowExternalDrop = true;
            webMapa.CreationProperties = null;
            webMapa.DefaultBackgroundColor = Color.White;
            webMapa.Location = new Point(230, 450);
            webMapa.Name = "webMapa";
            webMapa.Size = new Size(650, 230);
            webMapa.TabIndex = 5;
            webMapa.ZoomFactor = 1D;
            // 
            // btnImportarCarpeta
            // 
            btnImportarCarpeta.Location = new Point(295, 21);
            btnImportarCarpeta.Name = "btnImportarCarpeta";
            btnImportarCarpeta.Size = new Size(94, 29);
            btnImportarCarpeta.TabIndex = 6;
            btnImportarCarpeta.Text = "Importar Carpeta";
            btnImportarCarpeta.UseVisualStyleBackColor = true;
            btnImportarCarpeta.Click += btnImportarCarpeta_Click;
            // 
            // VistaFotos
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(btnImportarCarpeta);
            Controls.Add(webMapa);
            Controls.Add(btnEditarGPS);
            Controls.Add(lblInfoFoto);
            Controls.Add(picVisor);
            Controls.Add(lstFotos);
            Controls.Add(btnAgregarFoto);
            Name = "VistaFotos";
            Size = new Size(900, 700);
            Load += VistaFotos_Load;
            ((System.ComponentModel.ISupportInitialize)picVisor).EndInit();
            ((System.ComponentModel.ISupportInitialize)webMapa).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btnAgregarFoto;
        private ListBox lstFotos;
        private PictureBox picVisor;
        private Label lblInfoFoto;
        private Button btnEditarGPS;
        private Microsoft.Web.WebView2.WinForms.WebView2 webMapa;
        private Button btnImportarCarpeta;
    }
}
