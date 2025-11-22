namespace Lottery.Services
{
    public class UserState
    {
        public string Email { get; set; } = string.Empty;
        public bool IsLoggedIn { get; set; } = false;
        public string StringHead => IsLoggedIn ? $"Welcome, {Email}" : "Login";

        public event Action? OnChange;

        public void NotifyStateChanged() => OnChange?.Invoke();
    }
}
