
using Persistencia.Entidades;
using Persistencia.Repositorio;
using System.Linq;

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;
using System.Threading;

namespace Persistencia
{
    class Program
    {
        static void Main(string[] args)
        {
            //MovieContext _context = new MovieContext();

            #region LINQ - exercicio para entrega
            Console.WriteLine("\n1. Listar o nome de todos personagens desempenhados por um determinado ator, incluindo a informação de qual o filme");
            using (MovieContext _context = new MovieContext())
            {
                String ator = "Harrison Ford";
                var ex1 = from am in _context.Characters
                          where am.Actor.Name == ator
                          select new { am.Movie.Title, am.Character };

                Console.WriteLine("Ator: " + ator);
                foreach (var elem in ex1)
                {
                    Console.WriteLine("\t {0} - {1}", elem.Character, elem.Title);
                }

            }

            Console.WriteLine("\n2. Mostrar o nome de todos atores que desempenharam um determinado personagem(por exemplo, quais os atores que já atuaram como '007' ?)");

            using (MovieContext _context = new MovieContext())
            {
                String personagem = "James Bond";
                var ex2 = from am in _context.Characters
                          where am.Character == personagem
                          select new { am.Movie.Title, am.Actor.Name };

                Console.WriteLine("Personagem: " + personagem);
                foreach (var elem in ex2)
                {
                    Console.WriteLine("\t {0} - {1}", elem.Name, elem.Title);
                }
                
               
                Console.WriteLine("\n3. Informar qual o ator desempenhou mais vezes um determinado personagem(por exemplo: qual o ator que realizou mais filmes como o 'agente 007') ");
                var ex3 = from e in ex2
                          group e by e.Name into grp
                          select new { Ator = grp.Key, Nro = grp.Count() };

                Console.WriteLine("Personagem: " + personagem);
                foreach (var elem in ex3.OrderByDescending(e=>e.Nro))
                {
                    Console.WriteLine("\t {0} - {1} vezes", elem.Ator, elem.Nro);
                }
            }
            Console.WriteLine("\n4. Mostrar o nome e a data de nascimento do ator mais idoso e o mais novo");
            using (MovieContext _context = new MovieContext())
            {
                var ex4_jovem = (from a in _context.Actors
                                 select a.DateBirth).Max();

                var ex4_idoso = _context.Actors.Min(a => a.DateBirth);

                var ex4a = (from a in _context.Actors
                            where a.DateBirth == ex4_idoso
                            select new { a.Name });

                Console.WriteLine("Artor(es) mais idoso nasceu em: " + ex4_idoso.ToShortDateString());
                foreach (var elem in ex4a)
                {
                    Console.WriteLine("\t {0}", elem);
                }

                var ex4b = (from a in _context.Actors
                           let jovem = _context.Actors.Max(a=>a.DateBirth)
                            where a.DateBirth == jovem
                            select new { a.Name, a.DateBirth}).FirstOrDefault();

                //Console.WriteLine("\nArtor(es) mais idoso nasceu em: " + ex4_jovem.ToShortDateString());
                //foreach (var elem in ex4b)
                //{
                Console.WriteLine("\tMais idoso:  {0} - {1}", ex4b.Name, ex4b.DateBirth.ToShortDateString()) ;
                //}
            }

            Console.WriteLine("\n5. Mostrar o nome e a data de nascimento do ator mais idoso e o mais novo de um determinado gênero");
            using (MovieContext _context = new MovieContext())
            {
                String genero = "Comedy";
                var ex5_jovem = (from am in _context.Characters
                                                   .Include(a=>a.Movie)
                                                   .ThenInclude(m=>m.Genre)
                                                   .Include(c => c.Actor)
                                 where am.Movie.Genre.Name == genero
                                 select am.Actor.DateBirth).Max();

               
                var ex5a = (from a in _context.Actors
                            where a.DateBirth == ex5_jovem
                            select new { a.Name });

                Console.WriteLine("\tGenero: {0} Artor(es) mais jovem nasceu em: {1} ", genero, ex5_jovem.ToShortDateString());
                foreach (var elem in ex5a)
                {
                    Console.WriteLine("\t {0}", elem);
                }
            }


            Console.WriteLine("\n6. Mostrar o valor médio das avaliações dos filmes que um determinado ator participou");
            using (MovieContext _context = new MovieContext())
            {
                String ator = "Harrison Ford";
                var ex6 = (from am in _context.Characters
                                     .Include(a => a.Movie)
                                     .Include(c => c.Actor)
                           where am.Actor.Name == ator
                           select am.Movie.Rating).Average();

                Console.WriteLine("\t Avaliacao media dos filmes do {0} : {1:.2}", ator, ex6);
            }
            
            Console.WriteLine("\n7. Qual o elenco do filme melhor avaliado ?");
            using (MovieContext _context = new MovieContext())
            {

                var piorAvaliado = _context.Movies.Min(m => m.Rating);

                var ex7 = from am in _context.Characters
                                     .Include(a => a.Movie)
                                     .Include(c => c.Actor)
                          where am.Movie.Rating == piorAvaliado
                          select  new { am.Movie.Title, am.Character, am.Actor.Name};

                Console.WriteLine("Melhor avaliacao: " + piorAvaliado);
            

                foreach (var elem in ex7)
                {
                    Console.WriteLine("\t {0} - {1} - {2}", elem.Title, elem.Character, elem.Name);
                   
                }

            }

            Console.WriteLine("\n8. Qual o elenco do filme com o Menor faturamento?");
           
            using (MovieContext _context = new MovieContext())
            {
                //var ex8a = _context.Movies.Min(f => f.Gross);
                var ex8a = (from m in _context.Movies
                            select m.Gross).Min();

                Console.WriteLine("Menor faturamento: " + ex8a);

                var ex8b = (from m in _context.Movies
                                    .Include(m => m.Characters)
                                    .ThenInclude(c => c.Actor)
                            where m.Gross == ex8a
                            select m).FirstOrDefault();

                var ex8c = from cast in ex8b.Characters
                           select new { cast.Character, cast.Actor.Name };
                //Console.WriteLine("Personagem: " + ex8c);
                foreach (var pers in ex8c)
                {
                    Console.WriteLine("\t " + pers.Character + " -> " + pers.Name);
                }


            }

            Console.WriteLine("9. (Ismael) os 5 melhores filmes da história  com media, genero e todos os atores");
            using (MovieContext _context = new MovieContext())
            {
                var ex9 = (from m in _context.Movies
                                           .Include(g => g.Genre)
                                           .Include(q => q.Characters)
                                           .ThenInclude(p => p.Actor)
                            orderby m.Rating descending
                            select m ).Take(5);



                foreach (var m in ex9)
                {
                    Console.WriteLine("Title = {0} \t Director = {1} \t Rating: {2}",
                        m.Title, m.Rating, m.Director);

                }
            }
                #endregion

                #region # LINQ - consultas casting (aula)

                // elenco de um filme qualquer
                //var cast1 = _context.Characters                     
                //                   .Where(c => c.Movie.Title == "Skyfall")
                //                   .Select(c => new
                //                   {
                //                       c.Character,
                //                       c.Actor.Name
                //                   });
                //Console.WriteLine("Elenco do filme Skyfall:");
                //foreach (var elem in cast1)
                //{
                //    Console.WriteLine("\t{0:20} - {1}", elem.Character, elem.Name);
                //}
                //Console.WriteLine("- - - - - - - - - - -");

                //var cast2 = _context.Characters
                //                   .Where(c => c.Actor.Name == "Pierce Brosnan")
                //                   .Select(c => new
                //                   {
                //                       c.Movie.Title,
                //                       c.Movie.Genre.Name
                //                   });

                //Console.WriteLine("\nFilme do Pierce Brosnan com genero:");
                //foreach (var elem in cast2)
                //{
                //    Console.WriteLine("\t{0:-15} - {1}", elem.Title, elem.Name);
                //}

                //var cast3 = (from c in _context.Characters
                //             where c.Actor.Name == "Pierce Brosnan"
                //             select c).Count();


                //Console.WriteLine("\nPierce Brosnan autuou em " + cast3 + " filmes ");

                //var cast4 = from m in _context.Movies

                //            group m by m.Genre.Name into grp
                //           select new
                //         {
                //             Genero = grp.Key,
                //             AvaliacaoMedia = grp.Average(m => m.Rating)
                //         };


                //Console.WriteLine("\nAvaliacao media dos filmes por genero ");
                //foreach (var gen in cast4)
                //{
                //    Console.WriteLine("Genero = {0} \t avalicao media = {1}",
                //        gen.Genero, gen.AvaliacaoMedia);

                //}

                //var cast5 = _context.Characters.Where(c => c.Actor.Name.Contains("Pierce"))
                //            .Include(c => c.Movie)
                //            .ThenInclude(g => g.Genre)
                //            .Select(c => new
                //            {
                //                c.Movie.Title,
                //                c.Movie.Genre.Name

                //            }); ;



                //Console.WriteLine("\nFilme do Pierce Brosnan com genero:");
                //foreach (var elem in cast5)
                //{
                //    Console.WriteLine("\t{0:-15} - {1}", elem.Title, elem.Name);
                //}

                //var cast6 = from e in cast5
                //            group e by e.Name into grp                       
                //            select new
                //            {
                //                Genero = grp.Key,
                //                Qtde = grp.Count()
                //            };

                //Console.WriteLine("\nQtde Filmes do Pierce Brosnan por genero:");
                //foreach (var elem in cast6)
                //{
                //    Console.WriteLine("\t{0:-15} - {1}", elem.Genero, elem.Qtde);
                //}

                // data de nascimento do ator mais novo
                //var cast7 = (from a in _context.Actors
                //             select a.DateBirth).Max();

                //var cast7b = (from a in _context.Actors
                //                 where a.DateBirth == cast7
                //                 select a.Name).FirstOrDefault();

                //Console.WriteLine("\n Atorm mais novo: {0} DN: {1}", cast7b, cast7.ToShortDateString());


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

