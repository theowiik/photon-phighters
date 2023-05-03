using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;

public partial class PlayerEffectsDelegate : Node2D
{
    [GetNode("Particles/ExplosionParticle")]
    private CpuParticles2D _explosionParticleEmitter;

    [GetNode("Particles/JumpParticles")]
    private CpuParticles2D _jumpParticleEmitter;

    [GetNode("Sfx/DeathPlayer")]
    private AudioStreamPlayer2D _deathPlayer;

    [GetNode("Sfx/HurtPlayer")]
    private AudioStreamPlayer2D _hurtPlayer;

    [GetNode("Sfx/FallDeathPlayer")]
    private AudioStreamPlayer2D _fallDeathPlayer;
    
    [GetNode("Sfx/JumpPlayer")]
    private AudioStreamPlayer2D _jumpPlayer;

    public override void _Ready()
    {
        this.AutoWire();
    }
    
    public void PlayExplosionParticles()
    {
        GD.Print("Playing explosion particles");
        _explosionParticleEmitter.Emitting = true;
    }
    
    public void PlayJumpParticles()
    {
        GD.Print("Playing jump particles");
        _jumpParticleEmitter.Emitting = true;
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
}