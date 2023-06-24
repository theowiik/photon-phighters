using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts.OverlayControllers;

public partial class PauseOverlay : Control
{
  public delegate void PowerUpPicked(PowerUps.IPowerUpApplier powerUpApplier);

  [Signal]
  public delegate void ResumeGameEventHandler();

  [GetNode("AudioStreamPlayer")]
  private AudioStreamPlayer _audioStreamPlayer;

  [GetNode("Center/VBox/PowerUpButton")]
  private Button _powerUpButton;

  [GetNode("PowerUpsContainer/VBoxContainer")]
  private Control _powerUpsContainer;

  [GetNode("Center/VBox/QuitButton")]
  private Button _quitButton;

  [GetNode("Center/VBox/RestartButton")]
  private Button _restartButton;

  [GetNode("Center/VBox/ResumeButton")]
  private Button _resumeButton;

  public bool Enabled
  {
    get => Visible;
    set
    {
      Visible = value;
      _audioStreamPlayer.StreamPaused = !value;

      if (!value)
      {
        return;
      }

      if (!_audioStreamPlayer.Playing)
      {
        _audioStreamPlayer.Play();
      }

      GrabFocus();
    }
  }

  public event PowerUpPicked PowerUpPickedListeners;

  public override void _Ready()
  {
    this.AutoWire();
    _resumeButton.Pressed += () => EmitSignal(SignalName.ResumeGame);
    _quitButton.Pressed += () => GetTree().Quit();
    _restartButton.Pressed += () => GetTree().ChangeSceneToFile("res://Scenes/Scenes/StartScreen.tscn");
    _powerUpButton.Pressed += OnPowerUpButtonPressed;
    _powerUpsContainer.Visible = false;
    _powerUpButton.Visible = false;

    PopulatePowerUps();
  }

  public override void _UnhandledKeyInput(InputEvent @event)
  {
    if (@event.IsActionPressed("dev_power_up_menu"))
    {
      _powerUpButton.Visible = true;
    }
  }

  private void PopulatePowerUps()
  {
    foreach (var powerUp in PowerUpManager.PowerUps)
    {
      var button = new Button { Text = powerUp.GetType().Name };

      button.Pressed += () => PowerUpPickedListeners?.Invoke(powerUp);
      _powerUpsContainer.AddChild(button);
    }
  }

  private void OnPowerUpButtonPressed()
  {
    _powerUpsContainer.Visible = !_powerUpsContainer.Visible;
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
