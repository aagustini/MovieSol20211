using Entidades.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;



namespace Persistencia.Repositorio
{
    // contexto vai gerenciar o Identity
    //public class MovieContext : DbContext
    public class MovieContext : IdentityDbContext<ApplicationUser>
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

        // bd de autenticacao
        public DbSet<ApplicationUser> ApplicationUser { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                  .UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Filmes;Trusted_Connection=True");
                base.OnConfiguring(optionsBuilder);
            }
        }


        // associar a PK do Identity
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>().ToTable("AspNetUsers").HasKey(t => t.Id);

            base.OnModelCreating(builder);
        }
    }
}


