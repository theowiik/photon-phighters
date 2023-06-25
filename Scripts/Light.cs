﻿using Godot;
using PhotonPhighters.Scripts.GoSharper;
using PhotonPhighters.Scripts.GoSharper.AutoWiring;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;

public partial class Light : Area2D
{
  public enum LightMode
  {
    Light,
    Dark,
    None
  }

  private readonly Color _darkColorModulate = new(0, 0, 0, 0.5f);

  private readonly Color _lightColorModulate = new(1, 1, 1, 0.5f);

  [GsAutoWiring("AnimationPlayer")]
  private AnimationPlayer _animationPlayer;

  [GsAutoWiring("LightSprite")]
  private Sprite2D _lightSprite;

  public LightMode LightState { get; private set; }

  public override void _Ready()
  {
    this.AutoWire();
    _lightSprite.Visible = false;
    LightState = LightMode.None;
  }

  public void SetLight(LightMode lightMode)
  {
    if (LightState == lightMode)
    {
      return;
    }

    if (lightMode == LightMode.None)
    {
      _lightSprite.Visible = false;
      LightState = LightMode.None;
      return;
    }

    LightState = lightMode;
    _lightSprite.Visible = true;
    _lightSprite.Modulate = LightState == LightMode.Light ? _lightColorModulate : _darkColorModulate;
  }
}
