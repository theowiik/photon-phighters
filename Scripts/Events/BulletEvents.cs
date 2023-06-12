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

public partial class BulletCollideFloorEvent : BulletEvent
{
  public Node2D Floor { get; set; }
  public bool IsFinished { get; set; }

  public BulletCollideFloorEvent(Area2D area2D, Vector2 vector2, Node2D floor, bool isFinished)
    : base(area2D, vector2)
  {
    Floor = floor;
    IsFinished = isFinished;
  }
}
