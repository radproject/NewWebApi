using NoodleProject.WebApi.Models.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NoodleProject.WebApi.Models.Db
{
    public class Topic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Display(Name = "Title name")]
        public string Title { get; set; }
        [Display(Name = "Creation date")]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yy}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }

        public ICollection<ApplicationUser> subscribers { get; set; }

        [ForeignKey("posts")]
        public int postId { get; set; }
        public List<Post> posts { get; set; }
        [ForeignKey("creator")]
        public string creatorId { get; set; }
        public ApplicationUser creator { get; set; }

        public bool isPrivate { get; set; }
    }
}