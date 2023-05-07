namespace PhotonPhighters.Scripts;
using Godot;
using PhotonPhighters.Scripts.Utils;

public partial class PlayerEffectsDelegate : Node2D
{
    public Sprite2D PlayerSprite { get; set; }

    // TODO: Create a object pool for particles
    private PackedScene _jumpParticlesScene = ResourceLoader.Load<PackedScene>("res://Objects/Player/Particles/JumpParticles.tscn");
    private PackedScene _deathParticlesScene = ResourceLoader.Load<PackedScene>("res://Objects/Player/Particles/DeathParticles.tscn");
    private PackedScene _hurtParticlesScene = ResourceLoader.Load<PackedScene>("res://Objects/Player/Particles/HurtParticles.tscn");

    [GetNode("AnimationPlayer")]
    private AnimationPlayer _animationPlayer;

    [GetNode("Sfx/DeathPlayer")]
    private AudioStreamPlayer2D _deathPlayer;

    [GetNode("Sfx/HurtPlayer")]
    private AudioStreamPlayer2D _hurtPlayer;

    [GetNode("Sfx/FallDeathPlayer")]
    private AudioStreamPlayer2D _fallDeathPlayer;

    [GetNode("Sfx/JumpPlayer")]
    private AudioStreamPlayer2D _jumpPlayer;

    [GetNode("HurtTimer")]
    private Timer _hurtTimer;

    private const string JumpAnimation = "stretch_jump";
    private const string LandAnimation = "squish_land";
    private const string RunLeftAnimation = "running_left";
    private const string RunRightAnimation = "running_right";
    private const string Wall = "squish_wall";
    private readonly Color _hurtColor = new(0.8f, 0, 0);

    public override void _Ready()
    {
        this.AutoWire();
        _hurtTimer.Timeout += HurtTimerOnTimeout;
    }

    private void HurtTimerOnTimeout() => PlayerSprite.Modulate = Colors.White;

    public delegate void PlayerEffectPerformed(Node2D effect);
    public PlayerEffectPerformed PlayerEffectAddedListeners;

    public void EmitDeathParticles()
    {
        var instance = GenerateParticles(_deathParticlesScene);
        PlayerEffectAddedListeners?.Invoke(instance);
    }

    public void EmitJumpParticles()
    {
        var instance = GenerateParticles(_jumpParticlesScene);
        PlayerEffectAddedListeners?.Invoke(instance);
    }

    public void EmitHurtParticles()
    {
        var instance = GenerateParticles(_hurtParticlesScene);
        PlayerEffectAddedListeners?.Invoke(instance);
    }

    private static Node2D GenerateParticles(PackedScene particlesScene)
    {
        var instance = particlesScene.Instantiate<CpuParticles2D>();
        var timer = TimerFactory.OneShotStartedTimer(instance.Lifetime);

        instance.Emitting = true;
        timer.Timeout += () => instance.QueueFree();
        instance.AddChild(timer);

        return instance;
    }

    public void PlayDeathSound() => _deathPlayer.Play();

    public void PlayHurtSound() => _hurtPlayer.Play();

    public void PlayFallDeathSound() => _fallDeathPlayer.Play();

    public void PlayJumpSound() => _jumpPlayer.Play();

    public void AnimationPlayJump() => _animationPlayer.Play(JumpAnimation);

    public void AnimationPlayLand() => _animationPlayer.Play(LandAnimation);

    public void AnimationPlayRunLeft() => _animationPlayer.Play(RunLeftAnimation);

    public void AnimationPlayRunRight() => _animationPlayer.Play(RunRightAnimation);

    public void AnimationPlayWall() => _animationPlayer.Play(Wall);

    public void AnimationPlayHurt()
    {
        PlayerSprite.Modulate = _hurtColor;
        _hurtTimer.Start();
    }
}
