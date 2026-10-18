namespace JumpRace.Blazor.Models;

public class PlayerConfig
{
    public string Name { get; set; } = "Player";
    public string Color { get; set; } = "#ff00c8";
    public string Img { get; set; } = "img/1.png";
    public string JumpImg { get; set; } = "";
    public int Wins { get; set; }
    public List<string> Gifts { get; set; } = new() { "", "", "" };
    public List<string> GiftTexts { get; set; } = new() { "", "", "" };
}
