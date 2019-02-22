using System.Collections.Generic;

namespace WebApplication1.BizLogic.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FollowersUrl { get; set; }
        private List<User> _followers = new List<User>();
        public List<User> Followers
        {
            get => new List<User>(_followers);
            set => _followers = value;
        }

    }
}