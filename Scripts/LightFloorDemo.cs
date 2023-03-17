using System.Linq;
using System.Collections.Generic;
using System.Numerics;
using Godot;

public partial class LightFloorDemo : StaticBody2D
{
    private PackedScene _lightScene = GD.Load<PackedScene>("res://Objects/Light.tscn");
    private Node2D _lights;

    public override void _Ready()
    {
        _lights = GetNode<Node2D>("Lights");
        Place2();
    }

    private void PlaceLights()
    {
        var sideLength = 130;
        var half = sideLength / 2;

        GD.Print("Placing lights");
        var lights = 100;
        var perSide = lights / 4.0;
        var distancePerSide = sideLength / perSide;
    }

    private void Place2(){
        var sideLength = 130;
        var half = sideLength / 2;

        var lights = 20;
        var perSide = lights / 4;
        var distancePerSide = sideLength / perSide;

        var basee = BuildSide(new Godot.Vector2(half, -half), perSide, distancePerSide);
        var side = basee;
        var bottom = basee.Select(v => v.Rotated((Mathf.Pi / 2) * 1));
        var left = basee.Select(v => v.Rotated((Mathf.Pi / 2) * 2));
        var top = basee.Select(v => v.Rotated((Mathf.Pi / 2) * 3));

        Placee(side);
        Placee(bottom);
        Placee(left);
        Placee(top);
    }

    private void Placee(IEnumerable<Godot.Vector2> vectors) {
        foreach (var vector in vectors)
        {
            var light = _lightScene.Instantiate<Node2D>();
            _lights.AddChild(light);
            light.Position = vector;
        }
    }

    private IEnumerable<Godot.Vector2> BuildSide(Godot.Vector2 baseVector, int lightsOnSide, float apart)
    {
        var output = new List<Godot.Vector2>();

        for (var i = 0; i < lightsOnSide; i++)
        {
            output.Add(baseVector + new Godot.Vector2(0, apart * i));
        }

        return output;
    }
}
