using Persistencia.Interfaces;
using Persistencia.Repositorio;
using Persistencia.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Negocio
{
    public class AdmFacade
    {
        private readonly IMovieDAO _dao;

        //public AdmFacade()
        //{
        //    _dao = new MovieEF();
        //}

        public AdmFacade(IMovieDAO _movieDAO)
        {
            _dao = _movieDAO;
        }

        public List<RelFilmes> relatorioFilmes()
        {
            return _dao.consolidadoFilmes();
        }
       
    }
}
