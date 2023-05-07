using Godot;

namespace PhotonPhighters.Scripts.MenuControllers;

public partial class PauseOverlay : Control
{
    [Signal]
    public delegate void ResumeGameEventHandler();

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            EmitSignal(SignalName.ResumeGame);
            // TODO: Mark input as handled
        }
    }
}