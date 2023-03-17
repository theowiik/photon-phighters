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

        var lights = 10;
        var perSide = lights / 4;
        var distancePerSide = sideLength / perSide;

        var side = BuildSide(new Godot.Vector2(-half, -half), perSide, distancePerSide);
        Placee(side);
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
