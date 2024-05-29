using System.Threading.Tasks;
using Microsoft.Playwright;

namespace PlaywrightTests.Pages
{
    public interface ILoginPage {
        public ILocator UsernameInput();
        public ILocator PasswordInput();
        public ILocator LoginButton();
        public Task Login(string username, string password);
    }

    public class LoginPage : ILoginPage
    {
        private readonly IPage page;

        public LoginPage(IPage page)
        {
            this.page = page;
        }

        public ILocator UsernameInput() => page.Locator("input[id='username']");
        public ILocator PasswordInput() => page.Locator("input[id='password']");
        public ILocator LoginButton() => page.Locator("button[type='submit']");

        public async Task Login(string username, string password)
        {
            await UsernameInput().FillAsync(username);
            await PasswordInput().FillAsync(password);
            await LoginButton().ClickAsync();
        }
    }
}
