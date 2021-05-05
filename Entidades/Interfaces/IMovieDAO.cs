
using Entidades.Model;
using Entidades.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entidades.Interfaces
{
    public interface IMovieDAO
    {
         List<Movie> todos();
         List<RelFilmes> consolidadoFilmes();

    }
}
