using Godot;

public partial class Overlay : Control
{
    private RichTextLabel _scoreLabel;

    public string Score
    {
        set
        {
            _scoreLabel.Text = value;
        }
    }

    public override void _Ready()
    {
        _scoreLabel = GetNode<RichTextLabel>("ScoreLabel");
    }
}
