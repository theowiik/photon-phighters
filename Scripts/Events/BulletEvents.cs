using System;
using Godot;

namespace PhotonPhighters.Scripts.Events;

public partial class BulletEvent : Node
{
  public Area2D _area2D;
  public Vector2 _velocity;

  public BulletEvent(Area2D area2D, Vector2 velocity)
  {
    this._area2D = area2D;
    this._velocity = velocity;
  }
}
