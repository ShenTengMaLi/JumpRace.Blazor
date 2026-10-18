namespace JumpRace.Blazor.Models;

public class PlayerRuntimeState
{
    public int Current { get; set; }
    public int PianoIdx { get; set; }
    public bool IsJumping { get; set; }
    public bool IsLeader { get; set; }
}

public class GameRuntimeState
{
    public List<PlayerRuntimeState> Players { get; set; } = new()
    {
        new PlayerRuntimeState(),
        new PlayerRuntimeState()
    };

    public bool IsGameOver { get; set; }
    public int? WinnerIndex { get; set; }
    public int CurrentBgIdx { get; set; }
}
