using System;
using Godot;

namespace PhotonPhighters.Scripts;

public partial class Events
{
  public partial class PlayerMoveEvent : Node
  {
    public float Gravity;
    public float Speed;
    public Vector2 Velocity;
    public bool CanJump;
    public float JumpForce;
    public int MaxJumps;

    public PlayerMoveEvent(float Gravity, float Speed, Vector2 Velocity, bool CanJump, float JumpForce, int MaxJumps)
    {
      this.Gravity = Gravity;
      this.Speed = Speed;
      this.Velocity = Velocity;
      this.CanJump = CanJump;
      this.JumpForce = JumpForce;
      this.MaxJumps = MaxJumps;
    }
  }

  public partial class BulletEvent : Node
  {
    public Vector2 Velocity;
    public int Damage;
    public float Speed;

    public BulletEvent(Vector2 Velocity, int Damage, float Speed)
    {
      this.Velocity = Velocity;
      this.Damage = Damage;
      this.Speed = Speed;
    }
  }

  public partial class BulletCollideFloorEvent : BulletEvent
  {
    public Node2D floor;

    public BulletCollideFloorEvent(Vector2 Velocity, int Damage, float Speed, Node2D floor) : base(Velocity, Damage, Speed)
    {
      this.floor = floor;
    }
  }

  public partial class BulletCollidePlayerEvent : BulletEvent
  {
    public Player player;

    public BulletCollidePlayerEvent(Vector2 Velocity, int Damage, float Speed, Player player) : base(Velocity, Damage, Speed)
    {
      this.player = player;
    }
  }
}
