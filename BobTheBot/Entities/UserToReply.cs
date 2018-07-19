namespace BobTheBot.Entities
{
    public class UserToReply
    {
        protected UserToReply()
        {
        }


        public UserToReply(string conversationId, string skypeUserId, string skypeUserName)
        {
            ConversationId = conversationId;
            SkypeUserId = skypeUserId;
            SkypeUserName = skypeUserName;
        }

        public int Id { get; set; }
        public string ConversationId { get; set; }

        public string SkypeUserId { get; set; }

        public string SkypeUserName { get; set; }

        public string Email { get; set; }
        public bool SendEmail { get; set; }
        public bool IsActive { get; set; }
    }
}
