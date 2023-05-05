using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;

public partial class PlayerEffectsDelegate : Node2D
{
    private PackedScene _jumpParticlesScene = ResourceLoader.Load<PackedScene>("res://Objects/Player/Particles/JumpParticles.tscn");
    private PackedScene _deathParticlesScene = ResourceLoader.Load<PackedScene>("res://Objects/Player/Particles/DeathParticles.tscn");

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
    
    private const string JumpAnimation = "stretch_jump";
    private const string LandAnimation = "squish_land";
    private const string RunLeftAnimation = "running_left";
    private const string RunRightAnimation = "running_right";
    private const string Wall = "squish_wall";

    public override void _Ready()
    {
        this.AutoWire();
    }

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
        GD.Print("hurt!");
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
    
    public void PlayDeathSound()
    {
        GD.Print("Playing death sound");
        _deathPlayer.Play();
    }
    
    public void PlayHurtSound()
    {
        GD.Print("Playing hurt sound");
        _hurtPlayer.Play();
    }
    
    public void PlayFallDeathSound()
    {
        GD.Print("Playing fall death sound");
        _fallDeathPlayer.Play();
    }

    public void PlayJumpSound()
    {
        GD.Print("Playing jump sound");
        _jumpPlayer.Play();
    }

    public void AnimationPlayJump()
    {
        _animationPlayer.Play(JumpAnimation);
    }
    
    public void AnimationPlayLand()
    {
        _animationPlayer.Play(LandAnimation);
    }
    
    public void AnimationPlayRunLeft()
    {
        _animationPlayer.Play(RunLeftAnimation);
    }

    public void AnimationPlayRunRight()
    {
        _animationPlayer.Play(RunRightAnimation);
    }
    
    public void AnimationPlayWall()
    {
        _animationPlayer.Play(Wall);
    }
}