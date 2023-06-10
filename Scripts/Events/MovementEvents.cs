using System;
using Godot;

namespace PhotonPhighters.Scripts;

public partial class MovementEvents
{
  public partial class PlayerMovementEvent : Node
  {
    public float Gravity;
    public float Speed;
    public Vector2 Velocity;
    public Vector2 InputDirection;
    public bool CanMove;
    public CharacterBody2D CharacterBody;
    public bool CanJump;
    public float JumpForce;
    public int MaxJumps;

    public PlayerMovementEvent(
      float Gravity,
      float Speed,
      Vector2 Velocity,
      Vector2 InputDirection,
      bool CanMove,
      CharacterBody2D CharacterBody,
      bool CanJump,
      float JumpForce,
      int MaxJumps
    )
    {
      this.Gravity = Gravity;
      this.Speed = Speed;
      this.Velocity = Velocity;
      this.InputDirection = InputDirection;
      this.CanMove = CanMove;
      this.CharacterBody = CharacterBody;
      this.CanJump = CanJump;
      this.JumpForce = JumpForce;
      this.MaxJumps = MaxJumps;
    }
  }
}
