using MovieStoreWeb.Models.Domain;

namespace MovieStoreWeb.Repositories.Abstract
{
    public interface IGenreService
    {
        bool Add(Genre model);
        bool Update(Genre model);
        Genre GetById(int id);
        IQueryable<Genre> List();
        bool Delete(int id);

    }
}
