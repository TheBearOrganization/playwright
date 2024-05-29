using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using Microsoft.VisualBasic;

namespace PlaywrightTests.Pages;

public interface IMainPage
{
    public ILocator LetMeHack();
    public ILocator BookNow();
}

public class MainPage : IMainPage
{
    private readonly IPage page;
    public MainPage(IPage page)
    {
        this.page = page;
    }

    #region locators
    public ILocator LetMeHack() => page.GetByRole(AriaRole.Button, new() { Name = "Let me hack!" });
    public ILocator BookNow() => page.GetByRole(AriaRole.Button, new() { Name = "Book this room" }).First;
    #endregion
}