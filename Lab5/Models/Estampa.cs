using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Lab5.Models
{
    public class Estampa
    {
        [Display(Name = "Nombre")]
        public string nombre { get; set; }
        [Display(Name = "Cantidad")]
        public int cantidad { get; set; }
        [Display(Name = "Disponibilidad")]
        public string dispo { get; set; }
    }
}