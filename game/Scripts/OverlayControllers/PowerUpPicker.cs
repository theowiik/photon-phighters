using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Godot;
using GodotSharper;
using GodotSharper.AutoGetNode;
using GodotSharper.Instancing;
using PhotonPhighters.Scripts.GSAlpha;
using PhotonPhighters.Scripts.PowerUps;

namespace PhotonPhighters.Scripts.OverlayControllers;

public partial class PowerUpPicker : Control
{
  public delegate void PowerUpSelectionEnded(IPowerUpApplier powerUpApplier);

  // TODO: Remove unused label (the empty strings)
  private static readonly Dictionary<Rarity, (string color, string text)> s_rarityThemes =
    new()
    {
      { Rarity.Common, ("Green", "") },
      { Rarity.Rare, ("Blue", "") },
      { Rarity.Epic, ("Purple", "") },
      { Rarity.Legendary, ("Orange", "") }
    };

  private readonly List<IPowerUpApplier> _availablePowerups = new();
  private IEnumerable<Player> _allPlayers;

  [GetNode("BackgroundRect")]
  private ColorRect _backgroundRect;

  private int _disabledDelay = 2;

  [GetNode("GridContainer")]
  private GridContainer _gridContainer;

  [GetNode("Label")]
  private Label _label;

  private Team _loser;

  [GetNode("RerollTextureButton")]
  private TextureButton _rerollButton;

  [GetNode("Timer")]
  private Timer _timer;

  [GetNode("TimerLabel")]
  private Label _timerLabel;

  private int _timeToChoose = 20;

  private Team _winner;
  public event PowerUpSelectionEnded PowerUpSelectionEndedListeners;

  public override void _Ready()
  {
    this.GetNodes();
    _rerollButton.Pressed += HandleReroll;
    _timer.Timeout += HandleTimerRunOut;
  }

  public override void _Process(double delta)
  {
    _timerLabel.Text = $"Time left: {_timer.TimeLeft:0.0}";
  }

  public void SetTheme(Team value)
  {
    switch (value)
    {
      case Team.Light:
        _backgroundRect.Color = new Color(1, 1, 1, 0.5f);
        _label.Modulate = Colors.Black;
        _label.Text = "Light team won! Darkness, pick a helping hand";
        break;

      case Team.Dark:
        _backgroundRect.Color = new Color(0, 0, 0, 0.5f);
        _label.Modulate = Colors.White;
        _label.Text = "Dark team won! Lightness, pick a helping hand";
        break;

      case Team.Neutral:
      default:
        throw new ArgumentOutOfRangeException(nameof(value), value, null);
    }
  }

  public void BeginPowerUpSelection(Team winner, Team loser, IEnumerable<Player> players)
  {
    _allPlayers = players;
    _winner = winner;
    _loser = loser;
    SetTheme(winner);

    Clear();
    Populate(loser);
    DisableRerollButton();
    _timer.Start(_timeToChoose);

    // TODO: Possibly re-add vibration for the loser (the one to select a power up)
    Visible = true;
    GrabFocus();
  }

  private void EndPowerUpSelection(IPowerUpApplier powerUpApplier)
  {
    Visible = false;
    powerUpApplier.Apply(_loser, _winner, _allPlayers);
    _timer.Stop();
    PowerUpSelectionEndedListeners?.Invoke(powerUpApplier);
  }

  private void HandleReroll()
  {
    Clear();
    Populate(_loser);
    DisableRerollButton();

    GetLoosingPlayers().ForEach(p => p.MaxHealth -= 10);
  }

  private void DisableRerollButton()
  {
    // Reroll button should be disabled at first to avoid mistakenly rerolling
    _rerollButton.Disabled = true;
    AddChild(
      TimerFactory.StartedSelfDestructingOneShot(
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

  private IEnumerable<Player> GetWinningPlayers()
  {
    return _allPlayers.Where(p => p.Team == _winner).ToList();
  }

  private IEnumerable<Player> GetLoosingPlayers()
  {
    return _allPlayers.Where(p => p.Team == _loser).ToList();
  }

  private void Populate(Team losingPlayer)
  {
    foreach (var powerUp in PowerUpManager.GetUniquePowerUps(4))
    {
      // Store the available PowerUps to pick one in case the timer runs out
      _availablePowerups.Add(powerUp);

      var powerUpButton = Instanter.Instantiate<PowerUpTextureButton>();
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
        TimerFactory.StartedSelfDestructingOneShot(
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

    var btnTexture = GDX.LoadOrFail<Texture2D>(
      $"res://Assets/Sprites/Buttons/{color}/card_{color.ToLower(CultureInfo.InvariantCulture)}.png"
    );
    var btnTextureHover = GDX.LoadOrFail<Texture2D>(
      $"res://Assets/Sprites/Buttons/{color}/card_{color.ToLower(CultureInfo.InvariantCulture)}_hover.png"
    );
    var btnTextureDisabled = GDX.LoadOrFail<Texture2D>(
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
