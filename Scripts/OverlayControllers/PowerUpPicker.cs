using System;
using System.Collections.Generic;
using System.Globalization;
using Godot;
using PhotonPhighters.Scripts.GoSharper;
using PhotonPhighters.Scripts.GoSharper.AutoWiring;
using PhotonPhighters.Scripts.GoSharper.Instancing;
using PhotonPhighters.Scripts.PowerUps;
using static PhotonPhighters.Scripts.Player;

namespace PhotonPhighters.Scripts.OverlayControllers;

public partial class PowerUpPicker : Control
{
  public delegate void PowerUpSelectionEnded();

  private static readonly Dictionary<PowerUps.PowerUps.Rarity, (string color, string text)> s_rarityThemes =
    new()
    {
      { PowerUps.PowerUps.Rarity.Common, ("Green", "") },
      { PowerUps.PowerUps.Rarity.Rare, ("Blue", "(Rare)") },
      { PowerUps.PowerUps.Rarity.Epic, ("Purple", "(Epic)") },
      { PowerUps.PowerUps.Rarity.Legendary, ("Orange", "(LEGENDARY)") }
    };

  [GetNode("BackgroundRect")]
  private ColorRect _backgroundRect;

  [GetNode("GridContainer")]
  private GridContainer _gridContainer;

  [GetNode("Label")]
  private Label _label;

  [GetNode("RerollTextureButton")]
  private TextureButton _rerollButton;

  [GetNode("TimerLabel")]
  private Label _timerLabel;

  [GetNode("Timer")]
  private Timer _timer;

  private Player _loser;

  private Player _winner;

  private int _disabledDelay = 2;

  private int _timeToChoose = 20;
  private List<IPowerUpApplier> _availablePowerups = new();
  public event PowerUpSelectionEnded PowerUpSelectionEndedListeners;

  public override void _Ready()
  {
    this.AutoWire();
    _rerollButton.Pressed += () => HandleReroll();
    _timer.Timeout += HandleTimerRunOut;
  }

  public override void _Process(double delta)
  {
    _timerLabel.Text = string.Format("Time left: {0}", _timer.TimeLeft.ToString("0.0"));
  }

  public void SetTheme(TeamEnum value)
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

  public void BeginPowerUpSelection(Player winner, Player loser)
  {
    _winner = winner;
    _loser = loser;
    SetTheme(winner.Team);

    Clear();
    Populate(loser);
    DisableRerollButton();
    _timer.Start(_timeToChoose);

    loser.VibrateGamepadWeak(0.25f);
    Visible = true;
    GrabFocus();
  }

  private void EndPowerUpSelection(IPowerUpApplier powerUpApplier)
  {
    Visible = false;
    powerUpApplier.Apply(_loser, _winner);
    _timer.Stop();
    PowerUpSelectionEndedListeners.Invoke();
  }

  private void HandleReroll()
  {
    Clear();
    Populate(_loser);
    DisableRerollButton();
    _loser.MaxHealth -= 10;
  }

  private void DisableRerollButton()
  {
    // Reroll button should be disabled at first to avoid mistakenly rerolling
    _rerollButton.Disabled = true;
    AddChild(
      GsTimerFactory.OneShotSelfDestructingStartedTimer(
        _disabledDelay,
        () =>
        {
          _rerollButton.Disabled = false;
          _rerollButton.GrabFocus();
        }
      )
    );
  }

  private void HandleTimerRunOut()
  {
    // Pick a random PowerUp and end the PowerUp selection
    var random = new Random();
    var index = random.Next(_availablePowerups.Count);
    var powerUp = _availablePowerups[index];
    EndPowerUpSelection(powerUp);
  }

  private void Clear()
  {
    foreach (var powerUpButton in _gridContainer.GetNodesOfType<TextureButton>())
    {
      powerUpButton.QueueFree();
    }
  }

  private void Populate(Player losingPlayer)
  {
    foreach (var powerUp in PowerUpManager.GetUniquePowerUps(4))
    {
      // Store the available PowerUps to pick one in case the timer runs out
      _availablePowerups.Add(powerUp);

      var powerUpButton = GsInstanter.Instantiate<PowerUpTextureButton>();
      _gridContainer.AddChild(powerUpButton);
      var texturePack = GetThemeTextures(powerUp);

      powerUpButton.SetPowerUpName($"{texturePack.RarityText} {powerUp.Name}");
      powerUpButton.SetMark(powerUp.GetMarkName(losingPlayer));
      powerUpButton.TextureNormal = texturePack.BtnTexture;
      powerUpButton.TextureHover = texturePack.BtnTextureHover;
      powerUpButton.TextureDisabled = texturePack.BtnTextureDisabled;
      powerUpButton.Pressed += () => EndPowerUpSelection(powerUp);

      // Disable at first
      powerUpButton.Disabled = true;
      AddChild(
        GsTimerFactory.OneShotSelfDestructingStartedTimer(
          _disabledDelay,
          () =>
          {
            powerUpButton.TextureFocused = texturePack.BtnTextureHover;
            powerUpButton.Disabled = false;
          }
        )
      );
    }
  }

  private static TexturePack GetThemeTextures(IPowerUpApplier powerUp)
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

  private readonly struct TexturePack : IEquatable<TexturePack>
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
