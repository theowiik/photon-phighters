using Godot;
using PhotonPhighters.Scripts.Utils;
using static PhotonPhighters.Scripts.World;

namespace PhotonPhighters.Scripts.OverlayControllers;

public partial class Overlay : Control
{
    [GetNode("VBox/Logs")]
    private RichTextLabel _logs;

    [GetNode("VBox/RoundScoreBar")]
    private TextureProgressBar _roundScoreBar;

    [GetNode("VBox/RoundTimerLabel")]
    private Label _timerLabel;

    [GetNode("VBox/TotalScoreLabel")]
    private Label _totalScoreLabel;

    public string Time
    {
        set => _timerLabel.Text = value;
    }

    public string TotalScore
    {
        set => _totalScoreLabel.Text = value;
    }

    public Results RoundScore
    {
        set => _roundScoreBar.Value = (float)value.Dark / (value.Light + value.Dark);
    }

    public override void _Ready()
    {
        this.AutoWire();
    }

    private void Log(string msg)
    {
        _logs.Text += msg + "\n";
    }
}
