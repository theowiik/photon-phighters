using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;

public partial class Explosion : Node2D
{
    [GetNode("ExplosionPlayer")]
    private AudioStreamPlayer2D _explosionPlayer;

    [GetNode("CPUParticles2D")]
    private CpuParticles2D _explosionParticles;
    
    [GetNode("Area2D")]
    private Area2D _area;
    
    /// <summary>
    ///    If true, will get all lights inside the area and set them to dark.
    ///    Semi hacky way due to physics engine not detecting areas in the first frame.
    ///    Wait for the next frame to get all lights inside the area.
    /// </summary>
    private bool _getLightsInsideArea;

    public override void _Ready()
    {
        this.AutoWire();
    }

    public override void _PhysicsProcess(double delta)
    {
    }

    public void Explode()
    {
        _explosionParticles.Emitting = true;
        _explosionPlayer.Play();
        ColorLightsInsideRadius();
    }

    private async void ColorLightsInsideRadius()
    {
        var lights = await GetAllLightsInsideArea();
        foreach (var light in lights)
        {
            light.SetLight(Light.LightMode.Dark);
        }
    }

    private async Task<IEnumerable<Light>> GetAllLightsInsideArea()
    {
        await ToSignal(GetTree(), "process_frame");
        await ToSignal(GetTree(), "process_frame");
        await ToSignal(GetTree(), "process_frame");
        var areas = _area.GetOverlappingAreas();
        return areas.OfType<Light>();
    }
}