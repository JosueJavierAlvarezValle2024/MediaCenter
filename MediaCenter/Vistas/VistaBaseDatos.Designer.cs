namespace MediaCenter.Vistas
{
    partial class VistaBaseDatos
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
            btnImportarCSV = new Button();
            btnExportarCSV = new Button();
            btnNuevo = new Button();
            btnEliminar = new Button();
            btnRecargar = new Button();
            dgvArchivos = new DataGridView();
            btnModificar = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvArchivos).BeginInit();
            SuspendLayout();
            // 
            // btnImportarCSV
            // 
            btnImportarCSV.Location = new Point(15, 15);
            btnImportarCSV.Name = "btnImportarCSV";
            btnImportarCSV.Size = new Size(120, 35);
            btnImportarCSV.TabIndex = 0;
            btnImportarCSV.Text = "Importar CSV";
            btnImportarCSV.UseVisualStyleBackColor = true;
            btnImportarCSV.Click += btnImportarCSV_Click;
            // 
            // btnExportarCSV
            // 
            btnExportarCSV.Location = new Point(145, 15);
            btnExportarCSV.Name = "btnExportarCSV";
            btnExportarCSV.Size = new Size(120, 35);
            btnExportarCSV.TabIndex = 1;
            btnExportarCSV.Text = "Exportar CSV";
            btnExportarCSV.UseVisualStyleBackColor = true;
            btnExportarCSV.Click += btnExportarCSV_Click;
            // 
            // btnNuevo
            // 
            btnNuevo.Location = new Point(275, 15);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(100, 35);
            btnNuevo.TabIndex = 2;
            btnNuevo.Text = "Nuevo";
            btnNuevo.UseVisualStyleBackColor = true;
            btnNuevo.Click += btnNuevo_Click;
            // 
            // btnEliminar
            // 
            btnEliminar.Location = new Point(385, 15);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(100, 35);
            btnEliminar.TabIndex = 3;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            btnEliminar.Click += btnEliminar_Click;
            // 
            // btnRecargar
            // 
            btnRecargar.Location = new Point(495, 15);
            btnRecargar.Name = "btnRecargar";
            btnRecargar.Size = new Size(100, 35);
            btnRecargar.TabIndex = 4;
            btnRecargar.Text = "Recargar";
            btnRecargar.UseVisualStyleBackColor = true;
            btnRecargar.Click += btnRecargar_Click;
            // 
            // dgvArchivos
            // 
            dgvArchivos.AllowUserToAddRows = false;
            dgvArchivos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvArchivos.Location = new Point(15, 65);
            dgvArchivos.MultiSelect = false;
            dgvArchivos.Name = "dgvArchivos";
            dgvArchivos.RowHeadersWidth = 51;
            dgvArchivos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvArchivos.Size = new Size(870, 620);
            dgvArchivos.TabIndex = 5;
            // 
            // btnModificar
            // 
            btnModificar.Location = new Point(605, 15);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(100, 35);
            btnModificar.TabIndex = 6;
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click;
            // 
            // VistaBaseDatos
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(btnModificar);
            Controls.Add(dgvArchivos);
            Controls.Add(btnRecargar);
            Controls.Add(btnEliminar);
            Controls.Add(btnNuevo);
            Controls.Add(btnExportarCSV);
            Controls.Add(btnImportarCSV);
            Name = "VistaBaseDatos";
            Size = new Size(900, 700);
            Load += VistaBaseDatos_Load;
            ((System.ComponentModel.ISupportInitialize)dgvArchivos).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Button btnImportarCSV;
        private Button btnExportarCSV;
        private Button btnNuevo;
        private Button btnEliminar;
        private Button btnRecargar;
        private DataGridView dgvArchivos;
        private Button btnModificar;
    }
}
