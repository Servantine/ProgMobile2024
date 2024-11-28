using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UTSS.Models
{
    public class Categories
    {
        [PrimaryKey, AutoIncrement]
        public int categoryId { get; set; }


        public string name { get; set; }
        public string description { get; set; }
    }
}
