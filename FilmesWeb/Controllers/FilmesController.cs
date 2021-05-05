using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entidades.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Negocio;

namespace FilmesWeb.Controllers
{
    public class FilmesController : Controller
    {
        public readonly AdmFacade _negocio;

        public readonly UserManager<ApplicationUser> _userManager;

        private IWebHostEnvironment _environment;

        public FilmesController(AdmFacade negocio, 
                                UserManager<ApplicationUser> userManager, 
                                IWebHostEnvironment environment)
        {
            _negocio = negocio;
            _userManager = userManager;
            _environment = environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult roteiroAutenticacao()
        {
            return View();
        }




        private async Task<IActionResult> usuario()
        {
            var usuario = await _userManager.GetUserAsync(User);

            ViewBag.Id = usuario.Id;
            ViewBag.Endereco = usuario.Endereco;
            
            return View();

        }
    }
}