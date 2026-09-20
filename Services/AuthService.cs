using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

namespace WebApplication1.Services;

public class AuthService
{
    // Hardcoded account. These credentials are documented in README.md.
    private const string ValidUsername = "admin";
    private const string ValidPassword = "Portfolio@2026";

    private const int MaxAttempts = 5;
    private static readonly TimeSpan LockoutDuration = TimeSpan.FromMinutes(5);

    private static readonly ConcurrentDictionary<string, Attempt> Attempts = new();

    private class Attempt
    {
        public int Count { get; set; }
        public DateTime LockedUntil { get; set; }
    }

    public bool IsLockedOut(string key, out TimeSpan remaining)
    {
        remaining = TimeSpan.Zero;

        if (Attempts.TryGetValue(key, out var attempt) && attempt.LockedUntil > DateTime.UtcNow)
        {
            remaining = attempt.LockedUntil - DateTime.UtcNow;
            return true;
        }

        return false;
    }

    public bool Validate(string username, string password, string key)
    {
        var ok = FixedTimeEquals(username, ValidUsername) && FixedTimeEquals(password, ValidPassword);

        if (ok)
        {
            Attempts.TryRemove(key, out _);
            return true;
        }

        var attempt = Attempts.GetOrAdd(key, _ => new Attempt());
        attempt.Count++;

        if (attempt.Count >= MaxAttempts)
        {
            attempt.LockedUntil = DateTime.UtcNow.Add(LockoutDuration);
            attempt.Count = 0;
        }

        return false;
    }

    // Constant-time comparison so a wrong password cannot be guessed from response timing.
    private static bool FixedTimeEquals(string input, string expected)
    {
        var a = Encoding.UTF8.GetBytes(input ?? "");
        var b = Encoding.UTF8.GetBytes(expected);
        return a.Length == b.Length && CryptographicOperations.FixedTimeEquals(a, b);
    }
}
