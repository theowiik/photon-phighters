using System;
using Godot;
using GodotSharper.AutoGetNode;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;

public partial class Avatar : Node2D
{
  [GetUniqueNode("NameLabel")]
  private Label _nameLabel;

  [GetUniqueNode("Pivot")]
  private Node2D _pivot;

  [GetUniqueNode("Sprite2D")]
  private Sprite2D _sprite;

  private Team _team;
  public string PlayerName => _nameLabel.Text;
  public int DeviceId { get; set; } = -1;

  public Team Team
  {
    get => _team;
    set
    {
      if (value == Team.Neutral)
      {
        throw new ArgumentException("Team cannot be neutral");
      }

      _team = value;
      _sprite.Color(_team);
    }
  }

  public override void _Ready()
  {
    this.GetNodes();
    _nameLabel.Text = NameGenerator.Get();
  }

  public override void _PhysicsProcess(double delta)
  {
    if (_pivot.Position.DistanceTo(Position) <= 0.1f)
    {
      return;
    }

    _pivot.Position = _pivot.Position.Lerp(Vector2.Zero, (float)(3 * delta));
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    if (@event is InputEventJoypadButton button && button.Pressed)
    {
      // Wrong device
      if (button.Device == -1 || button.Device != DeviceId)
      {
        return;
      }

      switch (button.ButtonIndex)
      {
        case JoyButton.LeftShoulder:
          Team = Team.Light;
          break;
        case JoyButton.RightShoulder:
          Team = Team.Dark;
          break;
      }

      _pivot.Position = new Vector2(0, -30);
    }
  }
}
