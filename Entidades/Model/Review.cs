using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Model
{
    public class Review
    {
        public int ReviewId { get; set; }
        public String User { get; set; }
        public String Coment { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}
