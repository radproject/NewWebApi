using Microsoft.AspNet.SignalR;
using NoodleProject.WebApi.Models.Context;
using NoodleProject.WebApi.Models.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace NoodleProject.WebApi.SignalR
{
    public class AppHub: Hub
    {
        public async Task SendNewPostToConnection(Post p, ICollection<ApplicationUser> subscribers)
        {
            //await Clients.All.SendAsync("ReceiveMessage", user, message);
            await Clients.Users(subscribers.Select(x => x.Id).ToList()).newPost(p);
        }
    }
}