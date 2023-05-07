using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;
public partial class HealthBar : Control
{
    [GetNode("HealthLabel")]
    private Label _healthLabel;

    public override void _Ready() => NodeAutoWire.AutoWire(this);

    public void SetHealth(int health, int maxHealth) => _healthLabel.Text = $"{health}/{maxHealth}";
}
