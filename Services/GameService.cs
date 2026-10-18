using JumpRace.Blazor.Models;

namespace JumpRace.Blazor.Services;

public class GameService
{
    private readonly ConfigService _configService;
    private readonly AudioService _audioService;

    public GameConfig Config { get; private set; } = new();
    public GameRuntimeState State { get; private set; } = new();

    public event Action? OnStateChanged;

    // Anti-spam cooldown (ms)
    private readonly Dictionary<int, DateTime> _lastJumpTime = new();
    private const int JumpCooldownMs = 120;

    public GameService(ConfigService configService, AudioService audioService)
    {
        _configService = configService;
        _audioService = audioService;
    }

    public async Task InitializeAsync()
    {
        Config = await _configService.LoadAsync();
        ResetRuntimeState();
        Notify();
    }

    public async Task JumpAsync(int playerIndex)
    {
        if (State.IsGameOver) return;
        if (playerIndex < 0 || playerIndex >= State.Players.Count) return;

        // Simple anti-spam
        if (_lastJumpTime.TryGetValue(playerIndex, out var last) &&
            (DateTime.UtcNow - last).TotalMilliseconds < JumpCooldownMs)
            return;

        _lastJumpTime[playerIndex] = DateTime.UtcNow;

        var runtime = State.Players[playerIndex];
        if (runtime.Current >= Config.Goal) return;

        runtime.Current++;
        runtime.IsJumping = true;

        // Play note
        await _audioService.PlayNoteAsync(runtime.PianoIdx);
        runtime.PianoIdx = (runtime.PianoIdx + 1) % 7;

        UpdateLeaders();
        Notify();

        // Reset jumping flag after animation
        _ = Task.Delay(300).ContinueWith(_ =>
        {
            runtime.IsJumping = false;
            Notify();
        });

        if (runtime.Current >= Config.Goal)
        {
            await HandleWinAsync(playerIndex);
        }
    }

    private async Task HandleWinAsync(int playerIndex)
    {
        State.IsGameOver = true;
        State.WinnerIndex = playerIndex;
        Config.Players[playerIndex].Wins++;

        await _audioService.PlayWinNotesAsync();
        await _configService.SaveAsync(Config);

        // Randomize environment a bit
        RandomizeEnvironment();

        Notify();

        // Auto reset after 2 seconds
        await Task.Delay(2000);
        ResetRuntimeState();
        State.IsGameOver = false;
        State.WinnerIndex = null;
        Notify();
    }

    public void ResetRuntimeState()
    {
        State.Players = Config.Players.Select(_ => new PlayerRuntimeState()).ToList();
        State.IsGameOver = false;
        State.WinnerIndex = null;
        _lastJumpTime.Clear();
    }

    public async Task SaveConfigAsync()
    {
        await _configService.SaveAsync(Config);
    }

    public async Task ResetScoresAsync()
    {
        foreach (var p in Config.Players)
            p.Wins = 0;
        await _configService.SaveAsync(Config);
        Notify();
    }

    public async Task ResetToDefaultAsync()
    {
        await _configService.ClearAsync();
        Config = ConfigService.CreateDefault();
        ResetRuntimeState();
        Notify();
    }

    private void UpdateLeaders()
    {
        var maxStep = State.Players.Max(p => p.Current);
        for (int i = 0; i < State.Players.Count; i++)
        {
            State.Players[i].IsLeader = State.Players[i].Current > 0 && State.Players[i].Current == maxStep;
        }
    }

    private void RandomizeEnvironment()
    {
        var shapes = new[] { "shape-square", "shape-circle" };
        Config.TileShape = shapes[Random.Shared.Next(shapes.Length)];
    }

    private void Notify() => OnStateChanged?.Invoke();
}
