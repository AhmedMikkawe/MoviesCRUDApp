using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesCRUDApp.Models;
using MoviesCRUDApp.ViewModels;
using NToastNotify;

namespace MoviesCRUDApp.Controllers
{
    public class MoviesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IToastNotification _toastNotification;
        public MoviesController(ApplicationDbContext context, IToastNotification toastNotification)
        {
            _context = context;
            _toastNotification = toastNotification;
        }
        public async Task<IActionResult> Index()
        {
            var movies = await _context.Movies.ToListAsync();
            return View(movies);
        }
        public async Task<IActionResult> Create()
        {
            MovieFormViewModel viewModel = new MovieFormViewModel { Genres = await _context.Genres.OrderBy(m=>m.Name).ToListAsync() };
            return View("MovieForm",viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();
                return View("MovieForm",model);
            }
            var files = Request.Form.Files;
            if (!files.Any())
            {
                model.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();
                ModelState.AddModelError("Poster", "Please Select Movie Poster");
                return View("MovieForm", model);
            }
            var poster = files.FirstOrDefault();
            var allowedExtensions = new List<string> { ".jpg", ".png" };
            if (!allowedExtensions.Contains(Path.GetExtension(poster.FileName).ToLower()))
            {
                model.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();
                ModelState.AddModelError("Poster", "Only .PNG, .JPG images are allowed");
                return View("MovieForm", model);

            }
            if(poster.Length > 1048576)
            {
                model.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();
                ModelState.AddModelError("Poster", "Poster Can't be more than 1 MB");
                return View("MovieForm", model);

            }
            using var dataStream = new MemoryStream();
            await poster.CopyToAsync(dataStream);
            var movies = new Movie { 
                Title = model.Title,
                GenreId = model.GenreId,
                Year = model.Year,
                Rate = model.Rate,
                StoryLine = model.StoryLine,
                Poster = dataStream.ToArray()
            };
            _context.Movies.Add(movies);
            _context.SaveChanges();
            _toastNotification.AddSuccessToastMessage("Movie Created Successfully");
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var movie = await _context.Movies.FindAsync(id);
            if(movie == null)
            {
                return NotFound();
            }

            MovieFormViewModel viewModel = new MovieFormViewModel {
                Title = movie.Title,
                MovieId = movie.MovieId,
                GenreId = movie.GenreId,
                Rate = movie.Rate,
                Year = movie.Year,
                StoryLine = movie.StoryLine,
                Poster = movie.Poster,
                Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync() };
            return View("MovieForm", viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MovieFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();
                return View("MovieForm", model);
            }
            var movie = await _context.Movies.FindAsync(model.MovieId);
            if (movie == null)
            {
                return NotFound();
            }


            var files = Request.Form.Files;
            if (files.Any())
            {
                var poster = files.FirstOrDefault();
                using var dataStream = new MemoryStream();
                await poster.CopyToAsync(dataStream);
                model.Poster = dataStream.ToArray();
                var allowedExtensions = new List<string> { ".jpg", ".png" };

                if (!allowedExtensions.Contains(Path.GetExtension(poster.FileName).ToLower()))
                {
                    model.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();
                    ModelState.AddModelError("Poster", "Only .PNG, .JPG images are allowed");
                    return View("MovieForm", model);

                }
                if (poster.Length > 1048576)
                {
                    model.Genres = await _context.Genres.OrderBy(m => m.Name).ToListAsync();
                    ModelState.AddModelError("Poster", "Poster Can't be more than 1 MB");
                    return View("MovieForm", model);

                }

                movie.Poster = model.Poster;

            }


            movie.Title = model.Title;
            movie.GenreId = model.GenreId;
            movie.Year = model.Year;
            movie.Rate = model.Rate;
            movie.StoryLine = model.StoryLine;

            _context.SaveChanges();
            _toastNotification.AddSuccessToastMessage("Movie Edited Successfully");

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return BadRequest();
            }
            var movie = await _context.Movies.Include(m=>m.Genre).SingleOrDefaultAsync(m=>m.MovieId == id);
            if(movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }
    }
}
