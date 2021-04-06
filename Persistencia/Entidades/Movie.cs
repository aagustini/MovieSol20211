using System;
using System.Collections.Generic;
using System.Text;

namespace Persistencia.Entidades
{
    public class Movie
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Director { get; set; }
        public DateTime ReleaseDate { get; set; }
        public decimal Gross { get; set; }
    
        public double Rating { get; set; }

        public int GenreID { get; set; }
        public virtual Genre Genre { get; set; }

        public virtual ICollection<ActorMovie> Characters { get; set; }
    }

}
