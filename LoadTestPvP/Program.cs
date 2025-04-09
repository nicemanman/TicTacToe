using System.Collections.Concurrent;
using System.Diagnostics;

namespace LoadTestPvP;

public class Program
{
    public static async Task Main()
    {
        const int playerCount = 10;
        const int pairCount = playerCount / 2;
        var baseUrl = "http://localhost:5000";

        var completed = 0;
        var failed = 0;
        var errors = new ConcurrentBag<(string Pair, string Error)>();
        var sw = Stopwatch.StartNew();

        var tasks = new List<Task>();

        for (int i = 0; i < pairCount; i++)
        {
            var client1 = new HttpClient { BaseAddress = new Uri(baseUrl) };
            var client2 = new HttpClient { BaseAddress = new Uri(baseUrl) };

            var player1 = $"pvp_user_{i}_1";
            var player2 = $"pvp_user_{i}_2";

            var pair = new PvPTestPair(player1, player2, client1, client2);

            var task = pair.RunAsync(
                onSuccess: pairName =>
                {
                    Interlocked.Increment(ref completed);
                    Console.WriteLine($"✅ {pairName} завершили игру");
                },
                onError: (pairName, ex) =>
                {
                    Interlocked.Increment(ref failed);
                    errors.Add((pairName, ex.Message));
                    Console.WriteLine($"{pairName} ошибка: {ex.Message}");
                });

            tasks.Add(task);
        }

        await Task.WhenAll(tasks);
        sw.Stop();

        Console.WriteLine("\n===== PVP ТЕСТ РЕЗУЛЬТАТЫ =====");
        Console.WriteLine($"Всего пар:                 {pairCount}");
        Console.WriteLine($"Завершили без ошибок:   {completed}");
        Console.WriteLine($"С ошибками:             {failed}");
        Console.WriteLine($"Время выполнения:        {sw.Elapsed}");

        if (!errors.IsEmpty)
        {
            Console.WriteLine("\nОшибки:");
            foreach (var err in errors)
                Console.WriteLine($" - [{err.Pair}] {err.Error}");
        }
        else
        {
            Console.WriteLine("🎉 Все пары успешно завершили игру!");
        }
    }
}
