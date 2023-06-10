using System;
using Godot;

namespace PhotonPhighters.Scripts;

public partial class BulletEvents
{
  public partial class BulletEvent : Node
  {
    public Area2D Area2D;
    public Vector2 Velocity;

    public BulletEvent(Area2D Area2D, Vector2 Velocity)
    {
      this.Area2D = Area2D;
      this.Velocity = Velocity;
    }
  }
}
