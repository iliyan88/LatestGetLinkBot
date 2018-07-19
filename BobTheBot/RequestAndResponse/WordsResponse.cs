using BobTheBot.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BobTheBot.RequestAndResponse
{
    public class WordsResponse
    {
        public int Id { get; set; }
        public string Word { get; set; }

        public WordsResponse(int id, string word)
        {
            Id = id;
            Word = word;
        }

        public static WordsResponse Create(SearchKey keys)
        {
            return new WordsResponse(
                keys.Id,
                keys.Word);
        }
    }
}
