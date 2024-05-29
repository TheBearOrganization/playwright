using System.Threading.Tasks;
using Microsoft.Playwright;

namespace PlaywrightTests.Pages;

public interface IAdminPage
{
    #region locators
    public ILocator RoomsLink();
    public ILocator RoomNumberInput();
    public ILocator TypeInput();
    public ILocator AccessibleInput();
    public ILocator PriceInput();
    public ILocator DescriptionTextarea();
    public ILocator RefreshmentsCheckbox();
    public ILocator TvCheckbox();
    public ILocator SafeCheckbox();
    public ILocator EditButton();
    public ILocator UpdateRoom();
    public ILocator CreateRoom();
    public ILocator RoomNumber(string roomNumber);
    public ILocator ImageUrlInput();
    #endregion

    #region methods
    public Task NavigateToRooms();
    public Task AddRoom(string roomNumber, string type, string price, string description);
    public Task EditRoom(string roomNumber, string newDescription, string imageUrl);
    #endregion
}

public class AdminPage : IAdminPage
{
    private readonly IPage page;

    public AdminPage(IPage page)
    {
        this.page = page;
    }

    public ILocator RoomsLink() => page.Locator("a[href='#/admin/']");
    public ILocator RoomNumberInput() => page.Locator("input[id='roomName']");
    public ILocator TypeInput() => page.Locator("*[id='type']");
    public ILocator AccessibleInput() => page.Locator("*[id='accessible']");
    public ILocator PriceInput() => page.Locator("*[id='roomPrice']");
    public ILocator DescriptionTextarea() => page.Locator("*[id='description']");
    public ILocator RefreshmentsCheckbox() => page.Locator("*[id='refreshCheckbox']");
    public ILocator TvCheckbox() => page.Locator("*[id='tvCheckbox']");
    public ILocator SafeCheckbox() => page.Locator("*[id='safeCheckbox']");
    public ILocator EditButton() => page.Locator("button:has-text('Edit')");
    public ILocator UpdateRoom() => page.Locator("*[id='update']");
    public ILocator CreateRoom() => page.Locator("*[id='createRoom']");
    public ILocator RoomNumber(string roomNumber) => page.Locator($"id=roomName{roomNumber}");
    public ILocator ImageUrlInput() => page.Locator("*[id='image']");

    public async Task NavigateToRooms()
    {
        await RoomsLink().ClickAsync();
    }

    public async Task AddRoom(string roomNumber, string type, string price, string description)
    {
        await RoomNumberInput().FillAsync(roomNumber);
        await TypeInput().ClickAsync();
        await TypeInput().SelectOptionAsync(type);
        await AccessibleInput().SelectOptionAsync("true");
        await PriceInput().FillAsync(price);
        //await DescriptionTextarea.FillAsync(description);
        await RefreshmentsCheckbox().CheckAsync();
        await TvCheckbox().CheckAsync();
        await SafeCheckbox().CheckAsync();
        await CreateRoom().ClickAsync();
    }

    public async Task EditRoom(string roomNumber, string newDescription, string imageUrl)
    {
        await RoomNumber(roomNumber).ClickAsync();
        await EditButton().ClickAsync();
        await DescriptionTextarea().FillAsync(newDescription);
        await ImageUrlInput().FillAsync(imageUrl);
        await UpdateRoom().ClickAsync();
    }
}