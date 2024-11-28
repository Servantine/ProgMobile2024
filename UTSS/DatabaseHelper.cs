using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using UTSS.Models;

namespace UTSS.Helpers
{
    public class DatabaseHelper
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseHelper(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<Categories>().Wait();
        }

        public Task<int> AddCategoryAsync(Categories category)
        {
            return _database.InsertAsync(category);
        }

        public Task<List<Categories>> GetCategoriesAsync()
        {
            return _database.Table<Categories>().ToListAsync();
        }

        public Task<Categories> GetCategoryByIdAsync(int id)
        {
            return _database.Table<Categories>()
                            .Where(c => c.categoryId == id)
                            .FirstOrDefaultAsync();
        }

        public Task<int> UpdateCategoryAsync(Categories category)
        {
            return _database.UpdateAsync(category);
        }

        public Task<int> DeleteCategoryAsync(Categories category)
        {
            return _database.DeleteAsync(category);
        }


    }
}
