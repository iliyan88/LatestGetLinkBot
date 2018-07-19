using BobTheBot.ApplicationServices;
using BobTheBot.Entities;
using BobTheBot.RequestAndResponse;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace BobTheBot.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly UserService userService;

        public UserController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpGet()]
        public async Task<IReadOnlyList<UserToReply>> GetUsersAsync()
        {
            var result = await userService.GetAsync();
            return result;
        }

        [HttpPut("{entityId:required}")]
        public async Task<HttpResponseMessage> UpdateSchoolPageLanguageAsync(
            [FromRoute]int entityId,
            [FromBody]UserUpdateRequest request)
        {
            var result = await userService.UpdateAsync(entityId, request);
            return result;
        }

        [HttpDelete("{entityId:required}")]
        public async Task<HttpResponseMessage> DeleteSchoolPageAsync(
            [FromRoute]int entityId)
        {
            var result = await userService.DeleteAsync(entityId);
            return result;
        }
    }
}
