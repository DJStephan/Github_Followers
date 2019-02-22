using System.Collections.Generic;

namespace WebApplication1.ServiceLayer.Dtos
{
    public class UserDto
    {
        public string Username { get; set; }
        public List<UserDto> Followers { get; set; }
    }
}