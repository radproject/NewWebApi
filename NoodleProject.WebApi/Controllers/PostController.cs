using Microsoft.AspNet.Identity;
using Ninject;
using NoodleProject.WebApi.Models.Context;
using NoodleProject.WebApi.Models.Db;
using NoodleProject.WebApi.Models.Posts;
using NoodleProject.WebApi.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace NoodleProject.WebApi.Controllers
{
    [RoutePrefix("posts")]
    public class PostController : ApiController
    {
        public IPostRepository repository { get; set; }
        public IRepository<ApplicationUser, string> userRepository { get; set; }
        public ITopicRepository topicRepository { get; set; }

        public PostController(IPostRepository repository)
        {
            this.repository = repository;
        }

        public PostController()
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            this.repository = new PostRepository(dbContext);
            this.userRepository = new UserRepository();
            this.topicRepository = new TopicRepository(dbContext);
        }

        #region Controllers
        [HttpGet]
        [Route("getbyid")]
        [Authorize]
        public async Task<Post> GetById(int id)
        {
            Post posts = repository.GetOneById(id);
            return posts;
        }

        [HttpGet]
        [Route("getforthread")]
        [Authorize]
        public async Task<IEnumerable<Post>> GetAllForThread(int ThreadId)
        {
            IEnumerable<Post> posts = this.repository.getAllByThreadId(ThreadId);
            return posts;
        }

        [HttpPost]
        [Route("create")]
        [Authorize]
        public async Task<IHttpActionResult> CreatePost([FromBody]PostBindingModel model)
        {
            try
            {
                ApplicationUser creator = this.userRepository.GetOneById(model.UserId);
                Topic t = this.topicRepository.GetOneById(model.ThreadId);

                if(t.creator != creator && t.subscribers.Where(x => x.Id == creator.Id).Count() != 1 && !User.IsInRole("Admin"))
                {
                    return BadRequest("Cannot add a post to a topic which you haven't created or are subbed to!");
                }

                repository.CreateOne(new Post { ThreadID = model.ThreadId, creator = creator, Text = model.Text });
                return Ok("Post Created");
            }
            catch
            {
                return BadRequest("Bad Request");
            }
        }


        [HttpPatch]
        [Route("update")]
        [Authorize]
        public async Task<IHttpActionResult> UpdatePost([FromBody]PostBindingModel model)
        {
            try
            {
                ApplicationUser creator = this.userRepository.GetOneById(model.UserId);
                Post p = this.repository.GetOneById(model.PostId);

                if (p.creator != creator && !User.IsInRole("Admin"))
                {
                    return BadRequest("Cannot update a post which you did not create!");
                }

                repository.UpdateOne(new Post { ID = model.PostId, Text = model.Text });
                return Ok("Post Updated");
            }
            catch
            {
                return BadRequest("Bad Request");
            } 
        }

        [HttpPost]
        [Route("delete")]
        [Authorize]
        public async Task<IHttpActionResult> DeletePost(int id)
        {
            try
            {
                ApplicationUser creator = this.userRepository.GetOneById(User.Identity.GetUserId());
                Post p = this.repository.GetOneById(id);

                if (p.creator != creator && !User.IsInRole("Admin"))
                {
                    return BadRequest("Cannot delete a post which you did not create!");
                }

                repository.DeleteOneById(id);
                return Ok("Post Deleted");
            }
            catch
            {
                return BadRequest("Bad Request");
            }
        }
    }    
}
        #endregion
