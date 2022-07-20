namespace WeatherCast.Tests
{
    internal static class TestHelper
    {
        internal static async Task WaitAsync(TimeSpan delay)
        {
            await Task.Run(async () => await Task.Delay(delay));
        }
    }
}
