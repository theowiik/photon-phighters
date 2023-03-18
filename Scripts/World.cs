using Godot;
using System;

public partial class World : Node2D
{
    public override void _Ready()
    {
        GetNode<Player>("Player").Gun.ShootDelegate += OnShoot;
    }

    private void OnShoot(Node2D bullet)
    {
        AddChild(bullet);
    }

    public struct Results
    {
        public int On;
        public int Off;
        public int Neutral;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            GetTree().Quit();
        }

        if (@event.IsActionPressed("ui_down"))
        {
            var results = GetResults();
            GD.Print($"On: {results.On}, Off: {results.Off}, Neutral: {results.Neutral}");
        }
    }

    private Results GetResults()
    {
        var lights = GetTree().GetNodesInGroup("Light");
        var results = new Results();

        foreach (var light in lights)
        {
            var lightNode = light as Light;

            if (lightNode == null)
            {
                throw new Exception("Light node is not a Light!!");
            }

            switch (lightNode.LightState)
            {
                case Light.LightMode.On:
                    results.On++;
                    break;
                case Light.LightMode.Off:
                    results.Off++;
                    break;
                case Light.LightMode.Neutral:
                    results.Neutral++;
                    break;
            }
        }

        return results;
    }
}
