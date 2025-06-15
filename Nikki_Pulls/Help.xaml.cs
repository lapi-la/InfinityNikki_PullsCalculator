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
using System.IO;

namespace Nikki_Pulls
{
    /// <summary>
    /// Lógica de interacción para Window1.xaml
    /// </summary>
    public partial class Help : Window
    {
        public Help()
        {
            InitializeComponent();

            string rutaHtml = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "help.html");
            if (File.Exists(rutaHtml))
            {
              
                var uri = new Uri(rutaHtml, UriKind.Absolute);
                WBHelp.Navigate(uri);
            }
            else
            {
                WBHelp.NavigateToString("<html><body><h2>File not found help.html</h2></body></html>");
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
