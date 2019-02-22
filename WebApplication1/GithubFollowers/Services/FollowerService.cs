using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.BizLogic;
using WebApplication1.BizLogic.Models;
using WebApplication1.ServiceLayer.Dtos;

namespace WebApplication1.ServiceLayer
{
    public class FollowerService : IFollowerService
    {
        private IMapper _mapper;
        private IFollowerLogic _followerLogic;

        public FollowerService(IMapper mapper, IFollowerLogic followerLogic)
        {
            _mapper = mapper;
            _followerLogic = followerLogic;
        }
        /// <summary>
        /// Takes github username and returns a nested list of that users followers with status code 200 if successful,
        /// an error message with status code 404 if no such user exists, or an error message with status code 500 if
        /// an error occurs while processing the result.
        /// </summary>
        /// <param name="username">The github username to find the followers of</param>
        /// <returns>Task<IActionResult></returns>
        public async Task<IActionResult> GetFollowers(string username)
        {
            List<User> followers;
            try
            {
                followers = await _followerLogic.GetFollowers(username);
            }
            catch (Exception e)
            {
                if(e is UsernameDoesNotExistException) return new ContentResult()
                    {Content = $"{username} is not a Github user.", StatusCode = 404};
                
                return new ContentResult(){Content = e.Message, StatusCode = 500};
            }

            List<UserDto> mappedResult = _mapper.Map<List<UserDto>>(followers);
            return new OkObjectResult(mappedResult);
        }
    }
}