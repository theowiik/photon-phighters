using Godot;
using System;

public partial class World : Node2D
{
    private ScoreCounter _scoreCounter;

    public override void _Ready()
    {
        _scoreCounter = GetNode<ScoreCounter>("ScoreCounter");
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_down"))
        {
            var results = _scoreCounter.GetResults();
            GD.Print($"On: {results.On}, Off: {results.Off}, Neutral: {results.Neutral}");
        }
    }
}
