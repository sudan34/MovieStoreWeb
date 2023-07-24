using Microsoft.AspNetCore.Mvc;
using MovieStoreWeb.Models.Domain;
using MovieStoreWeb.Repositories.Abstract;

namespace MovieStoreWeb.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IFileServices _fileService;
        private readonly IGenreService _genService;

        public MovieController(IMovieService movieService, IFileServices fileService, IGenreService genService)
        {
            _movieService = movieService;
            _fileService = fileService;
            _genService = genService;
        }

        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Add(Movie model)
        {
            return View();
        }
    }
}
