using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using Microsoft.VisualBasic;
using PlaywrightTests.Resources;

namespace PlaywrightTests.Pages;
public interface IBookingFormPage
{
    public ILocator FirstName();
    public ILocator LastName();
    public ILocator Email();
    public ILocator PhoneNo();
    public ILocator SubmitBooking();
    public ILocator BookingSuccessful();
    public Task FillForm(Client client);
}

public class BookingFormPage : IBookingFormPage
{
    private readonly IPage page;

    public BookingFormPage(IPage page)
    {
        this.page = page;
    }

    #region locators
    public ILocator FirstName() => page.GetByPlaceholder("Firstname");
    public ILocator LastName() => page.GetByPlaceholder("Lastname");
    public ILocator Email() => page.Locator("input[name=\"email\"]");
    public ILocator PhoneNo() => page.Locator("input[name=\"phone\"]");
    public ILocator SubmitBooking() => page.GetByRole(AriaRole.Button, new() { Name = "Book", Exact = true });
    public ILocator BookingSuccessful() => page.GetByRole(AriaRole.Heading, new() { Name = "Booking Successful!" });
    //await P
    #endregion

    #region methods
    public async Task FillForm(Client client)
    {
        await FirstName().ClickAsync();
        await FirstName().FillAsync(client.FirstName);
        await LastName().ClickAsync();
        await LastName().FillAsync(client.LastName);
        await Email().ClickAsync();
        await Email().FillAsync(client.Email);
        await PhoneNo().ClickAsync();
        await PhoneNo().FillAsync(client.PhoneNo);
    }
    #endregion
}