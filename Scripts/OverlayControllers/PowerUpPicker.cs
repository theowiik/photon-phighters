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
      Texture2D btnTexture;
      Texture2D btnTextureHover;
      Texture2D btnTextureDisabled;

      if (powerUp.Rarity == PowerUps.Rarity.Common)
      {
        rarityText = "";
        btnTexture = GD.Load<Texture2D>("res://Assets/Sprites/Buttons/Green/card_green.png");
        btnTextureHover = GD.Load<Texture2D>("res://Assets/Sprites/Buttons/Green/card_green_hover.png");
        btnTextureDisabled = GD.Load<Texture2D>("res://Assets/Sprites/Buttons/Green/card_green_disabled.png");
      }
      else if (powerUp.Rarity == PowerUps.Rarity.Rare)
      {
        rarityText = "(Rare) ";
        btnTexture = GD.Load<Texture2D>("res://Assets/Sprites/Buttons/Blue/card_blue.png");
        btnTextureHover = GD.Load<Texture2D>("res://Assets/Sprites/Buttons/Blue/card_blue_hover.png");
        btnTextureDisabled = GD.Load<Texture2D>("res://Assets/Sprites/Buttons/Blue/card_blue_disabled.png");
      }
      else if (powerUp.Rarity == PowerUps.Rarity.Epic)
      {
        rarityText = "(Epic) ";
        btnTexture = GD.Load<Texture2D>("res://Assets/Sprites/Buttons/Purple/card_purple.png");
        btnTextureHover = GD.Load<Texture2D>("res://Assets/Sprites/Buttons/Purple/card_purple_hover.png");
        btnTextureDisabled = GD.Load<Texture2D>("res://Assets/Sprites/Buttons/Purple/card_purple_disabled.png");
      }
      else if (powerUp.Rarity == PowerUps.Rarity.Legendary)
      {
        rarityText = "(LEGENDARY) ";
        btnTexture = GD.Load<Texture2D>("res://Assets/Sprites/Buttons/Orange/card_orange.png");
        btnTextureHover = GD.Load<Texture2D>("res://Assets/Sprites/Buttons/Orange/card_orange_hover.png");
        btnTextureDisabled = GD.Load<Texture2D>("res://Assets/Sprites/Buttons/Orange/card_orange_disabled.png");
      }
      else
      {
        throw new KeyNotFoundException("Rarity not supported");
      }

      powerUpButton.SetLabel(rarityText + powerUp.Name);
      powerUpButton.TextureNormal = btnTexture;
      powerUpButton.TextureHover = btnTextureHover;

      // This ensures that the menu gives feedback to the player when using a controller
      if (Input.GetConnectedJoypads().Count == 1)
      {
        powerUpButton.TextureFocused = btnTextureHover;
      }

      powerUpButton.TextureDisabled = btnTextureDisabled;
      powerUpButton.Pressed += () => PowerUpPickedListeners?.Invoke(powerUp);

      // Disable at first
      powerUpButton.Disabled = true;
      AddChild(TimerFactory.OneShotSelfDestructingStartedTimer(2, () => powerUpButton.Disabled = false));

      powerUpButton.GrabFocus();
    }
  }
}
