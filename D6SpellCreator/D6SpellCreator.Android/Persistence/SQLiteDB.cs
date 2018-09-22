
using System.IO;
using Xamarin.Forms;
using SQLite;
using D6SpellCreator.Persistence;
using D6SpellCreator.Droid;

[assembly:Dependency(typeof(SQLiteDB))]
namespace D6SpellCreator.Droid
{
    public class SQLiteDB : ISQLLiteDB
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);
            var path = Path.Combine(documentsPath, "Spells.db3");

            return new SQLiteAsyncConnection(path);
        }
    }
}