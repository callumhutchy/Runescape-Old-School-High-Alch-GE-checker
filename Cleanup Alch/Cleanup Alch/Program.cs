using System;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Text;

namespace Cleanup_Alch
{
    class Program
    {
        static string file = "output.json";

        static void Main(string[] args)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("[");

            int eliminated = 0;
            int kept = 0;

            string json = File.ReadAllText(file);
            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            while (reader.Read())
            {
                if(reader.TokenType.ToString() == "StartObject")
                {
                    reader.Read();
                    if((string) reader.Value == "id")
                    {
                        reader.Read();
                        Console.Write(reader.Value);

                        int id = int.Parse(reader.Value.ToString());

                        reader.Read();

                        if((string) reader.Value == "name")
                        {
                            reader.Read();
                            string name = (string)reader.Value;

                            reader.Read();
                            if((string) reader.Value == "alch_value")
                            {
                                reader.Read();
                                int alch = int.Parse(reader.Value.ToString());
                                Console.WriteLine(alch);

                                if(alch > 2000)
                                {
                                    sb.Append("{");
                                    sb.Append("\"id\": " + id + ",");
                                    sb.Append("\"name\": \"" + name + "\",");
                                    sb.Append("\"alch_value\": " + alch);

                                    sb.Append("},");
                                    kept++;
                                }
                                else
                                {
                                    eliminated++;
                                }

                            }

                        }
                    }
                }
            }


            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");

            using (StreamWriter swriter = new StreamWriter("newoutput.json"))
            {
                swriter.Write(sb.ToString());
            }

            Console.WriteLine("New file produced");
            Console.WriteLine("Number of items elimnated = " + eliminated);
            Console.WriteLine("Number of items kept = " + kept);

            Console.ReadLine();

        }
    }
}
