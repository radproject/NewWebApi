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
using Microsoft.Ajax.Utilities;
using NoodleProject.WebApi.Models.Posts;
using NoodleProject.WebApi.Models;

namespace NoodleProject.WebApi.Controllers
{
    [RoutePrefix("topics")]
    public class TopicController : ApiController
    {

        private ITopicRepository repository;
        private IRepository<ApplicationUser, string> userRepository;

        public TopicController(ITopicRepository repository, IRepository<ApplicationUser, string> userRepository)
        {
            this.repository = repository;
            this.userRepository = userRepository;
        }

        public TopicController() { }

        //public TopicController()
        //{
        //    ApplicationDbContext dbContext = new ApplicationDbContext();
        //    this.repository = new TopicRepository(dbContext);
        //    this.userRepository = new UserRepository();
        //}

        #region Controllers
        [HttpGet]
        [Route("getbyid")]
        //[Authorize]
        public async Task<TopicViewModel> GetById(int id)
        {
            try
            {
                Topic topic = this.repository.GetOneById(id);
                List<PostViewModel> posts = new List<PostViewModel>();
                if(topic.posts == null)
                {
                    posts = null;
                }
                else
                {
                    topic.posts.ForEach(post =>
                    {
                        PostViewModel postVM = new PostViewModel()
                        {
                            Id = post.Id,
                            Text = post.Text,
                            ThreadID = post.ThreadID,
                            TimeStamp = post.TimeStamp
                        };

                        ApplicationUser tempuser = this.userRepository.GetOneById(post.CreatorId);
                        if (tempuser == null)
                        {
                            postVM.creator = null;
                        }
                        else
                        {
                            postVM.creator = new IUserPostViewModel()
                            {
                                Id = tempuser.Id,
                                FullName = tempuser.FullName
                            };
                        }

                        posts.Add(postVM);
                    });
                }

                TopicViewModel topicVM = new TopicViewModel()
                {
                    CreationDate = topic.CreationDate,
                    Id = topic.Id,
                    isPrivate = topic.isPrivate,
                    posts = posts,
                    Title = topic.Title
                };

                if(topic.creator == null)
                {
                    topicVM.creator = null;
                }
                else
                {
                    topicVM.creator = new Models.PublicUserViewModel()
                    {
                        Email = topic.creator.Email,
                        FullName = topic.creator.FullName,
                        Id = topic.creator.Id,
                        StudentID = topic.creator.StudentNumber
                    };
                }

                return topicVM;
            }
            catch
            {
                return null;
            }
        }

        [HttpGet]
        [Route("getall")]
        [Authorize]
        public async Task<IEnumerable<TopicViewModel>> GetAll()
        {
            try
            {
                IEnumerable<Topic> topics = repository.getAll();

                List<TopicViewModel> dtoTopics = new List<TopicViewModel>();
                topics.ForEach(topic =>
                {
                    TopicViewModel topicVM = new TopicViewModel()
                    {
                        CreationDate = topic.CreationDate,
                        Id = topic.Id,
                        posts = null,
                        isPrivate = topic.isPrivate,
                        Title = topic.Title
                    };

                    ApplicationUser tempUser = this.userRepository.GetOneById(topic.creatorId);
                    if(tempUser != null)
                    {
                        topicVM.creator = new PublicUserViewModel()
                        {
                            FullName = tempUser.FullName,
                            Email = tempUser.Email,
                            Id = tempUser.Id,
                            StudentID = tempUser.StudentNumber
                        };
                    }
                    else
                    {
                        topicVM.creator = null;
                    }

                    dtoTopics.Add(topicVM);
                });
                return dtoTopics;
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

                if (user == null)
                {
                    return BadRequest("Bad Token! No User present!");
                }

                repository.CreateOne(new Topic { Title = model.Title, CreationDate = model.CreationDate, isPrivate = model.isPrivate, creatorId = user.Id });
                return Ok("Topic Created");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPatch]
        [Route("update")]
        [Authorize]
        public async Task<IHttpActionResult> UpdatePost([FromBody]TopicBindingModel model)
        {
            try
            {
                Topic t = this.repository.GetOneById(model.Id);
                if (t.creator != this.userRepository.GetOneById(User.Identity.GetUserId()) && !User.IsInRole("Admin"))
                {
                    return BadRequest("Cannot update a Topic which you are not an admin of!");
                }
                repository.UpdateOne(new Topic { Id = model.Id, Title = model.Title, CreationDate = model.CreationDate });
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
                    if (topic.creator == user || User.IsInRole("Admin"))
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
                    if (topic.creator == user || User.IsInRole("Admin"))
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