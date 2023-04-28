using Godot;
using static World;

public partial class Overlay : Control
{
    private PackedScene _powerUpScene = GD.Load<PackedScene>("res://Objects/UI/PowerUpButton.tscn");
    private HBoxContainer _powerUpDeck;
    private Label _timerLabel;
    private Label _totalScoreLabel;
    private TextureProgressBar _roundScoreBar;

    [Signal]
    public delegate void PowerUpSelectedEventHandler();

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

    public Results RoundScore
    {
        set
        {
            var percentageDarkness = (float)value.Dark / (value.Light + value.Dark);
            _roundScoreBar.Value = percentageDarkness;
        }
    }

    public override void _Ready()
    {
        const string rootPath = "VBoxContainer/";
        _timerLabel = GetNode<Label>(rootPath + "RoundTimerLabel");
        _totalScoreLabel = GetNode<Label>(rootPath + "TotalScoreLabel");
        _roundScoreBar = GetNode<TextureProgressBar>(rootPath + "RoundScoreBar");
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
                throw new System.Exception("fix game state");

                Player losingPlayer = null;
                powerUpButton.ApplyPowerUp(losingPlayer);
                GD.Print("Powerup applied");
                ClearPowerUpDeck();
                EmitSignal(SignalName.PowerUpSelected);
            };

            _powerUpDeck.AddChild(powerUpButton);
        }
    }
}
