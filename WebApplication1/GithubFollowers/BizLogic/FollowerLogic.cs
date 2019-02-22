using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration.Conventions;
using WebApplication1.BizLogic.Models;
using WebApplication1.GithubRequests;

namespace WebApplication1.BizLogic
{
    public class FollowerLogic : IFollowerLogic
    {
        private IMapper _mapper;
        private IGithubRequest _githubRequest;
        private int totalFollowers = 0;
        private int level = 1;

        public FollowerLogic(IMapper mapper, IGithubRequest githubRequest)
        {
            _mapper = mapper;
            _githubRequest = githubRequest;
        }
        
        /// <summary>
        /// Takes a string of a github username and returns a List<User> that follow the passed username.
        /// Note that User class contains field _followers which contain that users followers. Nested followers will
        /// be returned up to 4 layers deep or a total of 100 followers, which ever is reached first. 
        /// </summary>
        /// <param name="username">The github user to obtain the followers of</param>
        /// <returns>List<User></returns>
        public async Task<List<User>> GetFollowers(string username)
        {
            var followersUrl = $"https://api.github.com/users/{username}/followers?per_page=100";
            List<GithubUser> followers = await _githubRequest.GetFollowersAsync(followersUrl);
            List<User> mappedFollowers = _mapper.Map<List<User>>(followers);
            if (mappedFollowers.Count == 0) return mappedFollowers;
            if (mappedFollowers.Count >= 100) return mappedFollowers.Take(100).ToList();
            totalFollowers += mappedFollowers.Count;
            while (level < 4 && totalFollowers < 100)
            {
                mappedFollowers = await GetNestedFollowers(mappedFollowers);
                level += 1;
            }
           return mappedFollowers;
        }
        
        /// <summary>
        /// This method takes a base list containing the first level of followers and determines what level of
        /// followers need to be found. Retuns a new List<User> that represents all followers at current level.
        /// Throws InvalidOperationException if class variable "level" is not between 1-3.
        /// </summary>
        /// <param name="followers"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private async Task<List<User>> GetNestedFollowers(List<User> followers)
        {
            switch (level)
            {
                case 1:
                {
                    foreach (var follower in followers)
                    {
                        follower.Followers = await GetFollowersFollowers(follower);
                    }

                    return followers;
                }
                case 2: 
                {
                    foreach (var follower in followers)
                    {
                        foreach (var secondLevelFollower in follower.Followers)
                        {
                            secondLevelFollower.Followers = await GetFollowersFollowers(secondLevelFollower);
                        }
                    }

                    return followers;
                }
                case 3:
                {
                    foreach (var follower in followers)
                    {
                        foreach (var secondLevelFollower in follower.Followers)
                        {
                            foreach (var thirdLevelFollower in secondLevelFollower.Followers)
                            {
                                thirdLevelFollower.Followers = await GetFollowersFollowers(thirdLevelFollower);
                            }
                        }
                    }

                    return followers;
                }
                default:
                {
                    throw new InvalidOperationException(
                        "Level field should never be anything other 1-3 " + 
                        "when FollowerLogic.GetNestedFollowers is called");
                }
            }
        }
        
        /// <summary>
        /// Gets followers of user provided. Returns List<User> of all users that follow passed user or the number of
        /// users required to have the total followers be 100, whichever is fewer.
        /// </summary>
        /// <param name="user">User object to find followers of</param>
        /// <returns>Task<List<User>></returns>
        private async Task<List<User>> GetFollowersFollowers(User user)
        {
            if (totalFollowers >= 100) return new List<User>();
            var followersRemaining = 100 - totalFollowers;
            var followersUrl = user.FollowersUrl + "?per_page=100";
            List<GithubUser> followers = await _githubRequest.GetFollowersAsync(followersUrl);
            List<User> mappedFollowers = _mapper.Map<List<User>>(followers);
            if (followersRemaining < mappedFollowers.Count)
                mappedFollowers = mappedFollowers.Take(followersRemaining).ToList();
            totalFollowers += mappedFollowers.Count;
            return mappedFollowers;
        }
    }
    
}