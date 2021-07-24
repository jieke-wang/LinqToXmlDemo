using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LinqToXmlDemo
{
    public class Test
    {
        public int ID { get; set; }

        [LinqToXML.DefaultValue("Test", typeof(string))]
        public string Name { get; set; }

        [LinqToXML.DefaultValue(20, typeof(int))]
        public int Age { get; set; }

        [LinqToXML.DefaultValue("无业", typeof(string))]
        public string Job;
    }
}