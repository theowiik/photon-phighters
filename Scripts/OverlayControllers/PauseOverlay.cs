using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts.OverlayControllers;

public partial class PauseOverlay : Control
{
  [GetNode("Center/VBox/QuitButton")]
  private Button _quitButton;

  [GetNode("Center/VBox/ResumeButton")]
  private Button _resumeButton;

  [GetNode("Center/VBox/RestartButton")]
  private Button _restartButton;

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
      _audioStreamPlayer.StreamPaused = !value;

      if (value)
      {
        if (!_audioStreamPlayer.Playing)
        {
          _audioStreamPlayer.Play();
        }

        GrabFocus();
      }
    }
  }

  public override void _Ready()
  {
    this.AutoWire();
    _resumeButton.Pressed += () => EmitSignal(SignalName.ResumeGame);
    _quitButton.Pressed += () => GetTree().Quit();
    _restartButton.Pressed += () => GetTree().ChangeSceneToFile("res://Scenes/StartSceen.tscn");
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
