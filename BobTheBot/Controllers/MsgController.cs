using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Connector;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace BobTheBot.Controllers
{
    public class MsgController : Controller
    {
        //private readonly IConfigurationRoot _configurationRoot;
        //protected MsgController()
        //{
        //}

        //public MsgController(IConfigurationRoot configurationRoot)
        //{
        //    _configurationRoot = configurationRoot;
        //}

        //[HttpPost]
        //[Route("Hook")]
        //public async Task<HttpResponseMessage> Hook([FromBody]Activity activity)
        //{
        //    var appCredentials = new MicrosoftAppCredentials(_configurationRoot);
        //    var connector = new ConnectorClient(new Uri(activity.ServiceUrl), appCredentials);

        //    var length = (activity.Text ?? string.Empty).Length;
        //    var reply = activity.CreateReply($"You send{activity.Text} which was{length} characters!");

        //    await connector.Conversations.ReplyToActivityAsync(reply);
        //    var response = new HttpResponseMessage(HttpStatusCode.OK);
        //    return response;
        //}
    }
}
