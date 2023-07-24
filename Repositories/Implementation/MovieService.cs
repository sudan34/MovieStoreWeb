﻿using MovieStoreWeb.Models.Domain;
using MovieStoreWeb.Models.DTO;
using MovieStoreWeb.Repositories.Abstract;
using System.Collections;
using System.Net.NetworkInformation;

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

        public List<int> GetGenreByMovieId(int movieId)
        {
            var genreIds = _context.MoviesGenre.Where(a => a.MovieId == movieId).Select(a => a.GenreId).ToList();
            return genreIds;
        }

        public MovieListVm List(string term = "", bool paging = false, int currentPage = 0)
        {
            var data = new MovieListVm();
            var list = _context.Movie.ToList();
            if (!string.IsNullOrEmpty(term))
            {
                term = term.ToLower();
                list = list.Where(a => a.Title.ToLower().StartsWith(term)).ToList();
            }
            if (paging)
            {
                // here we will apply paging
                int pageSize = 5;
                int count = list.Count;
                int TotalPages = (int)Math.Ceiling(count / (double)pageSize);
                list = list.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
                data.PageSize = pageSize;
                data.CurrentPage = currentPage;
                data.TotalPages = TotalPages;
            }
            foreach (var movie in list)
            {
                var genres = (from genre in _context.Genre
                              join mg in _context.MoviesGenre
                              on genre.Id equals mg.GenreId
                              where mg.MovieId == movie.Id
                              select genre.GenreName
                                   ).ToList();
                var genreNames = string.Join(',', genres);
                movie.GenreNames = genreNames;
            }
            data.MovieList = list.AsQueryable();
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
