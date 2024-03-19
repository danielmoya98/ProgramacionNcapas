using System.Data.SqlClient;

namespace CapaDatos
{
    public class Coneccion
    {
        private static Coneccion instacia;
        private readonly string cadenaDeConeccion;

        private Coneccion()
        {
            cadenaDeConeccion = "Server=(LocalDB)\\MSSQLLocalDB;Database=ONION_PROJECT;Integrated Security=true; TrustServerCertificate=True";
        }

        public static Coneccion Instance
        {
            get
            {
                if (instacia == null)
                {
                    instacia = new Coneccion();
                }

                return instacia;
            }
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(cadenaDeConeccion);
        }
    }
}