using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;

public partial class PlayerEffectsDelegate : Node2D
{
  public delegate void PlayerEffectPerformed(Node2D effect);

  private const string JumpAnimation = "jump";
  private const string LandAnimation = "land";
  private const string RunLeftAnimation = "run_left";
  private const string RunRightAnimation = "x";
  private const string SpawnAnimation = "x";
  private const string Wall = "x";
  private readonly Color _hurtColor = new(0.8f, 0, 0);

  private readonly PackedScene _hurtParticlesScene = ResourceLoader.Load<PackedScene>(
    "res://Objects/Player/Particles/HurtParticles.tscn"
  );

  // TODO: Create a object pool for particles
  private readonly PackedScene _jumpParticlesScene = ResourceLoader.Load<PackedScene>(
    "res://Objects/Player/Particles/JumpParticles.tscn"
  );

  [GetNode("Sfx/DeathPlayer")]
  private AudioStreamPlayer2D _deathPlayer;

  [GetNode("Sfx/FallDeathPlayer")]
  private AudioStreamPlayer2D _fallDeathPlayer;

  [GetNode("Sfx/Hurt2Player")]
  private AudioStreamPlayer2D _hurt2Player;

  [GetNode("Sfx/HurtPlayer")]
  private AudioStreamPlayer2D _hurtPlayer;

  [GetNode("HurtTimer")]
  private Timer _hurtTimer;

  [GetNode("Sfx/JumpPlayer")]
  private AudioStreamPlayer2D _jumpPlayer;

  [GetNode("AnimationPlayer")]
  private AnimationPlayer _animationPlayer;

  public PlayerEffectPerformed PlayerEffectAddedListeners { get; set; }

  public Sprite2D PlayerSprite { get; set; }

  public override void _Ready()
  {
    this.AutoWire();
    _hurtTimer.Timeout += HurtTimerOnTimeout;
  }

  public void AnimationPlayHurt()
  {
    PlayerSprite.Modulate = _hurtColor;
    _hurtTimer.Start();
  }

  public void AnimationPlayJump()
  {
    _animationPlayer.Stop();
    _animationPlayer.Play(JumpAnimation);
  }

  public void AnimationPlayLand()
  {
    _animationPlayer.Stop();
    _animationPlayer.Play(LandAnimation);
  }

  public void AnimationPlayRunLeft()
  {
    _animationPlayer.Play(RunLeftAnimation);
  }

  public void AnimationPlayRunRight()
  {
    _animationPlayer.Play(RunLeftAnimation);
  }

  public void AnimationPlaySpawn()
  {
    // _animationPlayer.Play(SpawnAnimation);
  }

  public void AnimationPlayWall()
  {
    // _animationPlayer.Play(Wall);
  }

  public void EmitHurtParticles()
  {
    var instance = GenerateParticles(_hurtParticlesScene);
    PlayerEffectAddedListeners?.Invoke(instance);
  }

  public void EmitJumpParticles()
  {
    var instance = GenerateParticles(_jumpParticlesScene);
    PlayerEffectAddedListeners?.Invoke(instance);
  }

  public void PlayDeathSound()
  {
    _deathPlayer.Play();
  }

  public void PlayFallDeathSound()
  {
    _fallDeathPlayer.Play();
  }

  public void PlayHurtSound()
  {
    _hurtPlayer.PitchScale = (float)GD.RandRange(0.8, 1.2);
    _hurtPlayer.Play();

    if (GD.Randf() > 0.6)
    {
      _hurt2Player.Play();
    }
  }

  public void PlayJumpSound()
  {
    _jumpPlayer.Play();
  }

  private static Node2D GenerateParticles(PackedScene particlesScene)
  {
    var instance = particlesScene.Instantiate<CpuParticles2D>();
    var timer = TimerFactory.OneShotStartedTimer(instance.Lifetime, () => instance.QueueFree());

    instance.Emitting = true;
    instance.AddChild(timer);

    return instance;
  }

  private void HurtTimerOnTimeout()
  {
    PlayerSprite.Modulate = Colors.White;
  }
}
