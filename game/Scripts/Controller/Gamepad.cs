using System;
using Godot;
using Vector2 = Godot.Vector2;

namespace PhotonPhighters.Scripts.Controller;

public sealed class Gamepad : IController
{
  private const float DeadZone = 0.1f;
  private readonly int _gamepadIndex;
  private bool _jumpedLastPoll;

  public Gamepad(int gamepadIndex)
  {
    if (gamepadIndex < 0)
    {
      throw new ArgumentOutOfRangeException(nameof(gamepadIndex), "Gamepad index must be greater than or equal to 0.");
    }

    _gamepadIndex = gamepadIndex;
  }

  /// <summary>
  ///   Returns the movement direction.
  /// </summary>
  public Vector2 GetMovement()
  {
    return GetAxis(JoyAxis.LeftX, JoyAxis.LeftY);
  }

  /// <summary>
  ///   Returns the aim direction.
  /// </summary>
  public Vector2 GetAim()
  {
    return GetAxis(JoyAxis.RightX, JoyAxis.RightY);
  }

  /// <summary>
  ///   Returns true if the jump button is pressed.
  /// </summary>
  public bool IsJumpPressed()
  {
    var isPressing = Input.GetJoyAxis(_gamepadIndex, JoyAxis.TriggerLeft) > DeadZone;

    if (isPressing && _jumpedLastPoll)
    {
      return false;
    }

    _jumpedLastPoll = isPressing;
    return isPressing;
  }

  /// <summary>
  ///   Returns true if the shoot button is pressed.
  /// </summary>
  public bool IsShootPressed()
  {
    return Input.GetJoyAxis(_gamepadIndex, JoyAxis.TriggerRight) > DeadZone;
  }

  /// <summary>
  ///   Vibrates the gamepad.
  /// </summary>
  public void Vibrate()
  {
    Input.StartJoyVibration(_gamepadIndex, 0.666f, 0.666f, 0.5f);
  }

  private Vector2 GetAxis(JoyAxis axis1, JoyAxis axis2)
  {
    var vec = new Vector2(Input.GetJoyAxis(_gamepadIndex, axis1), Input.GetJoyAxis(_gamepadIndex, axis2));

    if (vec.Length() < DeadZone)
    {
      return Vector2.Zero;
    }

    return vec;
  }
}
