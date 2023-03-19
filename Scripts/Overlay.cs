using Godot;
using System.Linq;

public partial class Overlay : Control
{
    private PackedScene _powerUpScene = GD.Load<PackedScene>("res://Objects/UI/PowerUpButton.tscn");
    private HBoxContainer _powerUpDeck;
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
        _powerUpDeck = GetNode<HBoxContainer>("PowerUpDeck");
        ClearPowerUpDeck();
        FillPowerUpDeck();
    }

    private void ClearPowerUpDeck()
    {

        foreach (var powerUpButton in _powerUpDeck.GetNodes<Button>())
        {
            powerUpButton.QueueFree();
        }
    }

    private void FillPowerUpDeck()
    {
        var powerUpCount = 3;

        // TODO: Dont do this. Hackthon level code. Also, this will always apply to the first player
        var player = GetTree().GetNodesInGroup("players").Cast<Player>().First();

        for (int i = 0; i < powerUpCount; i++)
        {
            var powerUpButton = _powerUpScene.Instantiate<PowerUpButton>();
            powerUpButton.Pressed += () =>
            {
                var losingPlayer = GameState.Player1Won ? GameState.Player2 : GameState.Player1;
                powerUpButton.ApplyPowerUp(losingPlayer);
                GD.Print("Powerup applied");
                ClearPowerUpDeck();
            };

            _powerUpDeck.AddChild(powerUpButton);
        }
    }
}
