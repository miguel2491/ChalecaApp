using System;
using System.Collections.Generic;
using System.Text;

namespace Chalecas.Models
{
    public class NotificacionM
    {
        public int id { set; get; }
        public string id_user_emisor { set; get; }
        public string id_user_receptor { set; get; }
        public string mensaje { set; get; }
        public string fecha { set; get; }
    }
}
