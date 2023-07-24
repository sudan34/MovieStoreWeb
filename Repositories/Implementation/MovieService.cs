﻿using MovieStoreWeb.Models.Domain;
using MovieStoreWeb.Models.DTO;
using MovieStoreWeb.Repositories.Abstract;

namespace MovieStoreWeb.Repositories.Implementation
{
    public class MovieService : IMovieService
    {
        public readonly DatabaseContext _context;

        public MovieService(DatabaseContext context)
        {
            _context = context;
        }

        public bool Add(Movie model)
        {
            try
            {
                _context.Movie.Add(model);
                _context.SaveChanges();
                foreach (int genreId in model.Genres)
                {
                    var movieGenre = new MovieGenre
                    {
                        GenreId = genreId,
                        MovieId = model.Id,
                    };
                    _context.MoviesGenre.Add(movieGenre);
                }
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var data = this.GetById(id);
                if (data == null)
                {
                    return false;
                }
                var movieGeners = _context.MoviesGenre.Where(a => a.MovieId == data.Id);
                foreach (var movieGenere in movieGeners)
                {
                    _context.MoviesGenre.Remove(movieGenere);
                }
                _context.Movie.Remove(data);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public Movie GetById(int id)
        {
            return _context.Movie.Find(id);
        }

        public MovieListVm List()
        {
            var list = _context.Movie.AsQueryable();
            var data = new MovieListVm
            {
                MovieList = list
            };
            return data;
        }

        public bool Update(Movie model)
        {
            try
            {
                // these genreIds are not selected by users and still present is movieGenre table corresponding to
                // this movieId. So these ids should be removed.
                var genresToDeleted = _context.MoviesGenre.Where(a => a.MovieId == model.Id && !model.Genres.Contains(a.GenreId)).ToList();
                foreach (var mGenre in genresToDeleted)
                {
                    _context.MoviesGenre.Remove(mGenre);
                }
                foreach (int genId in model.Genres)
                {
                    var movieGenre = _context.MoviesGenre.FirstOrDefault(a => a.MovieId == model.Id && a.GenreId == genId);
                    if (movieGenre == null)
                    {
                        movieGenre = new MovieGenre { GenreId = genId, MovieId = model.Id };
                        _context.MoviesGenre.Add(movieGenre);
                    }
                }

                _context.Movie.Update(model);
                // we have to add these genre ids in movieGenre table
                _context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}