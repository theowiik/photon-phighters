using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts.OverlayControllers;

public partial class PauseOverlay : Control
{
  [GetNode("Center/VBox/QuitButton")]
  private Button _quitButton;

  [GetNode("Center/VBox/ResumeButton")]
  private Button _resumeButton;

  [GetNode("AudioStreamPlayer")]
  private AudioStreamPlayer _audioStreamPlayer;

  [Signal]
  public delegate void ResumeGameEventHandler();

  public bool Enabled
  {
    get => Visible;
    set
    {
      Visible = value;
      if (value)
      {
        GrabFocus();
        // _audioStreamPlayer.Play();
        _audioStreamPlayer.StreamPaused = false;
      }
      else
      {
        // _audioStreamPlayer.Stop();
        _audioStreamPlayer.StreamPaused = true;
      }
    }
  }

  public override void _Ready()
  {
    this.AutoWire();
    _resumeButton.Pressed += () => EmitSignal(SignalName.ResumeGame);
    _quitButton.Pressed += () => GetTree().Quit();
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    if (@event.IsActionPressed("ui_cancel"))
    {
      EmitSignal(SignalName.ResumeGame);
    }
    // TODO: Mark input as handled
  }
}
console.log("🚀 ~ file: PauseOverlay.cs:56 ~ delegate:", delegate)
