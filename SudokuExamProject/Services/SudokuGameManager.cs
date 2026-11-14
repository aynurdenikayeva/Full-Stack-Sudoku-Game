
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace SudokuExamProject.Services
{
    public class SudokuGameManager
    {
        private readonly IMemoryCache _cache;

        private readonly ConcurrentDictionary<string, SemaphoreSlim>
            _saveLocks = new();

        public SudokuGameManager(IMemoryCache cache)
        {
            _cache = cache;
        }

        public SudokuService GetGame(string sessionId)
        {
            string key = $"SudokuGame_{sessionId}";

            return _cache.GetOrCreate(key, entry =>
            {
                entry.SlidingExpiration =
                    TimeSpan.FromMinutes(60);

                return new SudokuService();
            })!;
        }

        public async Task<bool> SaveResultOnceAsync(
            string sessionId,
            Func<Task> saveAction)
        {
            var semaphore = _saveLocks.GetOrAdd(
                sessionId,
                _ => new SemaphoreSlim(1, 1));

            await semaphore.WaitAsync();

            try
            {
                var game = GetGame(sessionId).Game;

                if (!game.Completed || game.ResultSaved)
                    return false;

                await saveAction();

                game.ResultSaved = true;

                return true;
            }
            finally
            {
                semaphore.Release();
            }
        }
    }
}
