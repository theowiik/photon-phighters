using System.Collections.Generic;
using Godot;
using PhotonPhighters.Scripts.Utils;

public partial class LightPlacingAutomata : Node2D
{
  private readonly IList<RayCast2D> _rayCasts = new List<RayCast2D>();

  [Signal]
  public delegate void PossibleLightPositionFoundEventHandler(Vector2 globalPosition);

  public override void _Ready()
  {
    this.AutoWire();
    PlaceRaysInCircle();
  }

  public override void _Process(double delta)
  {
    CheckRays();
  }

  private void PlaceRaysInCircle()
  {
    const int NRays = 5;
    const int Length = 300;

    for (var i = 0; i < NRays; i++)
    {
      var angle = i * 2 * Mathf.Pi / NRays;
      var ray = new RayCast2D();
      ray.TargetPosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).Normalized() * Length;
      AddChild(ray);
      _rayCasts.Add(ray);
    }
  }

  private void CheckRays()
  {
    foreach (var ray in _rayCasts)
    {
      var hit = ray.GetCollider();

      if (hit is Node2D node2D && node2D.IsInGroup("floors"))
      {
        var intersection = ray.GetCollisionPoint();
        EmitSignal(SignalName.PossibleLightPositionFound, intersection);
      }
    }
  }
}
