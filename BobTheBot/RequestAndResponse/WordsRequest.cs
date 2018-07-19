using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BobTheBot.RequestAndResponse
{
    public class WordsRequest
    {
        [Required]
        public IEnumerable<string> Words { get; set; }
    }
}
