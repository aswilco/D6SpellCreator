using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using D6SpellCreator.Models;
using SQLitePCL;

namespace D6SpellCreator.Services
{
    public class SpellDatabase
    {
        SQLiteAsyncConnection database;
        public SpellDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Spell>().Wait();
        }

        public Task<List<Spell>> GetItemsAsync()
        {
            return database.Table<Spell>().ToListAsync();
        }

        //public Task<List<Spell>> GetItemsNotDoneAsync()
        //{
        //    return database.QueryAsync<Spell>("SELECT * FROM [TodoItem] WHERE [Done] = 0");
        //}

        public Task<Spell> GetItemAsync(int id)
        {
            return database.Table<Spell>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(Spell item)
        {
            if (item.ID != 0)
            {
                return database.UpdateAsync(item);
            }
            else
            {
                return database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(Spell item)
        {
            return database.DeleteAsync(item);
        }


    }
}
