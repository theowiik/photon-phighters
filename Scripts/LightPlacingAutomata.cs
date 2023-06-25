using System.Collections.Generic;
using Godot;
using PhotonPhighters.Scripts.GoSharper.AutoWiring;

namespace PhotonPhighters.Scripts;

public partial class LightPlacingAutomata : Node2D
{
  [Signal]
  public delegate void PossibleLightPositionFoundEventHandler(Vector2 globalPosition);

  private readonly IList<RayCast2D> _rayCasts = new List<RayCast2D>();
  private bool _enabled;

  public bool Enabled
  {
    get => _enabled;
    set
    {
      _enabled = value;
      foreach (var ray in _rayCasts)
      {
        ray.Enabled = _enabled;
      }
    }
  }

  public override void _Ready()
  {
    this.AutoWire();
    PlaceRaysInCircle();
    Enabled = true;
  }

  public override void _Process(double delta)
  {
    if (Enabled)
    {
      CheckRays();
    }
  }

  private void PlaceRaysInCircle()
  {
    const int NRays = 64;
    const int Length = 300;

    for (var i = 0; i < NRays; i++)
    {
      var angle = i * 2 * Mathf.Pi / NRays;
      var ray = new RayCast2D
      {
        TargetPosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).Normalized() * Length
      };
      AddChild(ray);
      _rayCasts.Add(ray);
    }
  }

  private void CheckRays()
  {
    foreach (var ray in _rayCasts)
    {
      var hit = ray.GetCollider();

      if (hit is not Node2D node2D || !node2D.IsInGroup("floors"))
      {
        continue;
      }

      var intersection = ray.GetCollisionPoint();
      EmitSignal(SignalName.PossibleLightPositionFound, intersection);
    }
  }
}
