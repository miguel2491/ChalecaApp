using SQLite;

namespace Chalecas.Models
{
    public class Usuario
    {
        [PrimaryKey, AutoIncrement]
        public int id { set; get; }
        public int id_user { set; get; }
        public string nombre { set; get; }
        public string username { set; get; }
        public string password { set; get; }
        public string rol { set; get; }
        public string token { set; get; }
        public string foto { set; get; }

        public int status { set; get; }
    }
}
