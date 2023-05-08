using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;

public partial class Explosion : Node2D
{
    [GetNode("ExplosionPlayer")]
    private AudioStreamPlayer2D _explosionPlayer;
    
    [GetNode("CpuParticles2D")]
    private CpuParticles2D _explosionParticles;
    
    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
    }

    public void Explode()
    {
        _explosionParticles.Emitting = true;
        _explosionPlayer.Play();
    }
}