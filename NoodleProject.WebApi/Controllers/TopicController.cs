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
using Microsoft.AspNet.Identity;

namespace NoodleProject.WebApi.Controllers
{
    [RoutePrefix("topics")]
    public class TopicController : ApiController
    {

        private ITopicRepository repository;
        private IRepository<ApplicationUser, string> userRepository;

        public TopicController(ITopicRepository repository)
        {
            this.repository = repository;
        }

        public TopicController()
        {
            ApplicationDbContext dbContext = new ApplicationDbContext();
            this.repository = new TopicRepository(dbContext);
            this.userRepository = new UserRepository();
        }

        #region Controllers
        [HttpGet]
        [Route("getbyid")]
        [Authorize]
        public async Task<Topic> GetById(int id)
        {
            try
            {
                Topic topic = this.repository.GetOneById(id);
                return topic;
            }
            catch
            {
                return null;
            }
        }

        [HttpGet]
        [Route("getall")]
        [Authorize]
        public async Task<IEnumerable<Topic>> GetAll()
        {
            try
            {
                IEnumerable<Topic> topics = repository.getAll();
                return topics;
            }
            catch
            {
                return null;
            }
        }

        [HttpGet]
        [Route("getsubscribedtopics")]
        [Authorize]
        public async Task<IEnumerable<Topic>> GetAllByUserId(string UserId)
        {
            try
            {
                IEnumerable<Topic> topicData = this.repository.getAllForUserId(UserId);

                return topicData;
            }
            catch
            {
                return null;
            }
        }

        [HttpPost]
        [Route("create")]
        [Authorize]
        public async Task<IHttpActionResult> CreatePost([FromBody]TopicBindingModel model)
        {
            try
            {
                ApplicationUser user = this.userRepository.GetOneById(User.Identity.GetUserId());

                if(user == null)
                {
                    return BadRequest("Bad Token! No User present!");
                }
                repository.CreateOne(new Topic {Title = model.Title, CreationDate = model.CreationDate, isPrivate = model.isPrivate, creator = user, subscribers = new List<ApplicationUser>() { user }});
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
                Topic t = this.repository.GetOneById(model.ID);
                if(t.creator != this.userRepository.GetOneById(User.Identity.GetUserId()) && !User.IsInRole("Admin"))
                {
                    return BadRequest("Cannot update a Topic which you are not an admin of!");
                }
                repository.UpdateOne(new Topic { ID = model.ID, Title = model.Title, CreationDate = model.CreationDate });
                return Ok("Topic Updated");
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
                Topic t = this.repository.GetOneById(id);
                if (t.creator != this.userRepository.GetOneById(User.Identity.GetUserId()) && !User.IsInRole("Admin"))
                {
                    return BadRequest("Cannot delete a Topic which you are not an admin of!");
                }

                repository.DeleteOneById(id);
                return Ok("Topic Deleted");
            }
            catch
            {
                return BadRequest("Bad Request");
            }
        }

        [HttpPost]
        [Route("subscribe")]
        [Authorize]
        public async Task<IHttpActionResult> AddSub(int TopicId, string UserId)
        {
            try
            {
                ApplicationUser user = this.userRepository.GetOneById(User.Identity.GetUserId());
                if (user == null)
                {
                    return BadRequest("Bad Token! No User present!");
                }

                Topic topic = this.repository.GetOneById(TopicId);
                
                //If userID is not specified then set it to your own ID
                if (UserId == "none")
                {
                    topic.subscribers.Add(user);
                    this.repository.UpdateOne(topic);
                    return Ok();
                }
                //Else check if permission to add other users to sub
                else
                {
                    if (topic.creator == user) //&& !User.IsInRole("Admin"))
                    {
                        user = this.userRepository.GetOneById(UserId);
                        topic.subscribers.Add(user);
                        this.repository.UpdateOne(topic);
                        return Ok();
                    }
                    else
                    {
                        return BadRequest("Only topic creators and admins can add subscribers");
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Route("unsubscribe")]
        [Authorize]
        public async Task<IHttpActionResult> RemoveSub(int TopicId, string UserId)
        {
            try
            {
                ApplicationUser user = this.userRepository.GetOneById(User.Identity.GetUserId());
                if (user == null)
                {
                    return BadRequest("Bad Token! No User present!");
                }

                Topic topic = this.repository.GetOneById(TopicId);

                //If userID is not specified then set it to your own ID
                if (UserId == "none")
                {
                    topic.subscribers.Add(user);
                    this.repository.UpdateOne(topic);
                    return Ok();
                }
                //Else check if permission to remove other users to sub
                else
                {
                    if (topic.creator == user) //&& !User.IsInRole("Admin"))
                    {
                        user = this.userRepository.GetOneById(UserId);
                        topic.subscribers.Remove(user);
                        this.repository.UpdateOne(topic);
                        return Ok();
                    }
                    else
                    {
                        return BadRequest("Only topic creators and admins can add subscribers");
                    }
                }
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        #endregion
    }
}