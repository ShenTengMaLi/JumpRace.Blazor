namespace JumpRace.Blazor.Models;

public class GameConfig
{
    public int Goal { get; set; } = 200;
    public double TileOpacity { get; set; } = 0.9;
    public string SceneEffect { get; set; } = "stars";
    public string TileShape { get; set; } = "shape-square";
    public List<string> Backgrounds { get; set; } = new()
    {
        "img/bg1.mp4",
        "img/bg2.mp4",
        "img/bg3.mp4"
    };
    public List<string> BgTypes { get; set; } = new() { "video", "video", "video" };
    public List<PlayerConfig> Players { get; set; } = new()
    {
        new PlayerConfig
        {
            Name = "Girls",
            Color = "#ff00c8",
            Img = "img/1.png",
            JumpImg = "img/gs1.png"
        },
        new PlayerConfig
        {
            Name = "Boys",
            Color = "#007bff",
            Img = "img/2.png",
            JumpImg = "img/bs1.png"
        }
    };
}
