using Godot;
using System;
using System.Linq;

public partial class World : Node2D
{
    private Overlay _overlay;

    public override void _Ready()
    {
        _overlay = GetNode<Overlay>("Overlay");
        GetNode<Timer>("ScoreUpdateTimer").Timeout += UpdateScore;
        this.GetNodes<Player>().ToList().ForEach(p => p.Gun.ShootDelegate += OnShoot);
    }

    private void OnShoot(Node2D bullet)
    {
        GD.Print("Shooting in World!");
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

        if (results.On == 0 && results.Off == 0)
        {
            _overlay.Score = "Go!";
            return;
        }

        var percentageOn = (float)results.On / (results.On + results.Off);
        var roundedOn = Math.Round(percentageOn * 100, 2);
        var percentageOff = (float)results.Off / (results.On + results.Off);
        var roundedOff = Math.Round(percentageOff * 100, 2);
        _overlay.Score = $"Lightness: {roundedOn}%, Darkness: {roundedOff}%";
    }

    private Results GetResults()
    {
        var lights = GetTree().GetNodesInGroup("lights");
        var results = new Results();

        foreach (var light in lights)
        {
            if (light is not Light lightNode)
                throw new Exception("Light node is not a Light!!");

            switch (lightNode.LightState)
            {
                case Light.LightMode.Light:
                    results.On++;
                    break;
                case Light.LightMode.Dark:
                    results.Off++;
                    break;
                case Light.LightMode.None:
                    results.Neutral++;
                    break;
            }
        }

        return results;
    }
}
