using System;
using Godot;

namespace PhotonPhighters.Scripts.Events;

public partial class PlayerMovementEvent : Node
{
  public float _gravity;
  public float _speed;
  public Vector2 _velocity;
  public Vector2 _inputDirection;
  public bool _canMove;
  public CharacterBody2D _characterBody;
  public bool _canJump;
  public float _jumpForce;
  public int _maxJumps;

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
    this._gravity = gravity;
    this._speed = speed;
    this._velocity = velocity;
    this._inputDirection = inputDirection;
    this._canMove = canMove;
    this._characterBody = characterBody;
    this._canJump = canJump;
    this._jumpForce = jumpForce;
    this._maxJumps = maxJumps;
  }
}
