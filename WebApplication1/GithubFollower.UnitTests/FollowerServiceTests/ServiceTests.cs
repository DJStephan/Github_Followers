using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using WebApplication1.BizLogic;
using WebApplication1.BizLogic.Models;
using WebApplication1.ServiceLayer;
using WebApplication1.ServiceLayer.Dtos;
using WebApplication1.ServiceLayer.Mapper;

namespace GithubFollower.UnitTests.FollowerServiceTests
{
    [TestFixture]
    public class ServiceTests
    {
        [Test]
        public async Task GetFollowers_NotValidUsername_ReturnsErrorMessage()
        {
            var followerService = new FollowerService(new MockMapper(), new UserNotFoundLogic());

            IActionResult result = await followerService.GetFollowers("");
            var contentResult = (ContentResult) result;
            
            Assert.That(result.GetType() == typeof(ContentResult));
            Assert.That(contentResult.StatusCode == 404);
        }

        [Test]
        public async Task GetFollowers_SuccessfulResult_ReturnsOkObjectResult()
        {
            var followerService = new FollowerService(new MockMapper(), new SuccessfulResultLogic());

            IActionResult result = await followerService.GetFollowers("");
            
            Assert.That(result.GetType() == typeof(OkObjectResult));
        }
    }

    public class SuccessfulResultLogic : IFollowerLogic
    {
        public async Task<List<User>> GetFollowers(string username)
        {
            return new List<User>(){new User()};
        }
    }

    public class UserNotFoundLogic : IFollowerLogic
    {
        public Task<List<User>> GetFollowers(string username)
        {
            throw new UsernameDoesNotExistException("");
        }
    }

    public class MockMapper : IMapper
    {
        public TDestination Map<TDestination>(object source)
        {
            return default(TDestination);
        }

        public TDestination Map<TDestination>(object source, Action<IMappingOperationOptions> opts)
        {
            throw new NotImplementedException();
        }

        public TDestination Map<TSource, TDestination>(TSource source)
        {
            throw new NotImplementedException();
        }

        public TDestination Map<TSource, TDestination>(TSource source, Action<IMappingOperationOptions<TSource, TDestination>> opts)
        {
            throw new NotImplementedException();
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            throw new NotImplementedException();
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination, Action<IMappingOperationOptions<TSource, TDestination>> opts)
        {
            throw new NotImplementedException();
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            throw new NotImplementedException();
        }

        public object Map(object source, Type sourceType, Type destinationType, Action<IMappingOperationOptions> opts)
        {
            throw new NotImplementedException();
        }

        public object Map(object source, object destination, Type sourceType, Type destinationType)
        {
            throw new NotImplementedException();
        }

        public object Map(object source, object destination, Type sourceType, Type destinationType, Action<IMappingOperationOptions> opts)
        {
            throw new NotImplementedException();
        }

        public IConfigurationProvider ConfigurationProvider { get; }
        public Func<Type, object> ServiceCtor { get; }
    }
}