namespace BobTheBot.Entities
{
    public class SearchKey
    {
        private SearchKey()
        {
        }


        public SearchKey(string word)
        {
            Word = word;
        }

        public int Id { get; set; }
        public string Word { get; set; }
    }
}
