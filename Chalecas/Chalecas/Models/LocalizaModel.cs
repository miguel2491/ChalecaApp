using SQLite;

namespace Chalecas.Models
{
    public class LocalizaModel
    {
        [PrimaryKey, AutoIncrement]
        public int id { set; get; }
        public string id_usuario { set; get; }
        public string latitud { set; get; }
        public string longitud { set; get; }
        public string fecha { set; get; }
        public string status { set; get; }
    }
}
