using System;
using Godot;

namespace PhotonPhighters.Scripts.Events;

public partial class PlayerMovementEvent : Node
{
  public float Gravity { get; set; }
  public float Speed { get; set; }
  public Vector2 Velocity { get; set; }
  public Vector2 InputDirection { get; set; }
  public bool CanMove { get; set; }
  public CharacterBody2D CharacterBody { get; set; }
  public bool CanJump { get; set; }
  public float JumpForce { get; set; }
  public int MaxJumps { get; set; }

  public PlayerMovementEvent(
    float gravity,
    float speed,
    Vector2 velocity,
    Vector2 inputDirection,
    bool canMove,
    CharacterBody2D characterBody,
    bool canJump,
    float jumpForce,
    int maxJumps
  )
  {
    Gravity = gravity;
    Speed = speed;
    Velocity = velocity;
    InputDirection = inputDirection;
    CanMove = canMove;
    CharacterBody = characterBody;
    CanJump = canJump;
    JumpForce = jumpForce;
    MaxJumps = maxJumps;
  }
}
