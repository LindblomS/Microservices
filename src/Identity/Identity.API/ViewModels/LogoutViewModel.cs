namespace Identity.Api.ViewModels
{
    using Identity.Api.Models;

    public class LogoutViewModel : LogoutInputModel
    {
        public bool ShowLogoutPrompt { get; set; } = true;
    }
}
