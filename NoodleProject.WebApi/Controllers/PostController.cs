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
        [Inject]
        public IPostRepository repository { get; set; }
        public IRepository<ApplicationUser, string> userRepository { get; set; }

        public PostController(IPostRepository repository)
        {
            this.repository = repository;
        }

        public PostController()
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            this.repository = new PostRepository(dbContext);
            this.userRepository = new UserRepository();
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
                repository.UpdateOne(new Post { ThreadID = model.ThreadId, Text = model.Text });
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
