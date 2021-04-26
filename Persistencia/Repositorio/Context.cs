using Microsoft.EntityFrameworkCore;
using Persistencia.Entidades;
using System;
using System.Collections.Generic;
using System.Text;

namespace Persistencia.Repositorio
{
    public class MovieContext : DbContext
    {
        public MovieContext() : base()
        {
        }

        public MovieContext(DbContextOptions<MovieContext> options)
    : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<Genre> Genres { get; set; }
        
        public DbSet<Actor> Actors { get; set; }
        public DbSet<ActorMovie> Characters { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=Filmes;Trusted_Connection=True;");
        }
    }

}


