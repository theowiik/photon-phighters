using Godot;
using System;

public partial class World : Node2D
{
    private Overlay _overlay;

    public override void _Ready()
    {
        _overlay = GetNode<Overlay>("Overlay");
        GetNode<Player>("Player").Gun.ShootDelegate += OnShoot;
        GetNode<Timer>("ScoreUpdateTimer").Timeout += UpdateScore;
    }

    private void OnShoot(Node2D bullet)
    {
        AddChild(bullet);
    }

    private struct Results
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
    }

    private void UpdateScore()
    {
        var results = GetResults();

        if (results.On == 0 && results.Off == 0) {
            _overlay.Score = "Go!";
            return;
        }

        var percentageOn = (float)results.On / (results.On + results.Off);
        var percentageOff = (float)results.Off / (results.On + results.Off);
        GD.Print(results.On);
        _overlay.Score = $"Lightness: {percentageOn * 100}%, Darkness: {percentageOff * 100}%";
    }

    private Results GetResults()
    {
        var lights = GetTree().GetNodesInGroup("Light");
        var results = new Results();

        foreach (var light in lights)
        {
            if (light is not Light lightNode)
                throw new Exception("Light node is not a Light!!");

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
