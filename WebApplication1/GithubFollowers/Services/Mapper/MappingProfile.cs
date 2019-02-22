using AutoMapper;
using System.Collections;
using System.Collections.Generic;
using WebApplication1.BizLogic.Models;
using WebApplication1.GithubRequests;
using WebApplication1.ServiceLayer.Dtos;

namespace WebApplication1.ServiceLayer.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<GithubUser, User>()
                .ForMember(destination => destination.Username, options
                    => options.MapFrom(source => source.Login));
        }
    }
}