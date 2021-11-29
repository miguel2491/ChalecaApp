using SQLite;
using Chalecas.Droid;
using Chalecas.SQLiteDB;
using Xamarin.Forms;

[assembly: Dependency(typeof(Android_SQLite))]
namespace Chalecas.Droid
{
    public class Android_SQLite : ISQLite
    {
        public SQLiteConnection GetConnection()
        {
            var dbName = "Chaleca.sqlite";
            var dbPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData);
            var path = System.IO.Path.Combine(dbPath, dbName);
            var conn = new SQLite.SQLiteConnection(path);
            return conn;
        }
    }
}