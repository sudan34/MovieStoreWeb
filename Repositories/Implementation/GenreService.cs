using MovieStoreWeb.Models.Domain;
using MovieStoreWeb.Repositories.Abstract;

namespace MovieStoreWeb.Repositories.Implementation
{
    public class GenreService : IGenreService
    {
        private readonly DatabaseContext _Context;

        public GenreService(DatabaseContext context)
        {
            _Context = context;
        }

        public bool Add(Genre model)
        {
            try
            {
                _Context.Genre.Add(model);
                _Context.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public Genre GetById(int id)
        {
            return _Context.Genre.Find(id);
        }

        public IQueryable<Genre> List()
        {
            var data = _Context.Genre.AsQueryable();
            return data;
        }

        public bool Update(Genre model)
        {
            try
            {
                _Context.Genre.Update(model);
                _Context.SaveChanges();
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
                    return false;

                _Context.Genre.Remove(data);
                _Context.SaveChanges() ;
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}
