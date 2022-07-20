namespace WeatherCast.Tests
{
    internal static class TestHelper
    {
        internal static Task Wait(TimeSpan delay)
        {
            Task.Run(async () => await Task.Delay(delay));

            return Task.FromResult(0);
        }

        internal static async Task WaitAsync(TimeSpan delay)
        {
            await Task.Run(async () => await Task.Delay(delay));
        }
    }
}
