using System;
using Godot;

namespace PhotonPhighters.Scripts.Gamepad;

/// <summary>
///   Shoot towards the closest enemy.
/// </summary>
public sealed class BotGreedyGamepad : IGamepad
{
  public void Vibrate()
  {
    throw new NotImplementedException();
  }

  public bool IsShootPressed()
  {
    throw new NotImplementedException();
  }

  public bool IsJumpPressed()
  {
    throw new NotImplementedException();
  }

  public Vector2 GetAim()
  {
    throw new NotImplementedException();
  }

  public Vector2 GetMovement()
  {
    throw new NotImplementedException();
  }
}
