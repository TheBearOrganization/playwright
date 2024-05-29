using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using Microsoft.VisualBasic;

namespace PlaywrightTests.Pages;

public interface IMainPage
{
    public ILocator LetMeHack();
    public ILocator BookRoomByIndex(int roomIndex);
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
    public ILocator BookRoomByIndex(int roomIndex=1) => page.Locator($"(//div[contains(@class,'hotel-room-info')]//p[contains(text(),'my_test_room')]//../button[@type='button'])[{roomIndex}]");
    #endregion
}