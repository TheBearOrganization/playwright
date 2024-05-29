using Microsoft.Playwright;
using NUnit.Framework;
using PlaywrightTests.Pages;
using PlaywrightTests.Resources;
using static Microsoft.Playwright.Assertions;

namespace PlaywrightTests;

[TestFixture]
public class CCBT_ReserveARoom
{
    private IPlaywright playwright;
    private IBrowser browser;
    private IPage page;

    private CalendarTablePage calendarTable;
    private MainPage mainPage;
    private BookingFormPage bookingFormPage;

    [SetUp]
    public async Task Setup()
    {
        playwright = await Playwright.CreateAsync();
        browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true });
        page = await browser.NewPageAsync();

        calendarTable = new CalendarTablePage(page);
        mainPage = new MainPage(page);
        bookingFormPage = new BookingFormPage(page);
    }

    [TearDown]
    public async Task Teardown()
    {
        await page.CloseAsync();
        await browser.CloseAsync();
        playwright.Dispose();
    }

    [Test]
    public async Task ReserveARoom()
    {
        await page.GotoAsync("https://automationintesting.online");


        await mainPage.LetMeHack().ClickAsync();
        await mainPage.BookRoomByIndex(1).ClickAsync();

        await calendarTable.SelectDates("Monday", numberOfNights: 3);

        await Expect(calendarTable.EventSegment("3 night(s) - £300")).ToHaveTextAsync("3 night(s) - £300");

        await bookingFormPage.FillForm(new Client());
        await bookingFormPage.SubmitBooking().ClickAsync();
        await Expect(bookingFormPage.BookingSuccessful()).ToBeVisibleAsync();
    }
}
