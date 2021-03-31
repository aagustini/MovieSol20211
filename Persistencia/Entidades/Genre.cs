using System;
using System.Collections.Generic;
using System.Text;

namespace Persistencia.Entidades
{
     public class Genre
    {
        public int GenreId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Movie> Movies { get; set; }

    }
}
