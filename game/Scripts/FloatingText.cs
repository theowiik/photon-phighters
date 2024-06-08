using Godot;
using GodotSharper;
using GodotSharper.AutoGetNode;
using GodotSharper.Instancing;

namespace PhotonPhighters.Scripts;

[Scene("res://Objects/FloatingText.tscn")]
public partial class FloatingText : Node2D
{
  private const float ExistsTime = 3f;
  private const float MovementSpeed = 23f;

  [GetNode("Label")]
  private Label _label;

  private Timer _timer;

  public override void _Ready()
  {
    this.GetNodes();
    _timer = TimerFactory.StartedSelfDestructingOneShot(ExistsTime, QueueFree);
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
