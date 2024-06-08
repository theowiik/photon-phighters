using Godot;
using GodotSharper.AutoGetNode;
using GodotSharper.Instancing;

namespace PhotonPhighters.Scripts;

[Scene("res://Objects/Light.tscn")]
public partial class Light : Area2D
{
  private readonly Color _darkColorModulate = new(0, 0, 0, 0.5f);

  private readonly Color _lightColorModulate = new(1, 1, 1, 0.5f);

  [GetNode("AnimationPlayer")]
  private AnimationPlayer _animationPlayer;

  [GetNode("LightSprite")]
  private Sprite2D _lightSprite;

  public Team Team { get; private set; }

  public override void _Ready()
  {
    this.GetNodes();
    _lightSprite.Visible = false;
    Team = Team.Neutral;
  }

  public void SetLight(Team team)
  {
    if (Team == team)
    {
      return;
    }

    if (team == Team.Neutral)
    {
      _lightSprite.Visible = false;
      Team = Team.Neutral;
      return;
    }

    Team = team;
    _lightSprite.Visible = true;
    _lightSprite.Modulate = Team == Team.Light ? _lightColorModulate : _darkColorModulate;
  }
}
