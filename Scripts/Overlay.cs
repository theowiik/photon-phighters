using Godot;

public partial class Overlay : Control
{
    public string Score
    {
        set
        {
            _scoreLabel.Text = value;
        }
    }

    private RichTextLabel _scoreLabel;

    public override void _Ready()
    {
        _scoreLabel = GetNode<RichTextLabel>("ScoreLabel");
    }

    public override void _Process(double delta)
    {

    }
}
