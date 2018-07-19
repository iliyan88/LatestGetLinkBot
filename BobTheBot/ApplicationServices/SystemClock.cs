using Microsoft.Extensions.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobTheBot.ApplicationServices
{
    public class SystemClock : ISystemClock
    {
        public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
    }
}
