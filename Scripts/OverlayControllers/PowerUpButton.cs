using Godot;

// TODO: Remove?
namespace PhotonPhighters.Scripts.OverlayControllers;

public partial class PowerUpButton : Button
{
    public override void _Ready()
    {
        GrabFocus();
    }
}