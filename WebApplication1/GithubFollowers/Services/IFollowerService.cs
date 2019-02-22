using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.ServiceLayer
{
    public interface IFollowerService
    {
        Task<IActionResult> GetFollowers(string username);
    }
}