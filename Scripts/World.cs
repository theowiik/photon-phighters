using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class World : Node2D
{
    [GetNode("P1Spawn")]
    private Node2D _lightSpawn;

    [GetNode("P2Spawn")]
    private Node2D _darkSpawn;

    [GetNode("CanvasLayer/Overlay")]
    private Overlay _overlay;

    [GetNode("RoundTimer")]
    private Timer _roundTimer;

    [GetNode("FollowingCamera")]
    private FollowingCamera _camera;

    [GetNode("Sfx/LightWin")]
    private AudioStreamPlayer _lightWin;

    [GetNode("Sfx/DarkWin")]
    private AudioStreamPlayer _darkWin;

    private const int RoundTime = 30;
    private IEnumerable<Player> _players;
    private Score _score;
    private const int ScoreToWin = 4;

    public override void _Ready()
    {
        NodeAutoWire.AutoWire(this);
        _score = new Score();
        _overlay.PowerUpSelected += OnPowerUpSelected;

        var uiUpdateTimer = GetNode<Timer>("UIUpdateTimer");
        uiUpdateTimer.Timeout += UpdateScore;
        uiUpdateTimer.Timeout += UpdateRoundTimer;
        _players = this.GetNodes<Player>().ToList();

        var ob = GetNode<Area2D>("OutOfBounds");
        ob.BodyEntered += OnOutOfBounds;

        foreach (var player in _players)
        {
            player.PlayerDied += OnPlayerDied;
            player.Gun.ShootDelegate += OnShoot;
            _camera.AddTarget(player);

            // TODO: dont do this
            if (player.PlayerNumber == 1)
            {
                GameState.Player1 = player;
            }

            if (player.PlayerNumber == 2)
            {
                GameState.Player2 = player;
            }
        }

        StartRound();
    }

    private void OnPlayerDied(Player player)
    {
        player.GlobalPosition = player.PlayerNumber == 1 ? _lightSpawn.GlobalPosition : _darkSpawn.GlobalPosition;
        player.Freeze = true;

        var liveTimer = new Timer();
        liveTimer.OneShot = true;
        liveTimer.WaitTime = 2;
        liveTimer.Timeout += () => player.Freeze = false;
        AddChild(liveTimer);
        liveTimer.Start();
    }

    private void OnOutOfBounds(Node body)
    {
        if (body is Player player)
        {
            player.TakeDamage(99999999);
        }
    }

    private void StartRound()
    {
        ResetLights();

        foreach (var player in _players)
            player.Freeze = false;

        _roundTimer.Start(RoundTime);
        _roundTimer.Timeout += OnRoundFinished;
    }

    private void OnRoundFinished()
    {
        GD.Print("Round ended");

        foreach (var player in _players)
            player.Freeze = true;

        // Remove all bullets
        foreach (var bullet in GetTree().GetNodesInGroup("bullets"))
        {
            bullet.QueueFree();
        }

        var isTie = false;
        var results = GetResults();
        if (results.Light == results.Dark)
        {
            isTie = true;
            _score.Ties++;
        }
        else if (results.Light > results.Dark)
        {
            _score.Light++;
            // GameState.Player1Won = true;
            _lightWin.Play();
        }
        else
        {
            _score.Dark++;
            // GameState.Player1Won = false;
            _darkWin.Play();
        }

        if (isTie)
        {
            StartRound();
            return;
        }

        _overlay.TotalScore = $"Lightness: {_score.Light}, Darkness: {_score.Dark}, Ties: {_score.Ties}";
        // TODO: Make this even harder to read
        if (Math.Sqrt(_score.Dark * _score.Dark) + Math.Sqrt(_score.Light * _score.Light) >= ScoreToWin)
        {
            GD.Print("Game over");

            if (_score.Light > _score.Dark)
            {
                GetTree().ChangeSceneToFile("res://Scenes/EndScreenLight.tscn");
            }
            else
            {
                GetTree().ChangeSceneToFile("res://Scenes/EndScreenDarkness.tscn");
            }

        }

        _overlay.StartPowerUpSelection();

        // Wait for PowerUpSelected signal
        // StartRound();
    }

    private void OnPowerUpSelected()
    {
        StartRound();
    }

    private void ResetLights()
    {
        var lights = GetTree().GetNodesInGroup("lights");

        foreach (var light in lights)
        {
            if (light is not Light lightNode)
                throw new Exception("Light node is not a Light!!");

            lightNode.SetLight(Light.LightMode.None);
        }
    }

    private void OnShoot(Node2D bullet)
    {
        AddChild(bullet);
    }

    public struct Results
    {
        public int Light;
        public int Dark;
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

        if (results.Light == 0 && results.Dark == 0)
        {
            return;
        }

        _overlay.RoundScore = results;
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
                    results.Light++;
                    break;
                case Light.LightMode.Dark:
                    results.Dark++;
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
