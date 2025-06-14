using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.IO;

namespace Nikki_Pulls
{
    /// <summary>
    /// Lógica de interacción para History.xaml
    /// </summary>
    public partial class History : Window
    {
        public class Pull
        {
            public int Stars { get; set; }
            public string Date { get; set; }
            public int Pity { get; set; }
        }

        public History()
        {
            InitializeComponent();
            LoadPullsData();
        }

        private void LoadPullsData()
        { 
            string xmlPath = "PullsData.xml";
            if (!File.Exists(xmlPath))
            {
                MessageBox.Show("No se encontró el archivo PullsData.xml.");
              //  MessageBox.Show(Environment.CurrentDirectory);


                return;
            }

            try
            {
                XDocument doc = XDocument.Load(xmlPath);
                var pulls = doc.Descendants("Pull")
                    .Select(x => new Pull
                    {
                        Stars = (int)x.Element("Stars"),
                        Date = (string)x.Element("Date"),
                        Pity = (int)x.Element("Pity")
                    })
                    .ToList();

                PullsDataGrid.ItemsSource = pulls;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los datos: " + ex.Message);
            }
        }

        private void PullsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnClean_Click(object sender, RoutedEventArgs e)
        {
            string xmlPath = "PullsData.xml";
            if (!File.Exists(xmlPath))
            {
                MessageBox.Show("No se encontró el archivo PullsData.xml.");
                return;
            }

            try
            {
                // Crear un documento XML vacío con la raíz esperada
                var doc = new XDocument(new XElement("Pulls"));
                doc.Save(xmlPath);

                // Limpiar el DataGrid
                PullsDataGrid.ItemsSource = null;

                MessageBox.Show("Historial limpiado correctamente.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al limpiar el historial: " + ex.Message);
            }
        }
    } 
        }
