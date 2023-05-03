using System.Collections.Generic;
using Godot;

namespace PhotonPhighters.Scripts;

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

        Position = Position.Lerp(targetPosition, (float)delta * 5.0f);

        // Zoom
        FitZoom();
    }

    private void FitZoom()
    {
        // Calculate the bounding box of all objects in the list
        var bounds = new Rect2();
        foreach (var obj in _targets)
        {
            var rect = obj.GetViewportRect();
            rect.Position = obj.ToGlobal(rect.Position);
            bounds = bounds.Merge(rect);
        }

        // Calculate the target zoom level to fit the bounding box on the screen
        var screenBounds = GetViewportRect().Size;
        var targetZoom = Mathf.Min(screenBounds.X / bounds.Size.X, screenBounds.Y / bounds.Size.Y);

        // Smoothly adjust the camera's zoom level and set its offset to the center of the bounding box
        Zoom = Zoom.Lerp(new Vector2(targetZoom + 0.3f, targetZoom + 0.3f), 1);
    }
}