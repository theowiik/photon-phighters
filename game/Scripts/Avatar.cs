using System.Collections.Generic;
using Godot;
using GodotSharper.AutoGetNode;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;

public partial class Avatar : Node2D
{
  [GetUniqueNode("LightPlayer")]
  private Sprite2D _lightPlayer;

  [GetUniqueNode("DarkPlayer")]
  private Sprite2D _darkPlayer;

  [GetUniqueNode("Pivot")]
  private Node2D _pivot;

  [GetUniqueNode("NameLabel")]
  private Label _nameLabel;

  public int DeviceId { get; set; } = -1;
  private Team _team;
  public Team Team
  {
    get => _team;
    set
    {
      if (value == _team)
      {
        return;
      }

      if (value == Team.Neutral)
      {
        throw new System.ArgumentException("Team cannot be neutral");
      }

      _team = value;
      _lightPlayer.Visible = value == Team.Light;
      _darkPlayer.Visible = value == Team.Dark;
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
