using Godot;

namespace PhotonPhighters.Scripts.Gamepad;

public interface IGamepad
{
  void Vibrate();
  bool IsShootPressed();
  bool IsJumpPressed();
  Vector2 GetAim();
  Vector2 GetMovement();
}
