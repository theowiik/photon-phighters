using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;

public partial class World : Node2D
{
    [GetNode("LightSpawn")]
    private Node2D _lightSpawn;

    [GetNode("DarkSpawn")]
    private Node2D _darkSpawn;

    [GetNode("CanvasLayer/Overlay")]
    private OverlayControllers.Overlay _overlay;

    [GetNode("CanvasLayer/PowerUpPicker")]
    private OverlayControllers.PowerUpPicker _powerUpPicker;

    [GetNode("RoundTimer")]
    private Timer _roundTimer;

    [GetNode("FollowingCamera")]
    private FollowingCamera _camera;

    [GetNode("Sfx/LightWin")]
    private AudioStreamPlayer _lightWin;

    [GetNode("Sfx/DarkWin")]
    private AudioStreamPlayer _darkWin;

    private Score _score;
    private const int RoundTime = 1;
    private const int ScoreToWin = 4;
    private IEnumerable<Player> _players;
    private Player _lightPlayer;
    private Player _darkPlayer;
    private Player _lastPlayerToScore;

    public override void _Ready()
    {
        NodeAutoWire.AutoWire(this);
        _score = new Score();
        _powerUpPicker.Visible = false;
        _powerUpPicker.PowerUpPickedListeners += OnPowerUpSelected;

        var uiUpdateTimer = GetNode<Timer>("UIUpdateTimer");
        uiUpdateTimer.Timeout += UpdateScore;
        uiUpdateTimer.Timeout += UpdateRoundTimer;

        var ob = GetNode<Area2D>("OutOfBounds");
        ob.BodyEntered += OnOutOfBounds;

        _players = GetTree().GetNodesInGroup("players").Cast<Player>();
        foreach (var player in _players)
        {
            player.PlayerDied += OnPlayerDied;
            player.Gun.ShootDelegate += OnShoot;
            _camera.AddTarget(player);
        }

        _lightPlayer = _players.First(p => p.PlayerNumber == 1);
        _darkPlayer = _players.First(p => p.PlayerNumber == 2);

        if (_lightPlayer == null || _darkPlayer == null)
            throw new Exception("Could not find players");

        StartRound();
    }

    private void OnPlayerDied(Player player)
    {
        player.GlobalPosition = player.PlayerNumber == 1 ? _lightSpawn.GlobalPosition : _darkSpawn.GlobalPosition;
        player.Freeze = true;

        var liveTimer = new Timer
        {
            OneShot = true,
            WaitTime = 2
        };
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

        _roundTimer.Timeout += OnRoundFinished;
        _roundTimer.Start(RoundTime);
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

        var results = GetResults();
        if (results.Light == results.Dark)
        {
            _score.Ties++;
            StartRound();
            return;
        }
        else if (results.Light > results.Dark)
        {
            _score.Light++;
            _lastPlayerToScore = _lightPlayer;
            _lightWin.Play();
        }
        else
        {
            _score.Dark++;
            _lastPlayerToScore = _darkPlayer;
            _darkWin.Play();
        }

        _overlay.TotalScore = $"Lightness: {_score.Light}, Darkness: {_score.Dark}, Ties: {_score.Ties}";
        if (_score.Dark >= ScoreToWin || _score.Light >= ScoreToWin)
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

        StartPowerUpSelection();
    }

    private void StartPowerUpSelection()
    {
        _powerUpPicker.WinningSide = _lastPlayerToScore.Team;
        _powerUpPicker.Visible = true;
        _powerUpPicker.GrabFocus();
        _powerUpPicker.Reset();
    }

    private void OnPowerUpSelected(PowerUpManager.IPowerUpApplier powerUp)
    {
        _powerUpPicker.Visible = false;
        powerUp.Apply(_lastPlayerToScore);
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