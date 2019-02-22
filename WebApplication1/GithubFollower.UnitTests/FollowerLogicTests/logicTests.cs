using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;
using NUnit.Framework;
using WebApplication1.BizLogic;
using WebApplication1.BizLogic.Models;
using WebApplication1.GithubRequests;
using WebApplication1.ServiceLayer.Mapper;

namespace GithubFollower.UnitTests.FollowerLogicTests
{
    [TestFixture]
    public class LogicTests
    {
        private IMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            _mapper = mappingConfig.CreateMapper();
        }
        [Test]
        public async Task GetFollowers_UserHasAtLeast100Followers_ReturnsOneLevelFollowers()
        {
            var followerLogic = new FollowerLogic(_mapper, new OneHundredFollowersRequest());

            List<User> result = await followerLogic.GetFollowers("");

            Assert.That(result.Count == 100);
            Assert.That(result.TrueForAll(user => user.Followers.Count == 0));//no second level
        }

        [Test]
        public async Task GetFollowers_MaxReachedOnLevelTwo_ReturnTwoLevels()
        {
            var followerLogic = new FollowerLogic(_mapper, new TwoLevelRequest());

            List<User> result = await followerLogic.GetFollowers("");
            
            Assert.That(result.Count == 1);
            Assert.That(result[0].Followers.Count == 99);
            Assert.That(result[0].Followers.TrueForAll(user => user.Followers.Count == 0));//no third level
        }

        [Test]
        public async Task GetFollowers_MaxReachedOnLevelThree_ReturnThreeLevels()
        {
            var followerLogic = new FollowerLogic(_mapper, new ThreeLevelRequest());

            List<User> result = await followerLogic.GetFollowers("");
            
            Assert.That(result.Count == 1);
            Assert.That(result[0].Followers.Count == 1);
            Assert.That(result[0].Followers[0].Followers.Count == 98);
            Assert.That(result[0].Followers[0].Followers.TrueForAll(user => user.Followers.Count == 0));//no 4th level
        }

        [Test]
        public async Task GetFollowers_MaxNotReached_ReturnFourLevels()
        {
            var followerLogic = new FollowerLogic(_mapper, new FourLevelRequest());

            List<User> result = await followerLogic.GetFollowers("");
            
            Assert.That(result.Count == 1);
            Assert.That(result[0].Followers.Count == 1);
            Assert.That(result[0].Followers[0].Followers.Count == 1);
            Assert.That(result[0].Followers[0].Followers[0].Followers.Count == 1); //4th level exists
            Assert.That(
                result[0].Followers[0].Followers[0].Followers.TrueForAll(user => user.Followers.Count == 0)
                );
        }
    }

    public class ThreeLevelRequest : IGithubRequest
    {
        public async Task<List<GithubUser>> GetFollowersAsync(string followersUrl)
        {
            var users = new List<GithubUser>();
            switch (followersUrl)
            {
                case "secondCall?per_page=100":
                {
                    users.Add(new GithubUser(){FollowersUrl = "thirdCall"});
                    return users;
                }
                case "thirdCall?per_page=100":
                {
                    for (int i = 0; i < 98; i++)
                    {
                        users.Add(new GithubUser());
                    }

                    return users;
                }
            }
            
            users.Add(new GithubUser(){FollowersUrl = "secondCall"});
            return users;
        }
    }

    public class FourLevelRequest : IGithubRequest
    {
        public async Task<List<GithubUser>> GetFollowersAsync(string followersUrl)
        {
            var users = new List<GithubUser>();
            users.Add(new GithubUser());
            return users;
        }
    }

    public class TwoLevelRequest : IGithubRequest
    {
        //UserLogic should make a call to this method twice to reach the max 100 followers
        public async Task<List<GithubUser>> GetFollowersAsync(string followersUrl)
        {
            var users = new List<GithubUser>();

            if (followersUrl.Equals("secondCall?per_page=100"))
            {
                for (int i = 0; i < 99; i++)
                {
                    users.Add(new GithubUser());
                }
            }
            else
            {
                users.Add(new GithubUser(){FollowersUrl = "secondCall"});
            }

            return users;
        }
    }

    class OneHundredFollowersRequest : IGithubRequest
    {
        public async Task<List<GithubUser>> GetFollowersAsync(string followersUrl)
        {
            var users = new List<GithubUser>();
            for (int i = 0; i < 100; i++)
            {
                users.Add(new GithubUser());
            }

            return users;
        }
    }


}