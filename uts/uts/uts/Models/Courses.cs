using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uts.Models
{
    public class Courses
    {
        public int courseId { get; set; } = 0;
        public required string name { get; set; }
        public string? imageName { get; set; }
        public double? duration { get; set; }
        public string? description { get; set; }
        public int categoryId { get; set; } = 0;
    }
}
