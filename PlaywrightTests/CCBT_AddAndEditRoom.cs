using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework;
using PlaywrightTests.Pages;

namespace PlaywrightTests
{
    public class CCBT_AddAndEditRoom
    {

        private IPlaywright playwright;
        private IBrowser browser;
        private IPage page;

        private MainPage mainPage;
        private LoginPage loginPage;
        private AdminPage adminPage;
        private string _roomNumber;

        [SetUp]
        public async Task Setup()
        {
            playwright = await Playwright.CreateAsync();
            browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true, SlowMo = 100 });
            page = await browser.NewPageAsync();

            mainPage = new MainPage(page);
            loginPage = new LoginPage(page);
            adminPage = new AdminPage(page);

            _roomNumber = new Random().Next(100, 300).ToString();
        }

        [TearDown]
        public async Task Teardown()
        {
            await page.CloseAsync();
            await browser.CloseAsync();
            playwright.Dispose();
        }

        [Test]
        public async Task AddAndEditRoom()
        {

            await page.GotoAsync("https://automationintesting.online/#/admin/");

            await mainPage.LetMeHack().ClickAsync();

            // Log in to the admin portal
            await loginPage.Login("admin", "password");

            // Wait for navigation to the admin dashboard
            await page.WaitForSelectorAsync("text=Rooms");

            // Navigate to the "Rooms" section
            await adminPage.NavigateToRooms();

            // Wait for the rooms section to load
            await page.WaitForSelectorAsync("button:has-text('Create')");

            // Add a new double room
            await adminPage.AddRoom(_roomNumber, "Double", "100", $"A cozy double room with all amenities. my_test_room_{_roomNumber}");

            // Wait for the room to be added
            await page.WaitForSelectorAsync($"id=roomName{_roomNumber}");

            // Edit the room's description and image
            await adminPage.EditRoom(_roomNumber, $"A cozy double room with all amenities. my_test_room_{_roomNumber}", "https://i.postimg.cc/bY5pFcLg/Screenshot-2024-05-21-202848.jpg");

            // Verify the updated details
            await page.WaitForSelectorAsync($"//*[contains(text(), 'A cozy double room with all amenities.')]");
        }
    }
}
