using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetGestionFilieres.Model
{
    public class Plotinfo
    {
        public string XValue { get; set; }
        public int YValue { get; set; }
        public double ZValue { get; set; }

        public Plotinfo(string x, int y, double z)
        {
            XValue = x;
            YValue = y;
            ZValue = z;
        }
    }
}
