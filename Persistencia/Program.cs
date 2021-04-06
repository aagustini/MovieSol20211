
using Persistencia.Entidades;
using Persistencia.Repositorio;
using System.Linq;

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace Persistencia
{
    class Program
    {
        static void Main(string[] args)
        {
            MovieContext _context = new MovieContext();


            #region # LINQ - consultas casting

            // elenco de um filme qualquer
            var cast1 = _context.Characters                     
                               .Where(c => c.Movie.Title == "Skyfall")
                               .Select(c => new
                               {
                                   c.Character,
                                   c.Actor.Name
                               });
            Console.WriteLine("Elenco do filme Skyfall:");
            foreach (var elem in cast1)
            {
                Console.WriteLine("\t{0:20} - {1}", elem.Character, elem.Name);
            }
            Console.WriteLine("- - - - - - - - - - -");

            var cast2 = _context.Characters
                               .Where(c => c.Actor.Name == "Pierce Brosnan")
                               .Select(c => new
                               {
                                   c.Movie.Title,
                                   c.Movie.Genre.Name
                               });

            Console.WriteLine("\nFilme do Pierce Brosnan com genero:");
            foreach (var elem in cast2)
            {
                Console.WriteLine("\t{0:-15} - {1}", elem.Title, elem.Name);
            }

            var cast3 = (from c in _context.Characters
                         where c.Actor.Name == "Pierce Brosnan"
                         select c).Count();


            Console.WriteLine("\nPierce Brosnan autuou em " + cast3 + " filmes ");

            var cast4 = from m in _context.Movies
                        
                        group m by m.Genre.Name into grp
                       select new
                     {
                         Genero = grp.Key,
                         AvaliacaoMedia = grp.Average(m => m.Rating)
                     };


            Console.WriteLine("\nAvaliacao media dos filmes por genero ");
            foreach (var gen in cast4)
            {
                Console.WriteLine("Genero = {0} \t avalicao media = {1}",
                    gen.Genero, gen.AvaliacaoMedia);
              
            }

            var cast5 = _context.Characters.Where(c => c.Actor.Name.Contains("Pierce"))
                        .Include(c => c.Movie)
                        .ThenInclude(g => g.Genre)
                        .Select(c => new
                        {
                            c.Movie.Title,
                            c.Movie.Genre.Name

                        }); ;
           


            Console.WriteLine("\nFilme do Pierce Brosnan com genero:");
            foreach (var elem in cast5)
            {
                Console.WriteLine("\t{0:-15} - {1}", elem.Title, elem.Name);
            }

            var cast6 = from e in cast5
                        group e by e.Name into grp                       
                        select new
                        {
                            Genero = grp.Key,
                            Qtde = grp.Count()
                        };

            Console.WriteLine("\nQtde Filmes do Pierce Brosnan por genero:");
            foreach (var elem in cast6)
            {
                Console.WriteLine("\t{0:-15} - {1}", elem.Genero, elem.Qtde);
            }

            // data de nascimento do ator mais novo
            var cast7 = (from a in _context.Actors
                         select a.DateBirth).Max();

            var cast7b = (from a in _context.Actors
                             where a.DateBirth == cast7
                             select a.Name).FirstOrDefault();

            Console.WriteLine("\n Atorm mais novo: {0} DN: {1}", cast7b, cast7.ToShortDateString());


            #endregion
            #region # LINQ - consultas aula 31/03
            // Console.WriteLine();
            //// Console.WriteLine("Todos os gêneros da base de dados:");
            //// foreach (Genre genero in _context.Genres)
            //// {
            ////     Console.WriteLine("{0} \t {1}", genero.GenreId, genero.Name);
            //// }

            // //listar todos os filmes de acao
            // Console.WriteLine();
            // Console.WriteLine("Todos os filmes do genero 'Action':");
            // var filmesAction = _context.Movies.Where(m => m.Genre.Name.Equals("Action"));

            // foreach (Movie filme in filmesAction)
            // {
            //     Console.WriteLine("\t{0}", filme.Title);
            // }

            // Console.WriteLine();
            // Console.WriteLine("Todos os filmes do genero 'Action':");
            // var filmesAction2 = from m in _context.Movies
            //                     where m.GenreID == 1
            //                     select m;
            // foreach (Movie filme in filmesAction2)
            // {
            //     Console.WriteLine("\t{0}", filme.Title);
            // }

            // Console.WriteLine();
            // Console.WriteLine("Todos os filmes de cada genero:");
            // var generosFilmes = from g in _context.Genres.Include(gen => gen.Movies)
            //                     select g;
            // //var generosFilmes2 = db.Genres.Include(gen => gen.Movies).ToList();

            // foreach (var gf in generosFilmes)
            // {
            //     Console.WriteLine("Filmes do genero: " + gf.Name);
            //     foreach (var f in gf.Movies)
            //     {
            //         Console.WriteLine("\t{0}", f.Title);
            //     }
            // }

            // Console.WriteLine("Nomes dos filmes do diretor Quentin Tarantino:");
            // var q0 = from f in _context.Movies
            //          where f.Director == "Quentin Tarantino"
            //          select f.Title;

            // foreach(String titulo in q0)
            // {
            //     Console.WriteLine("\t{0}", titulo);
            // }



            // Console.WriteLine();
            // Console.WriteLine("Nomes dos filmes do diretor Quentin Tarantino:");
            // var q1 = from f in _context.Movies
            //          where f.Director == "Quentin Tarantino"
            //          select new
            //          {
            //              Ano = f.ReleaseDate.Year,
            //              Titulo =  f.Title
            //          };

            // var q2 = _context.Movies.Where(f => f.Director == "Quentin Tarantino").Select(f => f.Title);

            // foreach (var item in q1)
            // {
            //     Console.WriteLine("{0} - {1}", item.Ano, item.Titulo);
            // }


            // Console.WriteLine();
            // Console.WriteLine("Nomes e data dos filmes do diretor Quentin Tarantino:");
            // var q3 = from f in _context.Movies
            //          where f.Director == "Quentin Tarantino"
            //          select new { f.Title, f.ReleaseDate };
            // foreach (var f in q3)
            // {
            //     Console.WriteLine("{0}\t {1}", f.ReleaseDate.ToShortDateString(), f.Title);
            // }

            // //Console.ReadLine();
            // Console.WriteLine();
            // Console.WriteLine("Todos os gêneros ordenados pelo nome:");
            // var q4 = _context.Genres.OrderByDescending(g => g.Name);
            // foreach (var genero in q4)
            // {
            //     Console.WriteLine("{0, 20}\t {1}", genero.Name, genero.Description.Substring(0, 30));
            // }
            // Console.WriteLine();
            // Console.WriteLine("Numero de filmes agrupados pelo anos de lançamento:");
            // var q5 = from f in _context.Movies
            //          group f by f.ReleaseDate.Year into grupo
            //          select new
            //          {
            //              Chave = grupo.Key,
            //              NroFilmes = grupo.Count()
            //          };

            // foreach (var ano in q5.OrderByDescending(g => g.NroFilmes))
            // {
            //     Console.WriteLine("Ano: {0}  Numero de filmes: {1}", 
            //                         ano.Chave,
            //                         ano.NroFilmes);

            // }
            // //Console.WriteLine("tecle algo para continuar");
            // //Console.ReadKey();

            // Console.WriteLine();
            // Console.WriteLine("Projeção do faturamento total, quantidade de filmes e avaliação média agrupadas por gênero:");
            // var q6 = from f in _context.Movies
            //          group f by f.Genre.Name into grpGen
            //          select new
            //          {
            //              Categoria = grpGen.Key,
            //              Faturamento = grpGen.Sum(e => e.Gross),
            //              Avaliacao = grpGen.Average(e => e.Rating),
            //              Quantidade = grpGen.Count()
            //          };

            // foreach (var genero in q6)
            // {
            //     Console.WriteLine("Genero: {0}", genero.Categoria);
            //     Console.WriteLine("\tFaturamento total: {0}\n\t Avaliação média: {1}\n\tNumero de filmes: {2}",
            //                     genero.Faturamento, genero.Avaliacao, genero.Quantidade);
            // }
            // Console.WriteLine("tecle algo para continuar");
            // Console.ReadKey();
            #endregion


            #region - minhas consulta

            #endregion
        }
        static void Main0(string[] args)
        {
            MovieContext _context = new MovieContext();

            Genre g1 = new Genre()
            {
                Name = "Comedia",
                Description = "Filmes de comedia"
            };

            Genre g2 = new Genre()
            {
                Name = "Ficcao",
                Description = "Filmes de ficcao"
            };

            _context.Genres.Add(g1);
            _context.Genres.Add(g2);

            Console.WriteLine("g1.genreId: {0}\n", g1.GenreId);

            _context.SaveChanges();
            
            Console.WriteLine("g1.genreId: {0}\n", g1.GenreId);

            Console.WriteLine("g1: {0}\n", g1.Name);
            List<Genre> genres = _context.Genres.ToList();

            foreach (Genre g in genres)
            {
                Console.WriteLine(String.Format("{0,2} {1,-10} {2}",
                                    g.GenreId, g.Name, g.Description));
            }

            genres[0].Description += "/modificado";

            _context.SaveChanges();

            Console.WriteLine(String.Format("{0,2} {1,-10} {2}",
                       genres[0].GenreId, genres[0].Name, genres[0].Description));

            Movie m1 = new Movie() {

                Title = "Back to the Future",
                Director = "Robert Zemeckis",
                ReleaseDate = new DateTime(1989, 01, 22),
                Gross = 210609762M,
                Rating = 8.5,
                GenreID = 1
        };
            Movie m2 = new Movie()
            {

                Title = "Back to the Future II",
                Director = "Robert Zemeckis",
                ReleaseDate = new DateTime(1989, 01, 22),
                Gross = 210609762M,
                Rating = 8.5,
                GenreID = 1
            };

            _context.Movies.Add(m1);
            _context.Movies.Add(m2);
        _context.SaveChanges();

         Console.WriteLine("m1 id: {0} genero: {1}\n", 
                                m1.MovieId,
                                m1.Genre.Name);
        
        Console.WriteLine(String.Format("Genero: {0} NroFilmes: {1} Filmes: {2}\n",
                      g1.GenreId, g1.Movies.Count, g1.Movies ));

            foreach (Movie m in g1.Movies)
            {
                Console.WriteLine(String.Format("Titulo: {0} Diretor: {1} \n",
                     m.Title, m.Director));
            }

        }

    }
}

