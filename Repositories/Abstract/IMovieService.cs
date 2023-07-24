using MovieStoreWeb.Models.Domain;
using MovieStoreWeb.Models.DTO;

namespace MovieStoreWeb.Repositories.Abstract
{
    public interface IMovieService
    {
        bool Add(Movie model);
        bool Update(Movie model);
        Movie GetById(int id);
        bool Delete(int id);
        MovieListVm List();

    }
}
