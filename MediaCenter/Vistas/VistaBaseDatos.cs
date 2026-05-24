using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MediaCenter.Vistas
{
    public partial class VistaBaseDatos : UserControl
    {
        public VistaBaseDatos()
        {
            InitializeComponent();
        }

        private void VistaBaseDatos_Load(object sender, EventArgs e)
        {


        }

        // Carga todos los archivos de la BD en la tabla
        private void CargarDatos()
        {
            try
            {
                DataTable tabla = MediaCenter.Datos.ConexionSQL.ObtenerTodosLosArchivos();
                dgvArchivos.DataSource = tabla;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos: " + ex.Message);
            }
        }

        private void btnRecargar_Click(object sender, EventArgs e)
        {
            CargarDatos();
            MessageBox.Show("Datos recargados");
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvArchivos.CurrentRow == null)
            {
                MessageBox.Show("Selecciona una fila primero");
                return;
            }

            // Pedir confirmación
            DialogResult respuesta = MessageBox.Show(
                "¿Estás seguro de eliminar este registro?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (respuesta != DialogResult.Yes) return;

            try
            {
                int id = Convert.ToInt32(dgvArchivos.CurrentRow.Cells["IdArchivo"].Value);
                MediaCenter.Datos.ConexionSQL.EliminarArchivo(id);

                CargarDatos();
                MessageBox.Show("Registro eliminado correctamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message);
            }

        }

        private void btnImportarCSV_Click(object sender, EventArgs e)
        {

            OpenFileDialog dialogo = new OpenFileDialog();
            dialogo.Filter = "Archivos CSV|*.csv";

            if (dialogo.ShowDialog() != DialogResult.OK) return;

            try
            {
                string[] lineas = File.ReadAllLines(dialogo.FileName);
                int insertados = 0;

                // Empezamos en 1 para saltar el encabezado
                for (int i = 1; i < lineas.Length; i++)
                {
                    string[] datos = lineas[i].Split(',');

                    // Verificar que tenga las 6 columnas esperadas
                    if (datos.Length < 6) continue;

                    string nombre = datos[0].Trim();
                    string ruta = datos[1].Trim();
                    string tipo = datos[2].Trim();
                    string extension = datos[3].Trim();
                    decimal tamanoKB = decimal.Parse(datos[4].Trim(),
                        System.Globalization.CultureInfo.InvariantCulture);
                    bool estaCorrupto = datos[5].Trim() == "1";

                    MediaCenter.Datos.ConexionSQL.InsertarArchivo(
                        nombre, ruta, tipo, extension, tamanoKB, estaCorrupto);
                    insertados++;
                }

                CargarDatos();
                MessageBox.Show("Se importaron " + insertados + " registros");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al importar: " + ex.Message);
            }
        }

        private void btnExportarCSV_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialogo = new SaveFileDialog();
            dialogo.Filter = "Archivos CSV|*.csv";
            dialogo.FileName = "archivos_exportados.csv";

            if (dialogo.ShowDialog() != DialogResult.OK) return;

            try
            {
                DataTable tabla = MediaCenter.Datos.ConexionSQL.ObtenerTodosLosArchivos();

                using (StreamWriter writer = new StreamWriter(dialogo.FileName))
                {
                    // Escribir encabezados
                    for (int i = 0; i < tabla.Columns.Count; i++)
                    {
                        writer.Write(tabla.Columns[i].ColumnName);
                        if (i < tabla.Columns.Count - 1) writer.Write(",");
                    }
                    writer.WriteLine();

                    // Escribir filas
                    foreach (DataRow fila in tabla.Rows)
                    {
                        for (int i = 0; i < tabla.Columns.Count; i++)
                        {
                            writer.Write(fila[i].ToString());
                            if (i < tabla.Columns.Count - 1) writer.Write(",");
                        }
                        writer.WriteLine();
                    }
                }

                MessageBox.Show("Exportado correctamente a:\n" + dialogo.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al exportar: " + ex.Message);
            }

        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            // Pedir cada dato con cuadros de diálogo
            string nombre = Microsoft.VisualBasic.Interaction.InputBox(
                "Nombre del archivo:", "Nuevo registro", "ejemplo.jpg");
            if (string.IsNullOrWhiteSpace(nombre)) return;

            string ruta = Microsoft.VisualBasic.Interaction.InputBox(
                "Ruta completa:", "Nuevo registro", "C:\\MediaCenter_Demo\\Fotos\\ejemplo.jpg");
            if (string.IsNullOrWhiteSpace(ruta)) return;

            string tipo = Microsoft.VisualBasic.Interaction.InputBox(
                "Tipo (Foto, Musica, Video):", "Nuevo registro", "Foto");
            if (string.IsNullOrWhiteSpace(tipo)) return;

            string extension = Microsoft.VisualBasic.Interaction.InputBox(
                "Extensión (jpg, mp3, mp4):", "Nuevo registro", "jpg");
            if (string.IsNullOrWhiteSpace(extension)) return;

            string tamStr = Microsoft.VisualBasic.Interaction.InputBox(
                "Tamaño en KB:", "Nuevo registro", "1024.00");
            if (string.IsNullOrWhiteSpace(tamStr)) return;

            if (!decimal.TryParse(tamStr, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out decimal tamano))
            {
                MessageBox.Show("El tamaño debe ser un número válido");
                return;
            }

            try
            {
                MediaCenter.Datos.ConexionSQL.InsertarArchivo(
                    nombre, ruta, tipo, extension, tamano, false);

                CargarDatos();
                MessageBox.Show("Registro agregado correctamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar: " + ex.Message);
            }

        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (dgvArchivos.CurrentRow == null)
            {
                MessageBox.Show("Selecciona una fila primero");
                return;
            }

            try
            {
                // Obtener los valores actuales
                int id = Convert.ToInt32(dgvArchivos.CurrentRow.Cells["IdArchivo"].Value);
                string nombreActual = dgvArchivos.CurrentRow.Cells["Nombre"].Value.ToString();
                string rutaActual = dgvArchivos.CurrentRow.Cells["RutaCompleta"].Value.ToString();
                string tipoActual = dgvArchivos.CurrentRow.Cells["Tipo"].Value.ToString();
                string extensionActual = dgvArchivos.CurrentRow.Cells["Extension"].Value.ToString();
                decimal tamanoActual = Convert.ToDecimal(dgvArchivos.CurrentRow.Cells["TamanoKB"].Value);
                bool corruptoActual = Convert.ToBoolean(dgvArchivos.CurrentRow.Cells["EstaCorrupto"].Value);

                // Pedir nuevos valores (con el actual como valor por defecto)
                string nombre = Microsoft.VisualBasic.Interaction.InputBox(
                    "Nombre:", "Modificar", nombreActual);
                if (string.IsNullOrWhiteSpace(nombre)) return;

                string ruta = Microsoft.VisualBasic.Interaction.InputBox(
                    "Ruta:", "Modificar", rutaActual);
                if (string.IsNullOrWhiteSpace(ruta)) return;

                string tipo = Microsoft.VisualBasic.Interaction.InputBox(
                    "Tipo:", "Modificar", tipoActual);
                if (string.IsNullOrWhiteSpace(tipo)) return;

                string extension = Microsoft.VisualBasic.Interaction.InputBox(
                    "Extensión:", "Modificar", extensionActual);
                if (string.IsNullOrWhiteSpace(extension)) return;

                string tamStr = Microsoft.VisualBasic.Interaction.InputBox(
                    "Tamaño KB:", "Modificar", tamanoActual.ToString());
                if (!decimal.TryParse(tamStr, System.Globalization.NumberStyles.Any,
                    System.Globalization.CultureInfo.InvariantCulture, out decimal tamano))
                {
                    MessageBox.Show("El tamaño debe ser un número");
                    return;
                }

                MediaCenter.Datos.ConexionSQL.ActualizarArchivo(
                    id, nombre, ruta, tipo, extension, tamano, corruptoActual);

                CargarDatos();
                MessageBox.Show("Registro modificado correctamente");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al modificar: " + ex.Message);
            }

        }



    }
}
