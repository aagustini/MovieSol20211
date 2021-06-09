using Entidades.Interfaces;
using Entidades.Model;
using Entidades.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Negocio
{
    public class AdmFacade
    {
        private readonly IMovieDAO _daoFilmes;

        //public AdmFacade()
        //{
        //    _dao = new MovieEF();
        //}

        public AdmFacade(IMovieDAO _movieDAO)
        {
            _daoFilmes = _movieDAO;
        }

        public List<Movie> TodosFilmes()
        {
            return _daoFilmes.todos();
        }
        public List<RelFilmes> relatorioFilmes()
        {
            return _daoFilmes.consolidadoFilmes();
        }

        public Movie getMovie(int id)
        {
            return _daoFilmes.getMovie(id);
        }

        public void addReview(Review rev)
        {
            // para ganhar tempo, nao deveria estar no daoFilmes
             _daoFilmes.addReview(rev);
        }
    }
}
