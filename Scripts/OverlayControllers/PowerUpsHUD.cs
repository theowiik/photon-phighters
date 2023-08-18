using System;
using Godot;
using PhotonPhighters.Scripts;
using PhotonPhighters.Scripts.GoSharper.AutoWiring;
using PhotonPhighters.Scripts.PowerUps;

public partial class PowerUpsHUD : Control
{
  [GetNode("DarkLabel")]
  private RichTextLabel _darkLabel;

  [GetNode("LightLabel")]
  private RichTextLabel _lightLabel;

  public override void _Ready()
  {
    this.AutoWire();
  }

  public void Add(IPowerUpApplier powerUp, Player.TeamEnum who)
  {
    switch (who)
    {
      case Player.TeamEnum.Light:
        _lightLabel.Text += $"\n- {powerUp.Name}";
        break;
      case Player.TeamEnum.Dark:
        _darkLabel.Text += $"[right]\n- {powerUp.Name}[/right]";
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof(who), who, null);
    }
  }
}
