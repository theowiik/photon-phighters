using Godot;
using PhotonPhighters.Scripts.Utils;
using static PhotonPhighters.Scripts.Player;

namespace PhotonPhighters.Scripts.OverlayControllers;

public partial class PowerUpPicker : Control
{
    // Non Godot signal since Godot doesnt support custom types
    public delegate void PowerUpPicked(PowerUpManager.IPowerUpApplier powerUpApplier);
    public event PowerUpPicked PowerUpPickedListeners;

    [GetNode("GridContainer")]
    private GridContainer _gridContainer;

    [GetNode("BackgroundRect")]
    private ColorRect _backgroundRect;

    [GetNode("Label")]
    private Label _label;

    public TeamEnum WinningSide
    {
        set
        {
            switch (value)
            {
                case TeamEnum.Light:
                    _backgroundRect.Color = new Color(1, 1, 1, 0.5f);
                    _label.Modulate = Colors.Black;
                    _label.Text = "Light team won! Darkness, pick a helping hand";
                    break;
                case TeamEnum.Dark:
                    _backgroundRect.Color = new Color(0, 0, 0, 0.5f);
                    _label.Modulate = Colors.White;
                    _label.Text = "Dark team won! Lightness, pick a helping hand";
                    break;
            }
        }
    }

    private PackedScene _powerUpButtonScene = GD.Load<PackedScene>("res://Objects/UI/PowerUpButton.tscn");
    private const int AmountPowerUps = 4;

    public override void _Ready()
    {
        this.AutoWire();
        Reset();
    }

    public void Reset()
    {
        Clear();
        Populate();
    }

    private void Populate()
    {
        var powerUps = PowerUpManager.GetUniquePowerUps(AmountPowerUps);

        foreach (var powerUp in powerUps)
        {
            var powerUpButton = _powerUpButtonScene.Instantiate<PowerUpButton>();
            powerUpButton.Text = powerUp.Name;
            powerUpButton.Pressed += () =>
            {
                GD.Print("Powerup button pressed");
                PowerUpPickedListeners?.Invoke(powerUp);
            };

            _gridContainer.AddChild(powerUpButton);
        }
    }

    private void Clear()
    {
        foreach (var powerUpButton in _gridContainer.GetNodes<Button>())
        {
            powerUpButton.QueueFree();
        }
    }
}