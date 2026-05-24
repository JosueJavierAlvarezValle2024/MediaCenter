namespace MediaCenter.Vistas
{
    partial class VistaVideos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VistaVideos));
            btnAgregarVideo = new Button();
            lstVideos = new ListBox();
            wmVideo = new AxWMPLib.AxWindowsMediaPlayer();
            lblInfoVideo = new Label();
            ((System.ComponentModel.ISupportInitialize)wmVideo).BeginInit();
            SuspendLayout();
            // 
            // btnAgregarVideo
            // 
            btnAgregarVideo.Location = new Point(15, 15);
            btnAgregarVideo.Name = "btnAgregarVideo";
            btnAgregarVideo.Size = new Size(150, 35);
            btnAgregarVideo.TabIndex = 0;
            btnAgregarVideo.Text = "Agregar Video";
            btnAgregarVideo.UseVisualStyleBackColor = true;
            btnAgregarVideo.Click += btnAgregarVideo_Click;
            // 
            // lstVideos
            // 
            lstVideos.FormattingEnabled = true;
            lstVideos.Location = new Point(15, 60);
            lstVideos.Name = "lstVideos";
            lstVideos.Size = new Size(250, 604);
            lstVideos.TabIndex = 1;
            lstVideos.SelectedIndexChanged += lstVideos_SelectedIndexChanged;
            // 
            // wmVideo
            // 
            wmVideo.Enabled = true;
            wmVideo.Location = new Point(280, 60);
            wmVideo.Name = "wmVideo";
            wmVideo.OcxState = (AxHost.State)resources.GetObject("wmVideo.OcxState");
            wmVideo.Size = new Size(600, 450);
            wmVideo.TabIndex = 2;
            // 
            // lblInfoVideo
            // 
            lblInfoVideo.AutoSize = true;
            lblInfoVideo.BorderStyle = BorderStyle.FixedSingle;
            lblInfoVideo.Location = new Point(280, 520);
            lblInfoVideo.Name = "lblInfoVideo";
            lblInfoVideo.Size = new Size(143, 22);
            lblInfoVideo.TabIndex = 3;
            lblInfoVideo.Text = "Selecciona un video";
            // 
            // VistaVideos
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(lblInfoVideo);
            Controls.Add(wmVideo);
            Controls.Add(lstVideos);
            Controls.Add(btnAgregarVideo);
            Name = "VistaVideos";
            Size = new Size(900, 700);
            ((System.ComponentModel.ISupportInitialize)wmVideo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnAgregarVideo;
        private ListBox lstVideos;
        private AxWMPLib.AxWindowsMediaPlayer wmVideo;
        private Label lblInfoVideo;
    }
}
