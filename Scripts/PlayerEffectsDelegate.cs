using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;

public partial class PlayerEffectsDelegate : Node2D
{
    private PackedScene _jumpParticlesScene = ResourceLoader.Load<PackedScene>("res://Objects/Player/JumpParticles.tscn");

    [GetNode("AnimationPlayer")]
    private AnimationPlayer _animationPlayer;
    
    [GetNode("Particles/ExplosionParticle")]
    private CpuParticles2D _explosionParticleEmitter;

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
    private const string RunLeftAnimation = "runing_left";
    private const string RunRightAnimation = "running_right";
    private const string Wall = "squish_wall";

    public override void _Ready()
    {
        this.AutoWire();
    }
    
    public void EmitExplosionParticles()
    {
        GD.Print("Playing explosion particles");
        _explosionParticleEmitter.Emitting = true;
    }
    
    public void EmitJumpParticles()
    {
        GD.Print("Playing jump particles");
        var instance = _jumpParticlesScene.Instantiate<CpuParticles2D>();
        var timer = TimerFactory.OneShotStartedTimer(instance.Lifetime);

        instance.Emitting = true;
        timer.Timeout += () => instance.QueueFree();
        instance.AddChild(timer);
        AddChild(instance);
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