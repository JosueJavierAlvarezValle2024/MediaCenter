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
    public partial class VistaMusica : UserControl
    {
        // Diccionario que guarda cada lista con sus canciones
        private System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<ItemCancion>> listasReproduccion
            = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<ItemCancion>>();
        public VistaMusica()
        {
            InitializeComponent();
        }

        private void btnAgregarCancion_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialogo = new OpenFileDialog();
            dialogo.Filter = "Música|*.mp3;*.wav;*.wma";
            dialogo.Multiselect = true;

            if (dialogo.ShowDialog() == DialogResult.OK)
            {
                foreach (string ruta in dialogo.FileNames)
                {
                    string textoMostrar;
                    try
                    {
                        var archivo = TagLib.File.Create(ruta);
                        string titulo = string.IsNullOrEmpty(archivo.Tag.Title)
                            ? Path.GetFileNameWithoutExtension(ruta)
                            : archivo.Tag.Title;
                        string artista = string.IsNullOrEmpty(archivo.Tag.FirstPerformer)
                            ? "Desconocido"
                            : archivo.Tag.FirstPerformer;

                        textoMostrar = titulo + " - " + artista;
                    }
                    catch
                    {
                        textoMostrar = Path.GetFileName(ruta);
                    }

                    // Agregar un objeto que tiene texto Y ruta
                    lstCanciones.Items.Add(new ItemCancion(textoMostrar, ruta));
                }
            }
        }

        // Clase auxiliar para guardar texto visible + ruta real
        public class ItemCancion
        {
            public string Texto { get; set; }
            public string Ruta { get; set; }

            public ItemCancion(string texto, string ruta)
            {
                Texto = texto;
                Ruta = ruta;
            }

            // Esto define cómo se muestra en el ListBox
            public override string ToString()
            {
                return Texto;
            }
        }

        private void lstCanciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstCanciones.SelectedItem == null) return;

            ItemCancion item = (ItemCancion)lstCanciones.SelectedItem;
            string ruta = item.Ruta;

            // Reproducir la canción
            wmPlayer.URL = ruta;

            // Leer y mostrar metadatos
            try
            {
                var archivo = TagLib.File.Create(ruta);

                string info = "Título: " + (archivo.Tag.Title ?? "Desconocido") + "\n";
                info += "Artista: " + (archivo.Tag.FirstPerformer ?? "Desconocido") + "\n";
                info += "Álbum: " + (archivo.Tag.Album ?? "Desconocido") + "\n";
                info += "Año: " + (archivo.Tag.Year == 0 ? "Desconocido" : archivo.Tag.Year.ToString()) + "\n";
                info += "Género: " + (archivo.Tag.FirstGenre ?? "Desconocido") + "\n";
                info += "Duración: " + archivo.Properties.Duration.ToString(@"mm\:ss");

                lblInfoCancion.Text = info;

                // Mostrar la carátula si existe
                if (archivo.Tag.Pictures.Length > 0)
                {
                    var picData = archivo.Tag.Pictures[0].Data.Data;
                    using (var ms = new System.IO.MemoryStream(picData))
                    {
                        picCaratula.Image = System.Drawing.Image.FromStream(ms);
                    }
                }
                else
                {
                    picCaratula.Image = null;
                }
            }
            catch (Exception ex)
            {
                lblInfoCancion.Text = "Error al leer metadatos: " + ex.Message;
            }

        }

        private void btnNuevaLista_Click(object sender, EventArgs e)
        {
            string nombre = Microsoft.VisualBasic.Interaction.InputBox(
             "Nombre de la nueva lista:",
             "Crear lista", "Mi lista");

            if (string.IsNullOrWhiteSpace(nombre)) return;

            // Agregar al ComboBox
            cmbListas.Items.Add(nombre);
            cmbListas.SelectedItem = nombre;

            MessageBox.Show("Lista '" + nombre + "' creada");
        }

        private void btnAgregarALista_Click(object sender, EventArgs e)
        {
            if (cmbListas.SelectedItem == null)
            {
                MessageBox.Show("Primero selecciona una lista del menú desplegable");
                return;
            }

            if (lstCanciones.SelectedItem == null)
            {
                MessageBox.Show("Primero selecciona una canción de la lista de arriba");
                return;
            }

            ItemCancion cancion = (ItemCancion)lstCanciones.SelectedItem;
            string nombreLista = cmbListas.SelectedItem.ToString();

            // Guardar en el diccionario de listas
            if (!listasReproduccion.ContainsKey(nombreLista))
                listasReproduccion[nombreLista] = new System.Collections.Generic.List<ItemCancion>();

            listasReproduccion[nombreLista].Add(cancion);

            MessageBox.Show("Canción agregada a la lista '" + nombreLista + "'");
        }

        private void btnVerLista_Click(object sender, EventArgs e)
        {
            if (cmbListas.SelectedItem == null)
            {
                MessageBox.Show("Selecciona una lista primero");
                return;
            }

            string nombreLista = cmbListas.SelectedItem.ToString();

            if (!listasReproduccion.ContainsKey(nombreLista) || listasReproduccion[nombreLista].Count == 0)
            {
                MessageBox.Show("La lista '" + nombreLista + "' está vacía");
                return;
            }

            // Reemplazar las canciones del ListBox con las de esta lista
            lstCanciones.Items.Clear();
            foreach (var cancion in listasReproduccion[nombreLista])
            {
                lstCanciones.Items.Add(cancion);
            }

            MessageBox.Show("Mostrando lista: " + nombreLista);
        }



    }
}
