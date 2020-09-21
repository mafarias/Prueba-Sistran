using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Dominio;

namespace PruebaBrinks.Models
{
    public class ModelOrden 
    {
        public int Id { get; set; }

        public string Cliente { get; set; }

        public string Oficina { get; set; }

        public DateTime FechaIngreso { get; set; }

        public string producto { get; set; }


    }
}