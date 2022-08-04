namespace WeatherCast.Tests
{
    internal static class TestHelper
    {
        internal static async Task WaitAsync(TimeSpan delay)
        {
            await Task.Delay(delay);
        }
    }
}
