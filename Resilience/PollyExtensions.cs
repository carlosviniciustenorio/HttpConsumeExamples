using System.Drawing;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Refit;

namespace HttpConsumeExamples.Resilience
{
    public static class PollyExtensions
    {
        public static void AddPollyResilience(this IServiceCollection services)
        {
            services.AddSingleton<AsyncPolicy>(CreateRetryPolicy());
            services.AddSingleton<AsyncPolicy>(CreateCircuitBreakerPolicy());
        }

        private static AsyncRetryPolicy CreateRetryPolicy()
        {
            return Policy.Handle<Exception>().WaitAndRetryAsync(
                sleepDurations: new []{ TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2)},
                onRetry: (_, timespan, count, _) =>
                {
                    var previousBackgroundColor = Console.BackgroundColor;
                    var previousForegroundColor = Console.ForegroundColor;

                    Console.BackgroundColor = ConsoleColor.Yellow;
                    Console.ForegroundColor = ConsoleColor.Black;

                    Console.Out.WriteLineAsync($"Date: {DateTime.Now:HH:mm:ss} | Retry Attemp: {count} | Wait for: {timespan} seconds");

                    Console.BackgroundColor = previousBackgroundColor;
                    Console.ForegroundColor = previousForegroundColor;
                }
                );
        }
        private static AsyncCircuitBreakerPolicy CreateCircuitBreakerPolicy()
        {
            return Policy.Handle<Exception>().CircuitBreakerAsync(
                    exceptionsAllowedBeforeBreaking: 3,
                    durationOfBreak: TimeSpan.FromSeconds(15),
                    onBreak: (exception, timespan) =>
                    {
                        var backgroundColor = Console.BackgroundColor;
                        var foregroundColor = Console.ForegroundColor;

                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine($"Circuit Break is open for time: {timespan}. Exception: {exception.Message}");
                        
                        Console.BackgroundColor = backgroundColor;
                        Console.ForegroundColor = foregroundColor;
                    },
                    onReset: () =>
                    {
                        var backgroundColor = Console.BackgroundColor;
                        var foregroundColor = Console.ForegroundColor;

                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.ForegroundColor = ConsoleColor.Black;
                        
                        Console.WriteLine("Circuit Break is closed");
                        
                        Console.BackgroundColor = backgroundColor;
                        Console.ForegroundColor = foregroundColor;
                    },
                    onHalfOpen: () =>
                    {
                        var backgroundColor = Console.BackgroundColor;
                        var foregroundColor = Console.ForegroundColor;

                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.ForegroundColor = ConsoleColor.Black;
                        
                        Console.WriteLine("Circuit Break is trying to do some requests");
                        
                        Console.BackgroundColor = backgroundColor;
                        Console.ForegroundColor = foregroundColor;
                    }
                );
        }
        
    }
}