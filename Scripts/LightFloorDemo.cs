using Godot;

public partial class LightFloorDemo : StaticBody2D
{
    private PackedScene _lightScene = GD.Load<PackedScene>("res://Objects/Light.tscn");
    private Node2D _lights;

    public override void _Ready()
    {
        PlaceLights();
        _lights = GetNode<Node2D>("Lights");
    }

    private void PlaceLights()
    {
        var sideLength = 130;
        var half = sideLength / 2;

        GD.Print("Placing lights");
        var lights = 100;
        var perSide = lights / 4.0;
        var distancePerSide = sideLength / perSide;

        for (var i = 0; i < lights; i++)
        {
            var pos = new Vector2((float) (half), (float) (-half + distancePerSide * i));
            var light = (Light)_lightScene.Instantiate();
            AddChild(light);
            light.Position = pos;
        }
    }
}
