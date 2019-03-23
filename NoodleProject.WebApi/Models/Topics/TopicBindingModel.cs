using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoodleProject.WebApi.Models.Topics
{
    public class TopicBindingModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }

        public int PostId { get; set; }
        public string CreatorId { get; set; }
        public ICollection<string> SubscribedIds { get; set; }

        public bool isPrivate { get; set; }
    }
}