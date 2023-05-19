using System;
using System.Collections.Generic;
using Godot;
using PhotonPhighters.Scripts.Utils;
using static PhotonPhighters.Scripts.Player;

namespace PhotonPhighters.Scripts.OverlayControllers;

public partial class PowerUpPicker : Control
{
  // Non Godot signal since Godot doesnt support custom types
  public delegate void PowerUpPicked(PowerUpManager.IPowerUp powerUp);

  public const bool DevMode = true;

  private const int AmountPowerUps = 4;

  [GetNode("BackgroundRect")]
  private ColorRect _backgroundRect;

  [GetNode("GridContainer")]
  private GridContainer _gridContainer;

  [GetNode("Label")]
  private Label _label;

  private PackedScene _powerUpButtonScene = GD.Load<PackedScene>("res://Objects/UI/PowerUpButton.tscn");

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

        default:
          throw new ArgumentOutOfRangeException(nameof(value), value, null);
      }
    }
  }

  public event PowerUpPicked PowerUpPickedListeners;

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

  private void Clear()
  {
    foreach (var powerUpButton in _gridContainer.GetNodes<Button>())
    {
      powerUpButton.QueueFree();
    }
  }

  private void Populate()
  {
    IEnumerable<PowerUpManager.IPowerUp> powerUps;

    if (DevMode)
    {
      powerUps = PowerUpManager.GetAllPowerUps();
    }
    else
    {
      powerUps = PowerUpManager.GetUniquePowerUps(AmountPowerUps);
    }


    foreach (var powerUp in powerUps)
    {
      var powerUpButton = _powerUpButtonScene.Instantiate<PowerUpButton>();

      var rarityText = powerUp.Rarity switch
      {
        PowerUpManager.Rarity.Common => "",
        PowerUpManager.Rarity.Rare => "(rare)",
        PowerUpManager.Rarity.Legendary => "(LEGENDARY)",
        _ => throw new KeyNotFoundException("Rarity not supported")
      };

      powerUpButton.Text = rarityText + powerUp.Name;
      powerUpButton.Pressed += () => PowerUpPickedListeners?.Invoke(powerUp);

      // Disable at first
      powerUpButton.Disabled = true;
      AddChild(TimerFactory.OneShotSelfDestructingStartedTimer(2, () =>
        powerUpButton.Disabled = false
      ));

      _gridContainer.AddChild(powerUpButton);
    }
  }
}
