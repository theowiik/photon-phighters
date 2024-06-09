using System;
using System.Diagnostics;
using Godot;

namespace PhotonPhighters.Scripts.Gamepad;

/// <summary>
///   Represents a bot-controlled gamepad with semi-human behavior for aiming, moving, shooting, and jumping.
/// </summary>
public sealed class BotRandomGamepad : IGamepad
{
  private const float JumpProbability = 0.05f;
  private const float MoveProbability = 0.5f;
  private const float DecisionIntervalSeconds = 0.5f;
  private readonly Stopwatch _decisionTimer;
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
    _decisionTimer = new Stopwatch();
    _decisionTimer.Start();
  }

  public void Vibrate() { }

  public bool IsShootPressed()
  {
    return true;
  }

  public bool IsJumpPressed()
  {
    Update();
    return _jumpPressed;
  }

  public Vector2 GetAim()
  {
    Update();
    return _aim.Normalized();
  }

  public Vector2 GetMovement()
  {
    Update();
    return _movement.Normalized();
  }

  /// <summary>
  ///   Updates the bot's decisions based on the elapsed time.
  /// </summary>
  private void Update()
  {
    if (_decisionTimer.Elapsed.TotalSeconds < DecisionIntervalSeconds)
    {
      return;
    }

    MakeDecision();
    _decisionTimer.Restart();
  }

  /// <summary>
  ///   Makes a decision for the bot's actions, such as shooting, jumping, aiming, and moving.
  /// </summary>
  private void MakeDecision()
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
