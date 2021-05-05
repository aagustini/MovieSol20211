using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;
using Entidades.ViewModels;
using Entidades.Interfaces;
using Entidades.Model;

namespace Persistencia.Repositorio
{
    public class MovieEF : IMovieDAO
    {
        private readonly MovieContext _context;

        public MovieEF()
        {
            _context = new MovieContext();
        }

        public List<Movie> todos()
        {
            return _context.Movies.Include("Genre").ToList();
        }

        public List<RelFilmes> consolidadoFilmes()
        {
            var q6 = from f in _context.Movies.Include("Genre")
                     group f by f.Genre.Name into grpGen
                     select new RelFilmes
                     {
                         Categoria = grpGen.Key,
                         Faturamento = grpGen.Sum(e => e.Gross),
                         Avaliacao = grpGen.Average(e => e.Rating),
                         Quantidade = grpGen.Count()
                     };

            return q6.ToList();

        }

     

    }
}
