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

            public PostController(IRepository<Topic, int> repository)
            {
                this.repository = repository;
            }

            #region Controllers
            [HttpGet]
            [Route("getbyid")]
            [Authorize]
            public async Task<TopicViewModel> GetById(int id)
            {
                throw new NotImplementedException();
            }

            [HttpGet]
            [Route("getall")]
            [Authorize]
            public async Task<IEnumerable<TopicViewModel>> GetAll()
            {
                throw new NotImplementedException();
            }

            [HttpPost]
            [Route("create")]
            [Authorize]
            public async Task<IHttpActionResult> CreatePost([FromBody]TopicBindingModel model)
            {
                throw new NotImplementedException();
            }

            [HttpPatch]
            [Route("update")]
            [Authorize]
            public async Task<IHttpActionResult> UpdatePost([FromBody]TopicBindingModel model)
            {
                throw new NotImplementedException();
            }

            [HttpPost]
            [Route("DeletePost")]
            [Authorize]
            public async Task<IHttpActionResult> DeletePost(int id)
            {
                throw new NotImplementedException();
            }

            [HttpPost]
            [Route("AddSubscriberToPost")]
            [Authorize]
            public async Task<IHttpActionResult> AddSub(int TopicId, string UserId)
            {
                throw new NotImplementedException();
            }
            #endregion
        }
    }
}
