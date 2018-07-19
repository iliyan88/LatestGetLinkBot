using BobTheBot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobTheBot.Kernel
{
    public class WordDto
    {
        public WordDto()
        {

        }

        public WordDto(string word)
        {
            Word = word;
        }

        public string Word { get; set; }

        public static WordDto Create(SearchKey key)
        {
            return new WordDto(key.Word);
        }
    }
}
