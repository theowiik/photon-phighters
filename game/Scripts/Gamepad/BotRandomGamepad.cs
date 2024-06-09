using System;
using Godot;

namespace PhotonPhighters.Scripts.Gamepad;

/// <summary>
///   Represents a bot-controlled gamepad with semi-human behavior for aiming, moving, shooting, and jumping.
/// </summary>
public sealed class BotRandomGamepad : AbstractBotGamepad
{
  private const float JumpProbability = 0.05f;
  private const float MoveProbability = 0.5f;
  private readonly Random _random;
  private Vector2 _aim = Vector2.Zero;

  private bool _jumpPressed;
  private Vector2 _movement = Vector2.Zero;

  /// <summary>
  ///   Initializes a new instance of the <see cref="BotRandomGamepad" /> class.
  /// </summary>
  public BotRandomGamepad()
  {
    _random = new Random();
  }

  public override void Vibrate()
  {
    throw new NotImplementedException();
  }

  public override bool IsShootPressed()
  {
    return true;
  }

  public override bool IsJumpPressed()
  {
    Update();
    return _jumpPressed;
  }

  public override Vector2 GetAim()
  {
    Update();
    return _aim.Normalized();
  }

  public override Vector2 GetMovement()
  {
    Update();
    return _movement.Normalized();
  }

  protected override void MakeDecision()
  {
    _jumpPressed = _random.NextDouble() < JumpProbability;

    // Completely switch aim direction randomly
    var aimDir = (float)_random.NextDouble() * 2 * (float)Math.PI;
    _aim = new Vector2((float)Math.Cos(aimDir), (float)Math.Sin(aimDir));

    // Randomly decide whether to move or not
    if (_random.NextDouble() < MoveProbability)
    {
      var dir = (float)_random.NextDouble() * 2 * (float)Math.PI;
      _movement = new Vector2((float)Math.Cos(dir), (float)Math.Sin(dir));
    }
    else
    {
      _movement = Vector2.Zero;
    }
  }
}
