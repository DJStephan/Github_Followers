using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.BizLogic.Models;

namespace WebApplication1.GithubRequests
{
    public interface IGithubRequest
    {
        Task<List<GithubUser>> GetFollowersAsync(string followersUrl);
    }
}