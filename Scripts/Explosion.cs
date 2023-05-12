using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;

public partial class Explosion : Node2D
{
    [GetNode("Area2D")]
    private Area2D _area;

    [GetNode("CPUParticles2D")]
    private CpuParticles2D _explosionParticles;

    [GetNode("ExplosionPlayer")]
    private AudioStreamPlayer2D _explosionPlayer;

    public Light.LightMode LightMode { get; set; }

    public override void _Ready()
    {
        this.AutoWire();
    }

    public void Explode()
    {
        _explosionParticles.Emitting = true;
        _explosionPlayer.Play();
        ColorLightsInsideRadius();

        var timer = TimerFactory.OneShotStartedTimer(_explosionParticles.Lifetime);
        timer.Timeout += QueueFree;
        AddChild(timer);
    }

    private async void ColorLightsInsideRadius()
    {
        var lights = await GetAllLightsInsideArea();
        foreach (var light in lights) light.SetLight(LightMode);
    }

    private async Task<IEnumerable<Light>> GetAllLightsInsideArea()
    {
        // TODO: Wait for an idle frame instead of a fixed number of frames
        const int frameToWait = 10;

        for (var i = 0; i < frameToWait; i++)
            await ToSignal(GetTree(), "process_frame");

        var areas = _area.GetOverlappingAreas();
        return areas.OfType<Light>();
    }
}