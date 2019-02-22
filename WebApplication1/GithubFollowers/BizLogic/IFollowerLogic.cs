using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.BizLogic.Models;
using WebApplication1.GithubRequests;

namespace WebApplication1.BizLogic
{
    public interface IFollowerLogic
    {
        Task<List<User>> GetFollowers(string username);
    }
}