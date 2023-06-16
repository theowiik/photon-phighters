using System;
using System.Collections.Generic;
using Godot;
using PhotonPhighters.Scripts.Utils;
using static PhotonPhighters.Scripts.Player;

namespace PhotonPhighters.Scripts.OverlayControllers;

public partial class PowerUpPicker : Control
{
  public delegate void PowerUpPicked(PowerUps.IPowerUpApplier powerUpApplier);

  private readonly PackedScene _powerUpButtonScene = GD.Load<PackedScene>("res://UI/PowerUpTextureButton.tscn");

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
  }

  public void Reset()
  {
    Clear();
    Populate();
  }

  private void Clear()
  {
    foreach (var powerUpButton in _gridContainer.GetNodesOfType<TextureButton>())
    {
      powerUpButton.QueueFree();
    }
  }

  private void Populate()
  {
    foreach (var powerUp in PowerUpManager.GetUniquePowerUpsWithRarity(4, 0))
    {
      var powerUpButton = _powerUpButtonScene.Instantiate<PowerUpTextureButton>();
      _gridContainer.AddChild(powerUpButton);

      string rarityText;
      Texture2D rarityTexture;

      if (powerUp.Rarity == PowerUps.Rarity.Common)
      {
        rarityText = "";
        rarityTexture = GD.Load<Texture2D>("res://Assets/Sprites/card_green.png");
      }
      else if (powerUp.Rarity == PowerUps.Rarity.Rare)
      {
        rarityText = "(Rare) ";
        rarityTexture = GD.Load<Texture2D>("res://Assets/Sprites/card_blue.png");
      }
      else if (powerUp.Rarity == PowerUps.Rarity.Legendary)
      {
        rarityText = "(LEGENDARY) ";
        rarityTexture = GD.Load<Texture2D>("res://Assets/Sprites/card_orange.png");
      }
      else
      {
        throw new KeyNotFoundException("Rarity not supported");
      }

      powerUpButton.SetLabel(rarityText + powerUp.Name);
      powerUpButton.TextureNormal = rarityTexture;
      powerUpButton.Pressed += () => PowerUpPickedListeners?.Invoke(powerUp);

      // Disable at first
      powerUpButton.Disabled = true;
      AddChild(TimerFactory.OneShotSelfDestructingStartedTimer(2, () => powerUpButton.Disabled = false));

      powerUpButton.GrabFocus();
    }
  }
}
