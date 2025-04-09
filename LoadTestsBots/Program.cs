using System.Collections.Concurrent;
using System.Diagnostics;

namespace LoadTests;

public class Program
{
    public static async Task Main()
    {
        const int userCount = 10;
        var baseAddress = "http://localhost:5000";

        var completed = 0;
        var failed = 0;
        var errors = new ConcurrentBag<(string User, string Error)>();
        var sw = Stopwatch.StartNew();

        var tasks = Enumerable.Range(0, userCount).Select(i =>
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
            var username = $"test_user_{i}";
            var player = new TestPlayer(client, username);

            player.OnCompleted += u =>
            {
                Interlocked.Increment(ref completed);
                Console.WriteLine($"{u} завершил игру");
            };

            player.OnError += (u, ex) =>
            {
                Interlocked.Increment(ref failed);
                errors.Add((u, ex.Message));
                Console.WriteLine($"{u} ошибка: {ex.Message}");
            };

            return player.RunAsync();
        });

        await Task.WhenAll(tasks);
        sw.Stop();

        Console.WriteLine("\n===== РЕЗУЛЬТАТЫ ТЕСТА =====");
        Console.WriteLine($"Всего пользователей:     {userCount}");
        Console.WriteLine($"Завершили без ошибок:  {completed}");
        Console.WriteLine($"С ошибками:            {failed}");
        Console.WriteLine($"Время выполнения:      {sw.Elapsed}");

        if (!errors.IsEmpty)
        {
            Console.WriteLine("\nОшибки:");
            foreach (var err in errors)
            {
                Console.WriteLine($" - [{err.User}] {err.Error}");
            }
        }
        else
        {
            Console.WriteLine("🚀 Все пользователи завершили игру без ошибок!");
        }
    }
}