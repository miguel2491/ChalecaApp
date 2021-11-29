namespace Chalecas.SQLiteDB
{
    public interface ISQLite
    {
        SQLite.SQLiteConnection GetConnection();
    }
}
