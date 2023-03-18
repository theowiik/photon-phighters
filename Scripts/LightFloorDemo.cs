using System.Linq;
using System.Collections.Generic;
using Godot;

public partial class LightFloorDemo : StaticBody2D
{
    private PackedScene _lightScene = GD.Load<PackedScene>("res://Objects/Light.tscn");
    private Node2D _lights;

    public override void _Ready()
    {
        _lights = GetNode<Node2D>("Lights");
        BasicLightPlacement();
    }

    private void BasicLightPlacement()
    {
        // TODO: Dont hardcode side length
        var sideLength = 130;

        var half = sideLength / 2;
        var lights = 20;
        var lightsPerSide = lights / 4;
        var distancePerSide = sideLength / lightsPerSide;
        var baseSide = BuildSide(new Godot.Vector2(half, -half), lightsPerSide, distancePerSide);

        for (var i = 0; i < 4; i++)
        {
            var side = baseSide.Select(v => v.Rotated(Mathf.Pi / 2 * i));
            Placee(side);
        }
    }

    private void Placee(IEnumerable<Godot.Vector2> vectors)
    {
        foreach (var vector in vectors)
        {
            var light = _lightScene.Instantiate<Node2D>();
            _lights.AddChild(light);
            light.Position = vector;
        }
    }

    private IEnumerable<Vector2> BuildSide(Vector2 baseVector, int lightsOnSide, float apart)
    {
        var output = new List<Vector2>();

        for (var i = 0; i < lightsOnSide; i++)
        {
            output.Add(baseVector + new Godot.Vector2(0, apart * i));
        }

        return output;
    }
}
