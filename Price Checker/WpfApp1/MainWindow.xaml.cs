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
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        static string highalchprices = "newoutput.json";

        public MainWindow()
        {
            InitializeComponent();
        }

        static int natureRunePrice = 0;

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string json = File.ReadAllText(highalchprices);

            List<AlchValues> alchv = new List<AlchValues>();
            
            json = json.Replace("[", "");
            json = json.Replace("]", "");

            string[] temp = json.Split(',');

            List<string> product = new List<string>();

            List<string> tempList = new List<string>(temp);
            while(tempList.Count > 0)
            {
                product.Add(tempList[0] +","+ tempList[1] + ","+ tempList[2]);
                for (int i = 0; i < 3; i++)
                {
                    tempList.RemoveAt(0);
                }
            }

            foreach(string item in product)
            {
                alchv.Add(JsonConvert.DeserializeObject<AlchValues>(item));
            }

            //txtOutput.Text += alchv.Count;

            
            natureRunePrice = GetPriceOfNatureRune();
            Console.WriteLine(natureRunePrice);
            //txtOutput.Text += natureRunePrice;



            foreach(AlchValues alch in alchv)
            {
                int currentPrice = GetPriceOfItem(alch.id);
                Console.WriteLine(alch.id + ": " + alch.name + " " + currentPrice + " " + alch.alch_value);
                if(currentPrice < 0)
                {
                    continue;
                }
                if((currentPrice + natureRunePrice + 200) < alch.alch_value)
                {
                    txtOutput.Text += alch.name + " " + currentPrice + " " + alch.alch_value;
                }
                
            }


                        
        }

        int GetPriceOfItem(int id)
        {
            var w = new WebClient();
            var json_data = string.Empty;
            string url = "http://services.runescape.com/m=itemdb_oldschool/api/catalogue/detail.json?item=" + id.ToString();
            json_data = w.DownloadString(url);
            Console.WriteLine(json_data);
            if (json_data == "")
            {
                
                return -2;
            }

            return ParseNumber(CreateItemFromJson(json_data).current.price);
        }

        int ParseNumber(string num)
        {
            double result;
            if (double.TryParse(num, out result))
            {

                return (int) result;

            }
            else
            {
                if (num.ToLower().Contains("k"))
                {
                    num = num.Replace("k", "");
                    if(double.TryParse(num, out result))
                    {
                        result = result * 1000;
                        return (int) result;
                    }

                }
            }
            return -3;
        }

        int GetPriceOfNatureRune()
        {
            var w = new WebClient();
            var json_data = string.Empty;
            string url = "http://services.runescape.com/m=itemdb_oldschool/api/catalogue/detail.json?item=561";
            json_data = w.DownloadString(url);
            
            return ParseNumber(CreateItemFromJson(json_data).current.price);
                       
        }

        Item CreateItemFromJson(string json)
        {
            Item item = new Item();

            JsonTextReader r = new JsonTextReader(new StringReader(json));

            if(json == "")
            {
                return null;
            }

            item = JsonConvert.DeserializeObject<RootObject>(json).item;
            
            return item;
        }

    }
}
