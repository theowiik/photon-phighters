using Godot;

namespace PhotonPhighters.Scripts.Controller;

public interface IController
{
  void Vibrate();
  bool IsShootPressed();
  bool IsJumpPressed();
  Vector2 GetAim();
  Vector2 GetMovement();
}
