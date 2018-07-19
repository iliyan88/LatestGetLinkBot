using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BobTheBot.RequestAndResponse
{
    public class UserUpdateRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        public bool SendEmail { get; set; }
        public bool IsActive { get; set; }
    }
}
