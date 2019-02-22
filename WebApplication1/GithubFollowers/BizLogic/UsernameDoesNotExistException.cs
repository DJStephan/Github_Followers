using System;

namespace WebApplication1.BizLogic
{
    public class UsernameDoesNotExistException : Exception
    {
        public UsernameDoesNotExistException(string message) : base(message)
        {
            
        }
    }
}