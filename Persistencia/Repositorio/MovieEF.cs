using Microsoft.EntityFrameworkCore;
using Persistencia.Interfaces;
using Persistencia.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

using System.Linq;



namespace Persistencia.Repositorio
{
    public class MovieEF : IMovieDAO
    {
        private readonly MovieContext _context;

        public MovieEF()
        {
            _context = new MovieContext();
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
