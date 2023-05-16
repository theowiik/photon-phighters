using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts.OverlayControllers;

public partial class PauseOverlay : Control
{
    [Signal]
    public delegate void ResumeGameEventHandler();

    [GetNode("Center/VBox/QuitButton")]
    private Button _quitButton;

    [GetNode("Center/VBox/ResumeButton")]
    private Button _resumeButton;

    public override void _Ready()
    {
        this.AutoWire();
        _resumeButton.Pressed += () => EmitSignal(SignalName.ResumeGame);
        _quitButton.Pressed += () => GetTree().Quit();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            EmitSignal(SignalName.ResumeGame);
        }
        // TODO: Mark input as handled
    }
}
