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

    [GetNode("Label")]
    private Label _label;

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
        if (body is Player player)
        {
            _playersInside.Add(player);
        }
    }

    private void OnBodyExited(Node2D body)
    {
        if (body is Player player)
        {
            _playersInside.Remove(player);
        }
    }

    public override void _Process(double delta)
    {
        if (_captured) return;

        QueueRedraw();
        var diffPlayers = CalcActiveCaptureDiff();
        if (diffPlayers == 0)
        {
            return;
        }

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
        var progress = CreateProgressBar(-TimeToCapture, TimeToCapture, _captureTime, 10);
        _label.Text = progress;
    }

    private static string CreateProgressBar(float minValue, float maxValue, float currentValue, int barLength)
    {
        const char first = '-';
        const char last = '_';

        if (minValue > maxValue)
        {
            throw new ArgumentException("Min value should not be greater than max value.");
        }

        if (barLength <= 0)
        {
            throw new ArgumentException("Bar length should be greater than 0.");
        }

        if (currentValue >= maxValue)
        {
            return "Light captured";
            return new string(first, barLength);
        }

        if (currentValue < minValue)
        {
            return "Dark captured";
            return new string(last, barLength);
        }

        var progress = (currentValue - minValue) / (maxValue - minValue);
        var filledLength = (int)(barLength * progress);

        var filled = new string(first, filledLength);
        var empty = new string(last, barLength - filledLength);

        return filled + empty;
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
        {
            color = noneColor;
        }
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