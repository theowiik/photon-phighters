using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;

public partial class CapturePoint : Node2D
{
    public delegate void CapturedEvent(CapturePoint which, Player.TeamEnum team);

    private const float TimeToCapture = 5f;

    private readonly ICollection<Player> _playersInside = new List<Player>();
    private bool _captured;

    /// <summary>
    ///     Light should reach +TimeToCapture.
    ///     Dark should reach -TimeToCapture.
    /// </summary>
    private float _captureTime;

    [GetNode("ProgressBar")]
    private ProgressBar _progressBar;

    public CapturedEvent CapturedListeners;

    public override void _Ready()
    {
        this.AutoWire();
        var area = GetNode<Area2D>("Area2D");
        area.BodyEntered += OnBodyEntered;
        area.BodyExited += OnBodyExited;
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is Player player) _playersInside.Add(player);
    }

    private void OnBodyExited(Node2D body)
    {
        if (body is Player player) _playersInside.Remove(player);
    }

    public override void _Process(double delta)
    {
        if (_captured) return;

        QueueRedraw();
        var diffPlayers = CalcActiveCaptureDiff();
        if (diffPlayers == 0) return;

        var diff = diffPlayers > 0 ? 1 : -1;
        _captureTime += diff * (float)delta;

        if (_captureTime >= TimeToCapture)
        {
            _captured = true;
            CapturedListeners?.Invoke(this, Player.TeamEnum.Light);
            _captureTime = TimeToCapture;
        }
        else if (_captureTime <= -TimeToCapture)
        {
            _captured = true;
            CapturedListeners?.Invoke(this, Player.TeamEnum.Dark);
            _captureTime = -TimeToCapture;
        }

        UpdateProgress();
    }

    private int CalcActiveCaptureDiff()
    {
        var lightPlayers = _playersInside.Count(p => p.Team == Player.TeamEnum.Light);
        var darkPlayers = _playersInside.Count(p => p.Team == Player.TeamEnum.Dark);
        return lightPlayers - darkPlayers;
    }

    private void UpdateProgress()
    {
        var min = -TimeToCapture;
        var max = TimeToCapture;
        var value = _captureTime;
        var progress = (value - min) / (max - min);
        _progressBar.Value = progress;
    }

    public override void _Draw()
    {
        const int radius = 300;
        var noneColor = new Color(0, 1, 0.8f, 0.3f);
        var lightColor = new Color(1, 1, 1, 0.3f);
        var darkColor = new Color(0, 0, 0, 0.3f);
        var tiedColor = new Color(1, 0.67f, 0, 0.3f);

        var diffPlayers = CalcActiveCaptureDiff();

        Color color;
        if (!_playersInside.Any())
            color = noneColor;
        else
            color = diffPlayers switch
            {
                0 => tiedColor,
                > 0 => lightColor,
                _ => darkColor
            };

        DrawCircle(Vector2.Zero, radius, color);
    }
}