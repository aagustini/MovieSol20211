using System;
using System.Collections.Generic;
using System.Text;

namespace Persistencia.Entidades
{
    public class ActorMovie

    {

        public int ActorMovieId { get; set; }
        public string Character { get; set; }

        public int MovieId { get; set; }
        public virtual Movie Movie { get; set; }

        public int ActorId { get; set; }
        public virtual Actor Actor { get; set; }

    }
}
