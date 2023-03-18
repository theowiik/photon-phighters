using Godot;

public partial class Overlay : Control
{
    private RichTextLabel _scoreLabel;
    private Label _timerLabel;

    public string Score
    {
        set
        {
            _scoreLabel.Text = value;
        }
    }

    public string Time
    {
        set
        {
            _timerLabel.Text = value;
        }
    }

    public override void _Ready()
    {
        _scoreLabel = GetNode<RichTextLabel>("ScoreLabel");
        _timerLabel = GetNode<Label>("RoundTimerLabel");
    }
}
