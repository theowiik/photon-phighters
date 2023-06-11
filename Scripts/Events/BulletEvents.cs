using System;
using Godot;

namespace PhotonPhighters.Scripts.Events;

public partial class BulletEvent : Node
{
  public Area2D Area2D { get; set; }
  public Vector2 Velocity { get; set; }

  public BulletEvent(Area2D area2D, Vector2 velocity)
  {
    Area2D = area2D;
    Velocity = velocity;
  }
}
