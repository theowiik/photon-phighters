using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using PhotonPhighters.Scripts.GoSharper;
using PhotonPhighters.Scripts.GoSharper.AutoWiring;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;

public partial class Explosion : Node2D
{
  public enum ExplosionRadiusEnum
  {
    Medium = 100,
    Large = 150
  }

  [GsAutoWiring("Area2D")]
  private Area2D _area;

  [GsAutoWiring("CPUParticles2D")]
  private CpuParticles2D _explosionParticles;

  [GsAutoWiring("ExplosionPlayer")]
  private AudioStreamPlayer2D _explosionPlayer;

  private bool _hasExploded;

  public Light.LightMode LightMode { get; set; }
  public ExplosionRadiusEnum Radius { get; set; } = ExplosionRadiusEnum.Medium;

  public override void _Ready()
  {
    this.AutoWire();

    var shape = _area.GetNodeOrExplode<CollisionShape2D>("CollisionShape2D");
    shape.Shape = new CircleShape2D { Radius = (int)Radius };
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
    AddChild(GsTimerFactory.OneShotStartedTimer(_explosionParticles.Lifetime, QueueFree));
  }

  private void ColorLightsInsideRadius()
  {
    foreach (var light in GetAllLightsInsideArea())
    {
      light.SetLight(LightMode);
    }
  }

  private IEnumerable<Light> GetAllLightsInsideArea()
  {
    return _area.GetOverlappingAreas().OfType<Light>();
  }
}
