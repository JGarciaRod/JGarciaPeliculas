using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Dulceria
    {
        public int? IdDulce { get; set; }
        public string? Nombre { get; set; }
        public float? Precio { get; set; }
        public string? Imagen { get; set; }

        public int? Cantidad { get; set; }

        public List<object>? Dulces { get; set; }
    }
}
