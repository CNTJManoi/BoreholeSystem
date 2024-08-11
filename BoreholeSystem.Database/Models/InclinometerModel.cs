using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoreholeSystem.Database.Models
{
    public class InclinometerModel
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Temp { get; set; }
    }
}
