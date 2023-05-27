using System;
using System.Collections.Generic;
using Godot;
using PhotonPhighters.Scripts.Utils;
using static PhotonPhighters.Scripts.Player;

namespace PhotonPhighters.Scripts.OverlayControllers;

public partial class PowerUpPicker : Control
{
  public delegate void PowerUpPicked(PowerUps.IPowerUp powerUp);

  private readonly PackedScene _powerUpButtonScene = GD.Load<PackedScene>("res://Objects/UI/PowerUpButton.tscn");

  [GetNode("BackgroundRect")]
  private ColorRect _backgroundRect;

  [GetNode("GridContainer")]
  private GridContainer _gridContainer;

  [GetNode("Label")]
  private Label _label;

  public void SetWinningSide(TeamEnum value)
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
    foreach (var powerUp in PowerUpManager.GetUniquePowerUpsWithRarity(4, 0))
    {
      var powerUpButton = _powerUpButtonScene.Instantiate<PowerUpButton>();

      var rarityText = powerUp.Rarity switch
      {
        PowerUps.Rarity.Common => "",
        PowerUps.Rarity.Rare => "(Rare) ",
        PowerUps.Rarity.Legendary => "(LEGENDARY) ",
        _ => throw new KeyNotFoundException("Rarity not supported")
      };

      powerUpButton.Text = rarityText + powerUp.Name;
      powerUpButton.Pressed += () => PowerUpPickedListeners?.Invoke(powerUp);

      // Disable at first
      powerUpButton.Disabled = true;
      AddChild(TimerFactory.OneShotSelfDestructingStartedTimer(2, () => powerUpButton.Disabled = false));

      _gridContainer.AddChild(powerUpButton);
      powerUpButton.GrabFocus();
    }
  }
}
