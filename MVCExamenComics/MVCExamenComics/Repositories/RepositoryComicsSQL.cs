using Microsoft.AspNetCore.Http.HttpResults;
using MVCExamenComics.Models;
using System.Data;
using System.Data.SqlClient;

#region PROCEDIMIENTOS ALMACENADOS

//create procedure SP_INSERT_COMIC
//(@NOMBRE NVARCHAR(100), @IMAGEN NVARCHAR(100), @DESCRIPCION NVARCHAR(100))
//as
//	DECLARE @NEXTID INT
//	SELECT @NEXTID = MAX(IDCOMIC) +1 FROM COMICS
//	INSERT INTO COMICS VALUES (@NEXTID, @NOMBRE, @IMAGEN, @DESCRIPCION)
//go

#endregion

namespace MVCExamenComics.Repositories
{
    public class RepositoryComicsSQL: IRepositoryComics
    {
        private DataTable tablaComics;
        private SqlConnection connection;
        private SqlCommand command;

        public RepositoryComicsSQL() 
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS01;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=sa;Password=MCSD2023";         
            this.connection = new SqlConnection(connectionString);
            this.command = new SqlCommand();
            this.command.Connection = this.connection;
            this.tablaComics = new DataTable();
            string sql = "select * from COMICS";
            SqlDataAdapter adComic = new SqlDataAdapter(sql, this.connection);
            adComic.Fill(this.tablaComics);
        }

        public List<Comic> Getcomics()
        {
            var consulta = from datos in this.tablaComics.AsEnumerable() 
                           select datos;
            List<Comic> comics = new List<Comic>();
            foreach (var row in consulta)
            {
                Comic comic = new Comic
                {
                    IdComic = row.Field<int>("IDCOMIC"),
                    Nombre = row.Field<string>("NOMBRE"),
                    Imagen = row.Field<string>("IMAGEN"),
                    Descripcion = row.Field<string>("DESCRIPCION")
                };
                comics.Add(comic);
            }
            return comics;
        }

        public Comic FindComic(int idComic)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           where datos.Field<int>("IDCOMIC") == idComic
                           select datos;
            var row = consulta.First();
            Comic comic = new Comic();
            comic.IdComic = row.Field<int>("IDCOMIC");
            comic.Nombre = row.Field<string>("NOMBRE");
            comic.Imagen = row.Field<string>("IMAGEN");
            comic.Descripcion = row.Field<string>("DESCRIPCION");
            return comic;
        }

        public void InsertComicProcedure(string nombre, string imagen, string descripcion)
        {
            this.command.Parameters.AddWithValue("@NOMBRE", nombre);
            this.command.Parameters.AddWithValue("@IMAGEN", imagen);
            this.command.Parameters.AddWithValue("@DESCRIPCION", descripcion);
            this.command.CommandText = "SP_INSERT_COMIC";
            this.command.CommandType = CommandType.StoredProcedure;
            this.connection.Open();
            int af = this.command.ExecuteNonQuery();
            this.connection.Close();
            this.command.Parameters.Clear();
        }

        public void InsertComicLambda(string nombre, string imagen, string descripcion)
        {
            int nuevoIdComic = this.tablaComics.AsEnumerable().Max(row => row.Field<int>("IDCOMIC") + 1);

            string sql = "insert into COMICS values(@IDCOMIC, @NOMBRE, @IMAGEN, @DESCRIPCION)";

            this.command.Parameters.AddWithValue("@IDCOMIC", nuevoIdComic);
            this.command.Parameters.AddWithValue("@NOMBRE", nombre);
            this.command.Parameters.AddWithValue("@IMAGEN", imagen);
            this.command.Parameters.AddWithValue("@DESCRIPCION", descripcion);
            this.command.CommandText = sql;
            this.command.CommandType = CommandType.Text;
            this.connection.Open();
            int af = this.command.ExecuteNonQuery();
            this.connection.Close();
            this.command.Parameters.Clear();
        }

        public void DeleteComic(int idComic)
        {
            string sql = "DELETE FROM COMICS WHERE IDCOMIC = @idComic";
            this.command.Parameters.AddWithValue("@idComic", idComic);
            this.command.CommandText = sql;
            this.command.CommandType = CommandType.Text;
            this.connection.Open();
            int af = this.command.ExecuteNonQuery();
            this.connection.Close();
            this.command.Parameters.Clear();
        }

        public List<string> GetComicsNombre()
        {
            var consulta = (from datos in this.tablaComics.AsEnumerable()
                           select datos.Field<string>("NOMBRE")).Distinct();
            List<string> nombres = new List<string>();
            foreach (string nombre in consulta)
            {
                nombres.Add(nombre);
            }
            return nombres;
        }

        public Comic FindComicDetalle(string nombre)
        {
            var consulta = from datos in this.tablaComics.AsEnumerable()
                           where datos.Field<string>("NOMBRE") == nombre
                           select datos;
            var row = consulta.First();
            Comic comic = new Comic();
            comic.IdComic = row.Field<int>("IDCOMIC");
            comic.Nombre = row.Field<string>("NOMBRE");
            comic.Imagen = row.Field<string>("IMAGEN");
            comic.Descripcion = row.Field<string>("DESCRIPCION");
            return comic;
        }
    }
}
