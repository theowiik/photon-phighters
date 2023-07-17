using System;
using Godot;
using PhotonPhighters.Scripts;
using PhotonPhighters.Scripts.GoSharper.AutoWiring;
using PhotonPhighters.Scripts.PowerUps;

public partial class PowerUpsHUD : Control
{
  [GetNode("DarkLabel")]
  private Label _darkLabel;

  [GetNode("LightLabel")]
  private Label _lightLabel;

  public override void _Ready()
  {
    this.AutoWire();
    _darkLabel.Text = "Darkness";
    _lightLabel.Text = "Lightness";
  }

  public void Add(IPowerUpApplier powerUp, Player.TeamEnum who)
  {
    switch (who)
    {
      case Player.TeamEnum.Light:
        _lightLabel.Text += $"\n{powerUp.Name}";
        break;
      case Player.TeamEnum.Dark:
        _darkLabel.Text += $"\n{powerUp.Name}";
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof(who), who, null);
    }
  }
}
