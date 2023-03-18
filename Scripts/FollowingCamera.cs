using Godot;
using System.Collections.Generic;

public partial class FollowingCamera : Camera2D
{
    private readonly IList<Node2D> _targets = new List<Node2D>();

    public void AddTarget(Node2D target)
    {
        if (_targets.Contains(target))
            return;

        if (target == null)
            return;

        _targets.Add(target);
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_targets.Count == 0)
            return;

        var targetPosition = Vector2.Zero;
        foreach (var target in _targets)
            targetPosition += target.Position;

        targetPosition /= _targets.Count;

        Position = Position.Lerp(targetPosition, (float)delta * 5.0f); ;
    }
}
