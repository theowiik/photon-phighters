using System;
using Godot;
using Vector2 = Godot.Vector2;

namespace PhotonPhighters.Scripts;

public class GamepadWrapper
{
  private const float DeadZone = 0.1f;
  private readonly int _gamepadIndex;

  public GamepadWrapper(int gamepadIndex)
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

  private Vector2 GetAxis(JoyAxis axis1, JoyAxis axis2)
  {
    var vec = new Vector2(Input.GetJoyAxis(_gamepadIndex, axis1), Input.GetJoyAxis(_gamepadIndex, axis2));

    if (vec.Length() < DeadZone)
    {
      return Vector2.Zero;
    }

    return vec;
  }

  /// <summary>
  ///   Returns true if the jump button is pressed.
  /// </summary>
  public bool IsJumpPressed()
  {
    return Input.GetJoyAxis(_gamepadIndex, JoyAxis.TriggerLeft) > DeadZone;
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
}
