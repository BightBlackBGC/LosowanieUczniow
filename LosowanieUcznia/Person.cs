using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LosowanieUcznia
{
    internal class Person
    {

            public string Name { get; set; }
            public string Surname { get; set; }
            public string Number { get; set; }
            public string Class { get; set; }
            public int ExclusionTurns { get; set; }

        public Person()
        {
            ExclusionTurns = 0;
        }
    }
}
