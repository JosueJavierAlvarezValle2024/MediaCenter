namespace MediaCenter.Vistas
{
    partial class VistaMusica
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VistaMusica));
            btnAgregarCancion = new Button();
            lstCanciones = new ListBox();
            picCaratula = new PictureBox();
            lblInfoCancion = new Label();
            wmPlayer = new AxWMPLib.AxWindowsMediaPlayer();
            lblListas = new Label();
            cmbListas = new ComboBox();
            btnNuevaLista = new Button();
            btnAgregarALista = new Button();
            btnVerLista = new Button();
            ((System.ComponentModel.ISupportInitialize)picCaratula).BeginInit();
            ((System.ComponentModel.ISupportInitialize)wmPlayer).BeginInit();
            SuspendLayout();
            // 
            // btnAgregarCancion
            // 
            btnAgregarCancion.Location = new Point(15, 15);
            btnAgregarCancion.Name = "btnAgregarCancion";
            btnAgregarCancion.Size = new Size(150, 35);
            btnAgregarCancion.TabIndex = 0;
            btnAgregarCancion.Text = "Agregar Cancion";
            btnAgregarCancion.UseVisualStyleBackColor = true;
            btnAgregarCancion.Click += btnAgregarCancion_Click;
            // 
            // lstCanciones
            // 
            lstCanciones.FormattingEnabled = true;
            lstCanciones.Location = new Point(15, 60);
            lstCanciones.Name = "lstCanciones";
            lstCanciones.Size = new Size(300, 464);
            lstCanciones.TabIndex = 1;
            lstCanciones.SelectedIndexChanged += lstCanciones_SelectedIndexChanged;
            // 
            // picCaratula
            // 
            picCaratula.BorderStyle = BorderStyle.FixedSingle;
            picCaratula.Location = new Point(340, 60);
            picCaratula.Name = "picCaratula";
            picCaratula.Size = new Size(250, 250);
            picCaratula.SizeMode = PictureBoxSizeMode.Zoom;
            picCaratula.TabIndex = 2;
            picCaratula.TabStop = false;
            // 
            // lblInfoCancion
            // 
            lblInfoCancion.BorderStyle = BorderStyle.FixedSingle;
            lblInfoCancion.Location = new Point(610, 60);
            lblInfoCancion.Name = "lblInfoCancion";
            lblInfoCancion.Size = new Size(270, 250);
            lblInfoCancion.TabIndex = 3;
            lblInfoCancion.Text = "Seleccion una cancion";
            // 
            // wmPlayer
            // 
            wmPlayer.Enabled = true;
            wmPlayer.Location = new Point(340, 330);
            wmPlayer.Name = "wmPlayer";
            wmPlayer.OcxState = (AxHost.State)resources.GetObject("wmPlayer.OcxState");
            wmPlayer.Size = new Size(540, 280);
            wmPlayer.TabIndex = 4;
            // 
            // lblListas
            // 
            lblListas.AutoSize = true;
            lblListas.Location = new Point(15, 555);
            lblListas.Name = "lblListas";
            lblListas.Size = new Size(145, 20);
            lblListas.TabIndex = 5;
            lblListas.Text = "Listas de produccion";
            // 
            // cmbListas
            // 
            cmbListas.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbListas.FormattingEnabled = true;
            cmbListas.Location = new Point(15, 580);
            cmbListas.Name = "cmbListas";
            cmbListas.Size = new Size(200, 28);
            cmbListas.TabIndex = 6;
            // 
            // btnNuevaLista
            // 
            btnNuevaLista.Location = new Point(15, 615);
            btnNuevaLista.Name = "btnNuevaLista";
            btnNuevaLista.Size = new Size(60, 30);
            btnNuevaLista.TabIndex = 7;
            btnNuevaLista.Text = "Nueva";
            btnNuevaLista.UseVisualStyleBackColor = true;
            btnNuevaLista.Click += btnNuevaLista_Click;
            // 
            // btnAgregarALista
            // 
            btnAgregarALista.Location = new Point(80, 615);
            btnAgregarALista.Name = "btnAgregarALista";
            btnAgregarALista.Size = new Size(75, 30);
            btnAgregarALista.TabIndex = 8;
            btnAgregarALista.Text = "+ Cancion";
            btnAgregarALista.UseVisualStyleBackColor = true;
            btnAgregarALista.Click += btnAgregarALista_Click;
            // 
            // btnVerLista
            // 
            btnVerLista.Location = new Point(160, 615);
            btnVerLista.Name = "btnVerLista";
            btnVerLista.Size = new Size(70, 30);
            btnVerLista.TabIndex = 9;
            btnVerLista.Text = "Ver Lista";
            btnVerLista.UseVisualStyleBackColor = true;
            btnVerLista.Click += btnVerLista_Click;
            // 
            // VistaMusica
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(btnVerLista);
            Controls.Add(btnAgregarALista);
            Controls.Add(btnNuevaLista);
            Controls.Add(cmbListas);
            Controls.Add(lblListas);
            Controls.Add(wmPlayer);
            Controls.Add(lblInfoCancion);
            Controls.Add(picCaratula);
            Controls.Add(lstCanciones);
            Controls.Add(btnAgregarCancion);
            Name = "VistaMusica";
            Size = new Size(900, 700);
            ((System.ComponentModel.ISupportInitialize)picCaratula).EndInit();
            ((System.ComponentModel.ISupportInitialize)wmPlayer).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnAgregarCancion;
        private ListBox lstCanciones;
        private PictureBox picCaratula;
        private Label lblInfoCancion;
        private AxWMPLib.AxWindowsMediaPlayer wmPlayer;
        private Label lblListas;
        private ComboBox cmbListas;
        private Button btnNuevaLista;
        private Button btnAgregarALista;
        private Button btnVerLista;
    }
}
