using Godot;
using PhotonPhighters.Scripts.GoSharper.AutoWiring;

namespace PhotonPhighters.Scripts;

public partial class HealthBar : Control
{
  [GetNode("HealthLabel")]
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
