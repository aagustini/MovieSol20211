using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Negocio;
using Persistencia.ViewModels;

namespace MovieWeb.Controllers
{
    public class AdminController : Controller
    {
        private readonly AdmFacade _adm;

        //public AdminController ()
        //{
        //    _adm = new AdmFacade();
        //}

        public AdminController(AdmFacade _admFacade)
        {
            _adm = _admFacade;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult relFilmes()
        {
            List<RelFilmes> consolidado = _adm.relatorioFilmes();

            return View(consolidado);

        }

        public string Welcome()
        {
            return "This is the Welcome action method...";
        }

        // GET: /HelloWorld/Welcome/ 
        // Requires using System.Text.Encodings.Web;
        public string Welcome1(string name, int numTimes = 1)
        {
            return HtmlEncoder.Default.Encode($"Hello {name}, NumTimes is: {numTimes}");
        }

        public string Welcome2(string name, int ID = 1)
        {
            return HtmlEncoder.Default.Encode($"Hello {name}, ID: {ID}");
        }

        public IActionResult Welcome3(string name)
        {
            ViewData["Nome"] = name;
            return View();
        }

        public IActionResult Welcome4(string name, int? numTimes = 1)
        {
            ViewData["Message"] = "Hello " + name;
            ViewData["NumTimes"] = numTimes;

            return View();
        }


        public IActionResult Welcome5(string name, int? numTimes = 1)
        {
            ViewData["Message"] = "Hello " + name;
            ViewData["NumTimes"] = numTimes;

            return View();
        }

    }
}