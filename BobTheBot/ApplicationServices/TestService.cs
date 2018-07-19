using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobTheBot.ApplicationServices
{
    public class TestService
    {
        public TestService()
        {

        }

        public async Task<string> GetAsync(IMessageActivity messageActivity)
        {
            var message = messageActivity;
            
            if (message.Text.ToLower() == "rosen")
            {
                return "its working";
            }
            return "still working but word is different";
        }


    }
}
