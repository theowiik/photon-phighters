using Godot;

namespace PhotonPhighters.Scripts.Events;

public partial class BulletEvent : Node
{
  public BulletEvent(Area2D area2D, Vector2 velocity)
  {
    Area2D = area2D;
    Velocity = velocity;
  }

  public Area2D Area2D { get; private set; }
  public Vector2 Velocity { get; set; }
}

public partial class BulletCollideFloorEvent : BulletEvent
{
  public BulletCollideFloorEvent(Area2D area2D, Vector2 vector2, bool isFinished)
    : base(area2D, vector2)
  {
    IsFinished = isFinished;
  }

  public bool IsFinished { get; private set; }
}
