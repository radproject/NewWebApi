using NoodleProject.WebApi.Models.Db;
using NoodleProject.WebApi.Models.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoodleProject.WebApi.Models.Topics
{
    public class TopicViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }

        public IEnumerable<PostViewModel> posts { get; set; }
        public PublicUserViewModel creator { get; set; }
        public bool isPrivate { get; set; }
    }
}