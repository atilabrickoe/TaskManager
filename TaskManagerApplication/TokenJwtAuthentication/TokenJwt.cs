namespace TaskManagerApplication.TokenJwtAuthentication
{
    public class TokenJwt
    {
        public string Accesstoken { get; set; }
        public DateTime Expiration { get; set; }
        public string TokenType { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
    }
}
