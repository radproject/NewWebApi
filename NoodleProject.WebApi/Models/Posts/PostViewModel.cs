using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoodleProject.WebApi.Models.Posts
{
    public class PostViewModel
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public long TimeStamp { get; set; }
        public int ThreadID { get; set; }
        public string CreatorId { get; set; }
    }
}