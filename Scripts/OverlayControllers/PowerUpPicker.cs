using System;
using System.Collections.Generic;
using System.Globalization;
using Godot;
using PhotonPhighters.Scripts.GoSharper;
using PhotonPhighters.Scripts.GoSharper.AutoWiring;
using PhotonPhighters.Scripts.GoSharper.Instancing;
using static PhotonPhighters.Scripts.Player;

namespace PhotonPhighters.Scripts.OverlayControllers;

public partial class PowerUpPicker : Control
{
  public delegate void PowerUpPicked(PowerUps.IPowerUpApplier powerUpApplier);

  private static readonly Dictionary<PowerUps.Rarity, (string color, string text)> s_rarityThemes =
    new()
    {
      { PowerUps.Rarity.Common, ("Green", "") },
      { PowerUps.Rarity.Rare, ("Blue", "(Rare) ") },
      { PowerUps.Rarity.Epic, ("Purple", "(Epic) ") },
      { PowerUps.Rarity.Legendary, ("Orange", "(LEGENDARY) ") }
    };

  [GetNode("BackgroundRect")]
  private ColorRect _backgroundRect;

  [GetNode("GridContainer")]
  private GridContainer _gridContainer;

  [GetNode("Label")]
  private Label _label;

  [GetNode("RerollTextureButton")]
  private TextureButton _rerollButton;

  private Player _loser;

  private Player _winner;

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
    _rerollButton.Pressed += () => HandleReroll();
    PowerUpPickedListeners += EndPowerUpSelection;
  }

  public void Reset()
  {
    Clear();
    Populate();
  }

  public void BeginPowerUpSelection(Player winner, Player loser)
  {
    Visible = true;
    _winner = winner;
    _loser = loser;
    SetWinningSide(winner.Team);
    GrabFocus();
    Clear();
    Populate();
    loser.VibrateGamepadWeak(0.25f);
    winner.DisableInput();
    _rerollButton.Disabled = false;
  }

  public void EndPowerUpSelection(PowerUps.IPowerUpApplier powerUpApplier)
  {
    Visible = false;
    powerUpApplier.Apply(_loser, _winner);
    _winner.EnableInput();
  }

  public void HandleReroll()
  {
    Clear();
    Populate();
    _loser.MaxHealth -= 10;
    _rerollButton.Disabled = true;
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
    foreach (var powerUp in PowerUpManager.GetUniquePowerUps(4))
    {
      var powerUpButton = GsInstanter.Instantiate<PowerUpTextureButton>();
      _gridContainer.AddChild(powerUpButton);
      var texturePack = GetThemeTextures(powerUp);

      powerUpButton.SetLabel(texturePack.RarityText + powerUp.Name);
      powerUpButton.TextureNormal = texturePack.BtnTexture;
      powerUpButton.TextureHover = texturePack.BtnTextureHover;
      powerUpButton.TextureDisabled = texturePack.BtnTextureDisabled;
      powerUpButton.Pressed += () => PowerUpPickedListeners?.Invoke(powerUp);

      // Disable at first
      powerUpButton.Disabled = true;
      AddChild(
        GsTimerFactory.OneShotSelfDestructingStartedTimer(
          2,
          () =>
          {
            if (Input.GetConnectedJoypads().Count > 0)
            {
            // This ensures that the menu gives feedback to the player when using a controller
              powerUpButton.TextureFocused = texturePack.BtnTextureHover;
            }

            powerUpButton.Disabled = false;
          }
        )
      );
    }
  }

  private static TexturePack GetThemeTextures(PowerUps.IPowerUpApplier powerUp)
  {
    if (!s_rarityThemes.TryGetValue(powerUp.Rarity, out var theme))
    {
      throw new KeyNotFoundException("Rarity not supported");
    }

    var (color, rarityText) = theme;

    var btnTexture = Gs.LoadOrExplode<Texture2D>(
      $"res://Assets/Sprites/Buttons/{color}/card_{color.ToLower(CultureInfo.InvariantCulture)}.png"
    );
    var btnTextureHover = Gs.LoadOrExplode<Texture2D>(
      $"res://Assets/Sprites/Buttons/{color}/card_{color.ToLower(CultureInfo.InvariantCulture)}_hover.png"
    );
    var btnTextureDisabled = Gs.LoadOrExplode<Texture2D>(
      $"res://Assets/Sprites/Buttons/{color}/card_{color.ToLower(CultureInfo.InvariantCulture)}_disabled.png"
    );

    return new TexturePack(rarityText, btnTexture, btnTextureHover, btnTextureDisabled);
  }

  private struct TexturePack : IEquatable<TexturePack>
  {
    public string RarityText { get; }
    public Texture2D BtnTexture { get; }
    public Texture2D BtnTextureHover { get; }
    public Texture2D BtnTextureDisabled { get; }

    public TexturePack(string rarityText, Texture2D btnTexture, Texture2D btnTextureHover, Texture2D btnTextureDisabled)
    {
      RarityText = rarityText;
      BtnTexture = btnTexture;
      BtnTextureHover = btnTextureHover;
      BtnTextureDisabled = btnTextureDisabled;
    }

    public bool Equals(TexturePack other)
    {
      return RarityText == other.RarityText
        && Equals(BtnTexture, other.BtnTexture)
        && Equals(BtnTextureHover, other.BtnTextureHover)
        && Equals(BtnTextureDisabled, other.BtnTextureDisabled);
    }

    public override bool Equals(object obj)
    {
      return obj is TexturePack other && Equals(other);
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(RarityText, BtnTexture, BtnTextureHover, BtnTextureDisabled);
    }
  }
}
