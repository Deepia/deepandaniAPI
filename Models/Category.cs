using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace deepandaniAPI.Models
{
    public class Category
    {
        public int id { get; set; }
        public string category_name { get; set; }
        public bool is_active { get; set; }
        public DateTime created_at { get; set; }
    }
}