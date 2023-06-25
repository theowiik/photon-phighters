using Godot;
using PhotonPhighters.Scripts.GoSharper;
using PhotonPhighters.Scripts.GoSharper.AutoWiring;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;

public partial class HealthBar : Control
{
  [GsAutoWiring("HealthLabel")]
  private Label _healthLabel;

  public override void _Ready()
  {
    this.AutoWire();
  }

  private void SetHealth(int health, int maxHealth)
  {
    _healthLabel.Text = $"{health}/{maxHealth}";
  }
}
