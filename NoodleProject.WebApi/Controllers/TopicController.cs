using NoodleProject.WebApi.Models.Context;
using NoodleProject.WebApi.Models.Db;
using NoodleProject.WebApi.Models.Repositories;
using NoodleProject.WebApi.Models.Topics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace NoodleProject.WebApi.Controllers
{
    public class TopicController : ApiController
    {
        [RoutePrefix("posts")]
        public class PostController : ApiController
        {
            private IRepository<Topic, int> repository;
            private IRepository<ApplicationUser, string> userRepository;

            public PostController(IRepository<Topic, int> repository)
            {
                this.repository = repository;
            }

            #region Controllers
            [HttpGet]
            [Route("getbyid")]
            [Authorize]
            public async Task<Topic> GetById(int id)
            {
                Topic topics = repository.GetOneById(id);
                return topics;
            }

            [HttpGet]
            [Route("getall")]
            [Authorize]
            public async Task<IEnumerable<Topic>> GetAll()
            {
                IEnumerable<Topic> topics = repository.getAll();
                return topics;
            }

            [HttpPost]
            [Route("create")]
            [Authorize]
            public async Task<IHttpActionResult> CreatePost([FromBody]TopicBindingModel model)
            {
                try
                {
                    //ApplicationUser creator = 
                    repository.CreateOne(new Topic { ID = model.ID, Title = model.Title, CreationDate = model.CreationDate });
                    return Ok("Topic Created");
                }
                catch
                {
                    return BadRequest("Bad Request");
                }
            }

            [HttpPatch]
            [Route("update")]
            [Authorize]
            public async Task<IHttpActionResult> UpdatePost([FromBody]TopicBindingModel model)
            {
                try
                {
                    repository.UpdateOne(new Topic { ID = model.ID, Title = model.Title, CreationDate = model.CreationDate });
                    return Ok("Topic Updated");
                }
                catch
                {
                    return BadRequest("Bad Request");
                }
            }

            [HttpPost]
            [Route("DeletePost")]
            [Authorize]
            public async Task<IHttpActionResult> DeletePost(int id)
            {
                try
                {
                    repository.DeleteOneById(id);
                    return Ok("Topic Deleted");
                }
                catch
                {
                    return BadRequest("Bad Request");
                }
            }

            [HttpPost]
            [Route("AddSubscriberToPost")]
            [Authorize]
            public async Task<IHttpActionResult> AddSub(int TopicId, string UserId)
            {
                Topic topics = this.repository.GetOneById(TopicId);
                ApplicationUser applicationUser = this.userRepository.GetOneById(UserId); 
            }
            #endregion
        }
    }
}
