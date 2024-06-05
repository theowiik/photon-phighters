using System;
using Godot;
using GodotSharper.AutoGetNode;
using PhotonPhighters.Scripts.PowerUps;

namespace PhotonPhighters.Scripts.OverlayControllers;

public partial class PowerUpsHUD : Control
{
  [GetNode("DarkLabel")]
  private RichTextLabel _darkLabel;

  [GetNode("LightLabel")]
  private RichTextLabel _lightLabel;

  public override void _Ready()
  {
    this.GetNodes();
  }

  public void Add(IPowerUpApplier powerUp, LightMode who)
  {
    switch (who)
    {
      case LightMode.Light:
        _lightLabel.Text += $"\n- {powerUp.Name}";
        break;
      case LightMode.Dark:
        _darkLabel.Text += $"[right]\n- {powerUp.Name}[/right]";
        break;
      default:
        throw new ArgumentOutOfRangeException(nameof(who), who, null);
    }
  }
}
