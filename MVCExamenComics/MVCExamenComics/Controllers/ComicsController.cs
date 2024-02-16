using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCExamenComics.Models;
using MVCExamenComics.Repositories;
using System.Numerics;

namespace MVCExamenComics.Controllers
{
    public class ComicsController : Controller
    {
        private IRepositoryComics repoComic;

        public ComicsController(IRepositoryComics repo)
        {
            repoComic = repo;
        }

        public IActionResult Index()
        {
            List<Comic> comics = this.repoComic.Getcomics();
            return View(comics);
        }

        
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateProcedure(Comic comic)
        {
            this.repoComic.InsertComicProcedure(comic.Nombre, comic.Imagen, comic.Descripcion);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CreateLambda(Comic comic)
        {
            this.repoComic.InsertComicLambda(comic.Nombre, comic.Imagen, comic.Descripcion);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int idComic)
        {
            Comic comic = this.repoComic.FindComic(idComic);
            return View(comic);
        }

        [HttpPost]
        public IActionResult DeletePost(int idComic)
        {
            this.repoComic.DeleteComic(idComic);
            return RedirectToAction("Index");
        }

        public IActionResult Detalles()
        {
            ViewData["NOMBRES"] = this.repoComic.GetComicsNombre();
            return View();
        }

        [HttpPost]
        public IActionResult Detalles(string nombre)
        {
            ViewData["NOMBRES"] = this.repoComic.GetComicsNombre();
            Comic comic = this.repoComic.FindComicDetalle(nombre);
            return View(comic);
        }

    }
}
