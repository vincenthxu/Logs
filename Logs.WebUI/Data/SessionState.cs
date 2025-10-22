namespace Logs.WebUI.Data
{
    public class SessionState
    {
        public User? CurrentUser { get; set; }
        public AccessTokenResponse? AccessToken { get; set; }

        public bool IsLoggedIn => CurrentUser != null;
    }
}
