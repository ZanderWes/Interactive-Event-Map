using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFEventMap
{
    public class Person
    {
        public string Name { get; set; }
        public string Relation { get; set; }

        public Person()
        {

        }

        public Person(string name, string relationship)
        {
            Name = name;
            Relation = relationship;
        }
    }
}
