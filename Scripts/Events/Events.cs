using System;
using Godot;

namespace PhotonPhighters.Scripts;

public static class Events
{
  public class PlayerMovementEvent : Node
  {
    public float Gravity;
    public int MaxJumps;
    public float Speed;

    public PlayerMovementEvent(float Gravity, int MaxJumps, float Speed)
    {
      this.Gravity = Gravity;
      this.MaxJumps = MaxJumps;
      this.Speed = Speed;
    }

  }

  public class BulletCollideFloorEvent : Node
  {
    public Bullet bullet;
    public Node2D floor;

    public BulletCollideFloorEvent(Bullet bullet, Node2D floor)
    {
      this.bullet = bullet;
      this.floor = floor;
    }
  }

  public class BulletCollidePlayerEvent : Node
  {
    public Bullet bullet;
    public Player player;
    public BulletCollidePlayerEvent(Bullet bullet, Player player)
    {
      this.bullet = bullet;
      this.player = player;
    }
  }

  public class BulletFlyingEvent : Node
  {
    public Bullet bullet;

    public BulletFlyingEvent(Bullet bullet)
    {
      this.bullet = bullet;
    }
  }
}