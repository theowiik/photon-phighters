using System;
using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;

public partial class CapturePoint : Node2D
{
    [GetNode("Area2D")]
    private Area2D _area;

    [GetNode("Label")]
    private Label _label;

    private const float TimeToCapture = 5f;

    /// <summary>
    ///     Light should reach +TimeToCapture.
    ///     Dark should reach -TimeToCapture.
    /// </summary>
    private float _captureTime;

    public override void _Ready()
    {
        this.AutoWire();
    }

    public override void _Process(double delta)
    {
        _captureTime += (float)delta;
        UpdateProgress();
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
}