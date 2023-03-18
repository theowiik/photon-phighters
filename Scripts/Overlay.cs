using Godot;

public partial class Overlay : Control
{
    private RichTextLabel _scoreLabel;
    private Label _timerLabel;
    private Label _totalScoreLabel;

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

    public string TotalScore
    {
        set
        {
            _totalScoreLabel.Text = value;
        }
    }

    public override void _Ready()
    {
        _scoreLabel = GetNode<RichTextLabel>("ScoreLabel");
        _timerLabel = GetNode<Label>("RoundTimerLabel");
        _totalScoreLabel = GetNode<Label>("TotalScoreLabel");
    }
}
