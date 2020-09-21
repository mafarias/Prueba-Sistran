using System;
using System.Collections.Generic;
using System.Text;

namespace Dominio
{
    public class Orden
    {
        public int Id { get; set; }

        public string Cliente { get; set; }

        public string Oficina { get; set; }

        public DateTime FechaIngreso { get; set; }

        public string producto { get; set; }



        public Orden()
        {
        }
    }
}
