using Microsoft.JSInterop;

namespace JumpRace.Blazor.Services;

public class AudioService
{
    private readonly IJSRuntime _js;

    // Piano notes (same as original)
    private static readonly double[] Notes = { 261.63, 293.66, 329.63, 349.23, 392.00, 440.00, 493.88 };
    private static readonly double[] WinNotes = { 523.25, 659.25, 783.99, 1046.50 };

    public AudioService(IJSRuntime js)
    {
        _js = js;
    }

    public async Task PlayNoteAsync(int index)
    {
        var freq = Notes[index % Notes.Length];
        await _js.InvokeVoidAsync("gameInterop.playNote", freq);
    }

    public async Task PlayWinNotesAsync()
    {
        await _js.InvokeVoidAsync("gameInterop.playWinNotes", WinNotes);
    }

    public async Task ResumeAudioContextAsync()
    {
        await _js.InvokeVoidAsync("gameInterop.resumeAudio");
    }
}
