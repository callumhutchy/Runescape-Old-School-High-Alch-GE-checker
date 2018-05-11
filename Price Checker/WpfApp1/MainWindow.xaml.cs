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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

            var w = new WebClient();
            var json_data = string.Empty;
            string url = "http://services.runescape.com/m=itemdb_oldschool/api/catalogue/detail.json?item=" + txtId.Text;
            txtOutput.Text += url;
                json_data = w.DownloadString(url);
         

            txtOutput.Text += json_data;



            
        }
    }
}
