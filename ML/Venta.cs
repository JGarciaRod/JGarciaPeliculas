﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Venta
    {
        public Venta() { }

        public decimal Total { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal { get; set; }
        public List<object> Carrito { get; set; }
    }
}
