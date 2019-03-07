using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoodleProject.WebApi.Models.Posts
{
    public class PostBindingModel
    {
        public int ThreadId { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
    }
}