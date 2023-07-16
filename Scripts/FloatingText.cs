using Godot;
using PhotonPhighters.Scripts.GoSharper;
using PhotonPhighters.Scripts.GoSharper.AutoWiring;
using PhotonPhighters.Scripts.GoSharper.Instancing;

namespace PhotonPhighters.Scripts;

[Instantiable("res://Objects/FloatingText.tscn")]
public partial class FloatingText : Node2D
{
  private const float ExistsTime = 3f;
  private const float MovementSpeed = 23f;
  private Timer _timer;

  [GetNode("Label")]
  private Label _label;

  public override void _Ready()
  {
    this.AutoWire();
    _timer = GsTimerFactory.OneShotSelfDestructingStartedTimer(ExistsTime, QueueFree);
    AddChild(_timer);
  }

  public override void _Process(double delta)
  {
    Position += new Vector2(0, -(float)delta * MovementSpeed);
    Modulate = new Color(1, 1, 1, TimeLeftAsPercentage());
  }

  public void SetText(string text)
  {
    _label.Text = text;
  }

  private float TimeLeftAsPercentage()
  {
    return (float)_timer.TimeLeft / ExistsTime;
  }
}
