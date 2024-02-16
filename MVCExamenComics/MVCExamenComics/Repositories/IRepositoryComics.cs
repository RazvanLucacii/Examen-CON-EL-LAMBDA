using MVCExamenComics.Models;

namespace MVCExamenComics.Repositories
{
    public interface IRepositoryComics
    {
        List<Comic> Getcomics();

        void InsertComicProcedure(string nombre, string imagen, string descripcion);

        void InsertComicLambda(string nombre, string imagen, string descripcion);

        void DeleteComic(int idComic);

        Comic FindComic(int idComic);

        List<string> GetComicsNombre();

        Comic FindComicDetalle(string nombre);

    }
}
