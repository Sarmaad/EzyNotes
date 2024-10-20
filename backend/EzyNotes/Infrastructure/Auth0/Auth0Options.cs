namespace EzyNotes.Infrastructure.Auth0
{
    public class Auth0Options
    {
        public static string Section = "Auth0";

        public string Domain { get; set; }
        public string Audience { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
