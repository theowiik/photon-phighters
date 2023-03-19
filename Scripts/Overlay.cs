using Godot;

public partial class Overlay : Control
{
    private PackedScene _powerUpScene = GD.Load<PackedScene>("res://Objects/UI/PowerUpButton.tscn");
    private HBoxContainer _powerUpDeck;
    private Label _scoreLabel;
    private Label _timerLabel;
    private Label _totalScoreLabel;

    [Signal]
    public delegate void PowerUpSelectedEventHandler();

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
        _scoreLabel = GetNode<Label>("ScoreLabel");
        _timerLabel = GetNode<Label>("RoundTimerLabel");
        _totalScoreLabel = GetNode<Label>("TotalScoreLabel");
        _powerUpDeck = GetNode<HBoxContainer>("PowerUpDeck");
        ClearPowerUpDeck();
    }

    private void ClearPowerUpDeck()
    {

        foreach (var powerUpButton in _powerUpDeck.GetNodes<Button>())
        {
            powerUpButton.QueueFree();
        }

        _powerUpDeck.Visible = false;
    }

    public void StartPowerUpSelection()
    {
        GD.Print("Start powerup selection");
        ClearPowerUpDeck();
        FillPowerUpDeck();
    }

    private void FillPowerUpDeck()
    {
        _powerUpDeck.Visible = true;
        var powerUpCount = 3;

        for (int i = 0; i < powerUpCount; i++)
        {
            GD.Print("Adding powerup button");
            var powerUpButton = _powerUpScene.Instantiate<PowerUpButton>();
            powerUpButton.Pressed += () =>
            {
                var losingPlayer = GameState.Player1Won ? GameState.Player2 : GameState.Player1;
                powerUpButton.ApplyPowerUp(losingPlayer);
                GD.Print("Powerup applied");
                ClearPowerUpDeck();
                EmitSignal(SignalName.PowerUpSelected);
            };

            _powerUpDeck.AddChild(powerUpButton);
        }
    }
}
