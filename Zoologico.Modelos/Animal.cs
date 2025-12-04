using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zoologico.Modelos
{
    public class Animal
    {
        [Key] 
            public int Id { get; set; }
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public string Genero { get; set; }

        //Fk

        public int EspecieCodigo { get; set; } //sirve: para relacionar con la tabla especie
        public int RazaId { get; set; } //sirve: para relacionar con la tabla raza

        //Navegacion 

        public Especie? Especie { get; set; } //sirve: para navegar a la tabla especie
        public Raza? Raza { get; set; } //sirve: para navegar a la tabla raza


    }
}
