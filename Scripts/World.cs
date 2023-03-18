using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class World : Node2D
{
    private Overlay _overlay;
    private Timer _roundTimer;
    private FollowingCamera _camera;
    private const int RoundTime = 2;
    private IEnumerable<Player> _players;
    private Score _score;

    public override void _Ready()
    {
        _score = new Score();
        _roundTimer = GetNode<Timer>("RoundTimer");
        _overlay = GetNode<Overlay>("FollowingCamera/Overlay");
        _camera = GetNode<FollowingCamera>("FollowingCamera");
        var uiUpdateTimer = GetNode<Timer>("UIUpdateTimer");
        uiUpdateTimer.Timeout += UpdateScore;
        uiUpdateTimer.Timeout += UpdateRoundTimer;
        _players = this.GetNodes<Player>().ToList();

        foreach (var player in _players)
        {
            player.Gun.ShootDelegate += OnShoot;
            _camera.AddTarget(player);
        }

        StartRound();
    }

    private void StartRound()
    {
        foreach (var player in _players)
            player.AllowInputs = true;

        _roundTimer.Start(RoundTime);
        _roundTimer.Timeout += OnRoundFinished;
    }

    private void OnRoundFinished()
    {
        GD.Print("Round ended");

        foreach (var player in _players)
            player.AllowInputs = false;

        var results = GetResults();
        if (results.On == results.Off)
        {
            GD.Print("Round ended in a tie");
            _score.Ties++;
        }
        else if (results.On > results.Off)
        {
            GD.Print("Lightness won the round");
            _score.Light++;
        }
        else
        {
            GD.Print("Darkness won the round");
            _score.Dark++;
        }

        _overlay.TotalScore = $"Lightness: {_score.Light}, Darkness: {_score.Dark}, Ties: {_score.Ties}";

        if (_score.Light == 3 || _score.Dark == 3)
        {
            GD.Print("Game over");
            GetTree().Quit();
        }

        StartRound();
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

    private void UpdateRoundTimer()
    {
        _overlay.Time = $"{Math.Round(_roundTimer.TimeLeft, 1)}s";
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

    private struct Score
    {
        public int Light;
        public int Dark;
        public int Ties;
    }
}
