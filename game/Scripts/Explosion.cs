using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using GodotSharper;
using GodotSharper.AutoGetNode;
using GodotSharper.Instancing;
using PhotonPhighters.Scripts.GSAlpha;

namespace PhotonPhighters.Scripts;

[Scene("res://Objects/Explosion.tscn")]
public partial class Explosion : Node2D
{
  public enum ExplosionSize
  {
    Medium = 100,
    Large = 150
  }

  [GetNode("Area2D")]
  private Area2D _area;

  [GetNode("CPUParticles2D")]
  private CpuParticles2D _explosionParticles;

  [GetNode("ExplosionPlayer")]
  private AudioStreamPlayer2D _explosionPlayer;

  private bool _hasExploded;

  public Team Team { get; set; }
  public ExplosionSize Size { get; set; } = ExplosionSize.Medium;

  public override void _Ready()
  {
    this.GetNodes();

    var shape = _area.GetNodeOrExplode<CollisionShape2D>("CollisionShape2D");
    shape.Shape = new CircleShape2D { Radius = (int)Size };
  }

  public override void _PhysicsProcess(double delta)
  {
    if (_hasExploded)
    {
      return;
    }

    _hasExploded = true;
    Explode();
  }

  private async Task Explode()
  {
    await ToSignal(GetTree(), "physics_frame");
    _explosionParticles.Emitting = true;
    _explosionPlayer.Play();
    ColorLightsInsideRadius();
    AddChild(TimerFactory.StartedSelfDestructingOneShot(_explosionParticles.Lifetime, QueueFree));
  }

  private void ColorLightsInsideRadius()
  {
    foreach (var light in GetAllLightsInsideArea())
    {
      light.SetLight(Team);
    }
  }

  private IEnumerable<Light> GetAllLightsInsideArea()
  {
    return _area.GetOverlappingAreas().OfType<Light>();
  }
}
