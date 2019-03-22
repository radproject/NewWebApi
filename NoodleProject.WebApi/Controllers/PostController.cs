using Microsoft.AspNet.Identity;
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

        public PostController(IPostRepository repository, ITopicRepository topicRepository, IRepository<ApplicationUser, string> userRepository)
        {
            this.repository = repository;
            this.topicRepository = topicRepository;
            this.userRepository = userRepository;
        }

        public PostController() { }
        //public PostController()
        //{
        //    ApplicationDbContext dbContext = new ApplicationDbContext();
        //    this.repository = new PostRepository(dbContext);
        //    this.userRepository = new UserRepository();
        //    this.topicRepository = new TopicRepository(dbContext);
        //}

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
                ApplicationUser creator = this.userRepository.GetOneById(User.Identity.GetUserId());
                Topic t = this.topicRepository.GetOneById(model.ThreadId);

                //If the user is an admin, the creator or its public or its private and theyre a subscriber, post to topic
                if(User.IsInRole("Admin") || (t.creator == creator) || t.isPrivate == false || (t.subscribers.Where(x => x.Id == creator.Id).Count() != 1))
                {
                    repository.CreateOne(new Post() { ThreadID = model.ThreadId, Text = model.Text, creator = creator, TimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds() });
                    return Ok("Post Created");
                }
                else
                {
                    return BadRequest("Cannot post to a topic unless you're an admin, it's creator or a subscriber");
                }
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
                ApplicationUser creator = this.userRepository.GetOneById(User.Identity.GetUserId());
                Post p = this.repository.GetOneById(model.PostId);

                //If the user is an admin or the creator, update the post
                if (User.IsInRole("Admin") || (p.creator == creator))
                {
                    repository.UpdateOne(new Post { ID = model.PostId, Text = model.Text });
                    return Ok("Post Updated");
                }
                else
                {
                    return BadRequest("Cannot update this post unless you are its creator or an admin");
                }
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
