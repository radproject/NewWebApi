using NoodleProject.WebApi.Models.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NoodleProject.WebApi.Models.Db
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        [Display(Name = "Text")]
        public string Text { get; set; }
        [Display(Name = "Time stamp")]
        public long TimeStamp { get; set; }

        [ForeignKey("associatedThread")]
        public int ThreadID { get; set; }
        public Topic associatedThread { get; set; }

        [ForeignKey("creator")]
        public string CreatorId { get; set; }

        public ApplicationUser creator { get; set; }
    }
}