using System;
using Newtonsoft.Json;
using HtmlAgilityPack;
using System.Net;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace ConsoleApp1
{
    class Program
    {
        static string itemsFile = "objects_87.json";
        static List<Tuple<int, string>> itemsFromFile = new List<Tuple<int, string>>();
        static List<RootObject> itemsWithAlch = new List<RootObject>();
        static void Main(string[] args)
        {
            GetAllItems();

            GetHighAlchValues();

            WriteToNewFile();

            Console.ReadLine();
            






        }

        public static void WriteToNewFile()
        {
            string location = "output.json";
            StringBuilder sb = new StringBuilder();

            sb.Append("[");

            foreach(RootObject ro in itemsWithAlch)
            {
                sb.Append("{");


                sb.Append("\"id\": " + ro.id+ ",");
                sb.Append("\"name\": \"" + ro.name + "\",");
                sb.Append("\alch_value\": " + ro.alch_value);


                sb.Append("}");
            }

            sb.Append("]");

            using (StreamWriter swriter = new StreamWriter(location))
            {
                swriter.Write(sb.ToString());
            }

            Console.WriteLine("Wrote new file");


        }

        public static void GetHighAlchValues()
        {
            Console.WriteLine("Getting high alch values");
            foreach (Tuple<int, string> item in itemsFromFile)
            {
                int value = HighAlch(item.Item2);

                if(value > 0)
                {
                    itemsWithAlch.Add(new RootObject(item.Item1, item.Item2, value));
                }
            }
        }

        public static void GetAllItems()
        {
            Console.WriteLine("Getting items from file");
            string json = File.ReadAllText(itemsFile);
            
            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            
            while (reader.Read())
            {
                if (reader.TokenType.ToString() == "StartObject")
                {

                    reader.Read();
                    if ((string)reader.Value == "id")
                    {
                        reader.Read();
                        Console.WriteLine(reader.Value);
                        int id = int.Parse(reader.Value.ToString());

                        reader.Read();

                        if ((string)reader.Value == "name")
                        {
                            reader.Read();

                            string name = (string)reader.Value;

                            itemsFromFile.Add(new Tuple<int, string>(id, name));

                            reader.Read();

                            if (reader.TokenType.ToString() == "EndObject")
                            {

                            }

                        }


                    }


                }
                else
                {
                    Console.WriteLine("Token: {0}", reader.TokenType);
                }
            }
            
        }

       public static int HighAlch(string name)
        {
            var w = new WebClient();
            var json_data = string.Empty;
            string url = "http://oldschoolrunescape.wikia.com/wiki/" + name;
            Console.WriteLine(url);
            json_data = w.DownloadString(url);


            //Console.WriteLine(json_data);

            if (json_data.Contains("title=\"High Level Alchemy\">"))
            {
                Console.WriteLine("There is a value");
                string high_alchvalue = String.Empty;
                string[] splitString = json_data.Split(new string[] { "title=\"High Level Alchemy\">High Alch</a>\n</th><td> " }, StringSplitOptions.None);
                string[] theValue = splitString[1].Split(new string[] { "&#160;" }, StringSplitOptions.None);
                string value= theValue[0];
                value.Trim();
                Console.WriteLine(value);
                value = value.Replace(",", "");
                if (value.Contains("Unknown") || value.Contains("doses"))
                {
                    Console.WriteLine("Item can't be high alched");
                    return -1;
                }else if (value.Contains("Clean:"))
                {
                    value = value.Replace("Clean: ", "");
                    string[] temp = value.Split(new string[] { "coins" }, StringSplitOptions.None);
                    value = temp[0];
                    return Int32.Parse(value);
                }
                else
                {
                    
                    Console.WriteLine(value);
                    return Int32.Parse(value);
                }

    


            }
            return -1;
        }

        public static int OldHighAlch(string name)
        {
            var w = new WebClient();
            var json_data = string.Empty;
            string url = "http://oldschoolrunescape.wikia.com/wiki/Exchange:" + name;
            Console.WriteLine(url);
            json_data = w.DownloadString(url);


            Console.WriteLine(json_data);

            if (json_data.Contains("<b>High Alchemy:</b>"))
            {
                Console.WriteLine("There is a value");
                string high_alchvalue = String.Empty;
                string[] splitString = json_data.Split(new string[] { "<b>High Alchemy:</b>" }, StringSplitOptions.None);
                Console.WriteLine(splitString[1]);
                string whatWeWant = splitString[1];
                int index = whatWeWant.IndexOf("</li>");
                Console.WriteLine(index);

                string value = whatWeWant.Substring(0, index).Trim();

                if (value.Contains("Unknown"))
                {
                    Console.WriteLine("Item can't be high alched");
                    return -1;
                }
                else
                {
                    value = value.Replace(",", "");
                    Console.WriteLine(value);
                    return Int32.Parse(value);
                }




            }
            return -1;
        }

    }
}
