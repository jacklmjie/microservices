﻿using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Contact.API.Dtos;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Contact.API.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly IServiceDiscovery _serviceDiscovery;
        private readonly ServiceDiscoveryOptions _serviceDiscoveryOptions;
        public UserService(HttpClient httpClient,
            IServiceDiscovery serviceDiscovery,
            IOptions<ServiceDiscoveryOptions> options)
        {
            _httpClient = httpClient;
            _serviceDiscovery = serviceDiscovery;
            _serviceDiscoveryOptions = options.Value;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public async Task<UserIdentityDTO> GetUserAsync(int UserId)
        {
            var url = _serviceDiscovery.FindServiceInstances(_serviceDiscoveryOptions.UserServiceName);
            var response = await _httpClient.GetAsync(url + $"/api/users/identity/{UserId}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                var userIdentity = JsonConvert.DeserializeObject<UserIdentityDTO>(result);
                return userIdentity;
            }
            return null;
        }
    }
}
