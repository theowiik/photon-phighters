using System.Diagnostics;
using Godot;

namespace PhotonPhighters.Scripts.Controller.Bots;

/// <summary>
///   Bot that makes decisions every x seconds.
/// </summary>
public abstract class AbstractBot : IController
{
  private const float DecisionIntervalSeconds = 0.5f;
  private readonly Stopwatch _decisionTimer;

  protected AbstractBot()
  {
    _decisionTimer = new Stopwatch();
    _decisionTimer.Start();
  }

  public abstract void Vibrate();
  public abstract bool IsShootPressed();
  public abstract bool IsJumpPressed();
  public abstract Vector2 GetAim();
  public abstract Vector2 GetMovement();

  /// <summary>
  ///   Makes all decisions for the bot's actions, such as shooting, jumping, aiming, and moving.
  ///   Used to update the bot's decisions based on the elapsed time, and not every frame to avoid performance issues.
  /// </summary>
  protected abstract void MakeDecision();

  /// <summary>
  ///   Updates the bot's decisions based on the elapsed time.
  /// </summary>
  protected void Update()
  {
    if (_decisionTimer.Elapsed.TotalSeconds < DecisionIntervalSeconds)
    {
      return;
    }

    MakeDecision();
    _decisionTimer.Restart();
  }
}
