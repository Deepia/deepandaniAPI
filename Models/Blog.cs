using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace deepandaniAPI.Models
{
    public class Blog
    {
        public int id { get; set; }
        public string title { get; set; }
        public string short_desc { get; set; }
        public string author { get; set; }
        public string image { get; set; }
        public DateTime created_at { get; set; }
        public string description { get; set; }
    }

    public class BlogForAdmin
    {
        public int id { get; set; }
        public string title { get; set; }
        public string short_desc { get; set; }
        public int user_id { get; set; }
        public string image { get; set; }
        public bool is_featured { get; set; }
        public bool is_active { get; set; }
        public DateTime created_at { get; set; }
        public string description { get; set; }
    }

    public class category
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class Page
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
    }

    public class Pages
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public bool is_active { get; set; }
        public DateTime created_at { get; set; }
        public string slug { get; set; }
    }


    public class Contact
    {
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string message { get; set; }
    }
    public class User
    {
        public string username { get; set; }
        public string password { get; set; }
        public string token { get; set; }
    }

    public class Result
    {
        public string status { get; set; }
    }
}