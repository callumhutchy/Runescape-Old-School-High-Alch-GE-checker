using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    public class RootObject
    {
        public int id { get; set; }
        public string name { get; set; }
        public int alch_value { get; set; }

        public RootObject(int id, string name, int alch)
        {
            this.id = id;
            this.name = name;
            alch_value = alch;
        }


    }
}
