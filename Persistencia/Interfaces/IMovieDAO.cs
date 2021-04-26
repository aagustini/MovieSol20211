using Persistencia.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistencia.Interfaces
{
    public interface IMovieDAO
    { 
         List<RelFilmes> consolidadoFilmes();

    }
}
