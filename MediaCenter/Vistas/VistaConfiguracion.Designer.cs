namespace MediaCenter.Vistas
{
    partial class VistaConfiguracion
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            lblTitulo = new Label();
            gbEstadisticas = new GroupBox();
            btnActualizar = new Button();
            lblTotal = new Label();
            lblVideos = new Label();
            lblMusica = new Label();
            lblFotos = new Label();
            gbConexion = new GroupBox();
            lblConexion = new Label();
            btnAcercaDe = new Button();
            gbEstadisticas.SuspendLayout();
            gbConexion.SuspendLayout();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.Location = new Point(23, 27);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(218, 41);
            lblTitulo.TabIndex = 3;
            lblTitulo.Text = "Configuración";
            // 
            // gbEstadisticas
            // 
            gbEstadisticas.Controls.Add(btnActualizar);
            gbEstadisticas.Controls.Add(lblTotal);
            gbEstadisticas.Controls.Add(lblVideos);
            gbEstadisticas.Controls.Add(lblMusica);
            gbEstadisticas.Controls.Add(lblFotos);
            gbEstadisticas.Font = new Font("Segoe UI", 10F);
            gbEstadisticas.Location = new Point(23, 107);
            gbEstadisticas.Margin = new Padding(3, 4, 3, 4);
            gbEstadisticas.Name = "gbEstadisticas";
            gbEstadisticas.Padding = new Padding(3, 4, 3, 4);
            gbEstadisticas.Size = new Size(514, 307);
            gbEstadisticas.TabIndex = 2;
            gbEstadisticas.TabStop = false;
            gbEstadisticas.Text = "Estadísticas de la Base de Datos";
            // 
            // btnActualizar
            // 
            btnActualizar.Location = new Point(23, 233);
            btnActualizar.Margin = new Padding(3, 4, 3, 4);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(229, 47);
            btnActualizar.TabIndex = 0;
            btnActualizar.Text = "Actualizar estadísticas";
            btnActualizar.UseVisualStyleBackColor = true;
            btnActualizar.Click += btnActualizar_Click;
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTotal.Location = new Point(23, 173);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(73, 25);
            lblTotal.TabIndex = 1;
            lblTotal.Text = "Total: -";
            // 
            // lblVideos
            // 
            lblVideos.AutoSize = true;
            lblVideos.Location = new Point(23, 127);
            lblVideos.Name = "lblVideos";
            lblVideos.Size = new Size(77, 23);
            lblVideos.TabIndex = 2;
            lblVideos.Text = "Videos: -";
            // 
            // lblMusica
            // 
            lblMusica.AutoSize = true;
            lblMusica.Location = new Point(23, 87);
            lblMusica.Name = "lblMusica";
            lblMusica.Size = new Size(79, 23);
            lblMusica.TabIndex = 3;
            lblMusica.Text = "Música: -";
            // 
            // lblFotos
            // 
            lblFotos.AutoSize = true;
            lblFotos.Location = new Point(23, 47);
            lblFotos.Name = "lblFotos";
            lblFotos.Size = new Size(67, 23);
            lblFotos.TabIndex = 4;
            lblFotos.Text = "Fotos: -";
            // 
            // gbConexion
            // 
            gbConexion.Controls.Add(lblConexion);
            gbConexion.Font = new Font("Segoe UI", 10F);
            gbConexion.Location = new Point(23, 440);
            gbConexion.Margin = new Padding(3, 4, 3, 4);
            gbConexion.Name = "gbConexion";
            gbConexion.Padding = new Padding(3, 4, 3, 4);
            gbConexion.Size = new Size(857, 160);
            gbConexion.TabIndex = 1;
            gbConexion.TabStop = false;
            gbConexion.Text = "Conexión SQL";
            // 
            // lblConexion
            // 
            lblConexion.Location = new Point(17, 47);
            lblConexion.Name = "lblConexion";
            lblConexion.Size = new Size(823, 100);
            lblConexion.TabIndex = 0;
            lblConexion.Text = "Cadena: -";
            // 
            // btnAcercaDe
            // 
            btnAcercaDe.Font = new Font("Segoe UI", 10F);
            btnAcercaDe.Location = new Point(23, 640);
            btnAcercaDe.Margin = new Padding(3, 4, 3, 4);
            btnAcercaDe.Name = "btnAcercaDe";
            btnAcercaDe.Size = new Size(171, 53);
            btnAcercaDe.TabIndex = 0;
            btnAcercaDe.Text = "Acerca de";
            btnAcercaDe.UseVisualStyleBackColor = true;
            btnAcercaDe.Click += btnAcercaDe_Click;
            // 
            // VistaConfiguracion
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(btnAcercaDe);
            Controls.Add(gbConexion);
            Controls.Add(gbEstadisticas);
            Controls.Add(lblTitulo);
            Margin = new Padding(3, 4, 3, 4);
            Name = "VistaConfiguracion";
            Size = new Size(914, 800);
            gbEstadisticas.ResumeLayout(false);
            gbEstadisticas.PerformLayout();
            gbConexion.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.GroupBox gbEstadisticas;
        private System.Windows.Forms.Button btnActualizar;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblVideos;
        private System.Windows.Forms.Label lblMusica;
        private System.Windows.Forms.Label lblFotos;
        private System.Windows.Forms.GroupBox gbConexion;
        private System.Windows.Forms.Label lblConexion;
        private System.Windows.Forms.Button btnAcercaDe;
    }
}