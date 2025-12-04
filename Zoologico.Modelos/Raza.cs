using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoologico.Modelos
{
    public class Raza
    {
        [Key]
        public int Id { get; set; }
        public string NombreRaza     { get; set;   }

        //navegacion
        public List <Animal>? Animales { get; set; } //sirve: para navegar a la tabla animale



    }
}
