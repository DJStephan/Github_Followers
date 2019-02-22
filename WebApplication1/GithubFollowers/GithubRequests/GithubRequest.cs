using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using WebApplication1.BizLogic;

namespace WebApplication1.GithubRequests
{
    public class GithubRequest : IGithubRequest
    {
        private HttpClient _client;

        public GithubRequest(HttpClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Makes a GET request of github API at the passed followersUrl returns a List<GithubUser> returned by
        /// the API. Throws UsernameDoesNotExistException if there is no such user associated with the followersUrl.
        /// </summary>
        /// <param name="followersUrl">Url of github API to perform Get Request of</param>
        /// <returns>List<GithubUser></returns>
        public async Task<List<GithubUser>> GetFollowersAsync(string followersUrl)
        {
            List<GithubUser> followers;
            HttpResponseMessage response = await _client.GetAsync(followersUrl);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new UsernameDoesNotExistException("Requested user does not exist");
                } 
                
                throw new Exception($"The call to the github API had an unexpected error: {response.StatusCode}");
            }
            
            followers = await response.Content.ReadAsAsync<List<GithubUser>>();
            return followers;
        }
    }
}