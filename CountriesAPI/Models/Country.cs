using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CountriesAPI.Models
{
    public class Country
    {
        public string name { get; set; }

        public string capital { get; set; }

        public string region { get; set; }

        public string subregion { get; set; }

        public double? area { get; set; }

        public string flag { get; set; }

        public string alpha3code { get; set; }

        public int? population { get; set; }

        public double? gini { get; set; }

        public override string ToString()
        {
            return $"{name}";
        }
    }
}
