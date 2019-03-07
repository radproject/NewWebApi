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
        private IRepository<Post, int> repository;

        public PostController(IRepository<Post, int> repository)
        {
            this.repository = repository;
        }

        #region Controllers
        [HttpGet]
        [Route("getbyid")]
        [Authorize]
        public async Task<PostViewModel> GetById(int id)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("getall")]
        [Authorize]
        public async Task<IEnumerable<PostViewModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("create")]
        [Authorize]
        public async Task<IHttpActionResult> CreatePost([FromBody]PostBindingModel model)
        {
            throw new NotImplementedException();
        }

        [HttpPatch]
        [Route("update")]
        [Authorize]
        public async Task<IHttpActionResult> UpdatePost([FromBody]PostBindingModel model)
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
        #endregion
    }
}
