using System.Threading.Tasks;
using Microsoft.Playwright;

namespace PlaywrightTests.Pages
{
    public interface ICalendarTablePage
    {
        ILocator Next();
        ILocator CalendarCell(int row, int col);
        ILocator CalendarDate(int row, int col);
        ILocator EventSegment(string content);
        Task SelectDates(string startDay, int numberOfNights);
    }

    public class CalendarTablePage : ICalendarTablePage
    {
        private readonly IPage page;

        public CalendarTablePage(IPage page)
        {
            this.page = page;
        }

        #region Locators
        public ILocator Next() => page.GetByRole(AriaRole.Button, new() { Name = "Next", Exact = true });
        public ILocator CalendarCell(int row, int col = 2) => page.Locator($"div:nth-child({row}) > .rbc-row-bg > div:nth-child({col})");
        public ILocator CalendarDate(int row, int col = 2) => page.Locator($"div:nth-child({row}) > .rbc-row-content > .rbc-row > div.rbc-date-cell:nth-child({col})");
        public Task<int> UnavailableSegment(int row, int col = 2) => page.GetByText($"div:nth-child({row}) *[title^=\"Unavailable\"]").CountAsync();
        public ILocator EventSegment(string content) => page.Locator($"div:text-is(\"{content}\")");
        #endregion

        #region Methods
        public async Task SelectDates(string startDay, int numberOfNights)
        {
            int weekDayStart = GetDayColumn(startDay);
            int weekRow = await GetWeekWithAvailableDay(weekDayStart);

            ILocator startDayDiv = CalendarDate(weekRow, weekDayStart);
            ILocator endDayDiv = CalendarDate(weekRow, weekDayStart + numberOfNights);

            var startBox = await startDayDiv.BoundingBoxAsync();
            var endBox = await endDayDiv.BoundingBoxAsync();

            if (startBox != null && endBox != null)
            {
                await startDayDiv.HoverAsync();
                await page.Mouse.DownAsync();
                await page.Mouse.MoveAsync(startBox.X + 10, startBox.Y + 10);
                await page.Mouse.MoveAsync(endBox.X + 10, endBox.Y + 10);
                await page.Mouse.UpAsync();
            }
            else
            {
                throw new InvalidOperationException("Unable to select the dates due to bounding box issues.");
            }
        }

        private static int GetDayColumn(string day)
        {
            return day.ToLower() switch
            {
                "sunday" => 1,
                "monday" => 2,
                "tuesday" => 3,
                "wednesday" => 4,
                "thursday" => 5,
                "friday" => 6,
                "saturday" => 7,
                _ => throw new ArgumentException("Invalid day name.", nameof(day))
            };
        }

        private async Task<int> GetWeekWithAvailableDay(int weekDayStart)
        {
            int rowValue = 2; // Initial row offset to avoid calendar header

            for (int monthOffset = 0; monthOffset < 12; monthOffset++)
            {


                if (await IsDayAvailable(rowValue, weekDayStart))
                {
                    return rowValue;
                }

                rowValue = 3;

                if (await IsDayAvailable(rowValue, weekDayStart))
                {
                    return rowValue;
                }

                await Next().ClickAsync();
                rowValue = 2;
            }

            throw new InvalidOperationException("No available day found in the upcoming year.");
        }

        private async Task<bool> IsDayAvailable(int row, int col)
        {
            var calendarDateLocator = CalendarDate(row, col);
            var cellClass = await calendarDateLocator.GetAttributeAsync("class");
            if (cellClass == null) return false;

            await page.WaitForTimeoutAsync(300);

            var unavailableSegmentLocator = UnavailableSegment(row, col);
            int isUnavailable = unavailableSegmentLocator.Result;

            return !cellClass.Contains("rbc-off-range") && isUnavailable < 1;
        }
        #endregion
    }
}