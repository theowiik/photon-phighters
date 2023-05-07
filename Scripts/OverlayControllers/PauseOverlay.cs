namespace PhotonPhighters.Scripts.OverlayControllers;
using Godot;
using PhotonPhighters.Scripts.Utils;

public partial class PauseOverlay : Control
{
    [Signal]
    public delegate void ResumeGameEventHandler();

    [GetNode("VBox/ResumeButton")]
    private Button _resumeButton;

    [GetNode("VBox/QuitButton")]
    private Button _quitButton;

    public override void _Ready() => this.AutoWire();

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            EmitSignal(SignalName.ResumeGame);
            // TODO: Mark input as handled
        }
    }
}
