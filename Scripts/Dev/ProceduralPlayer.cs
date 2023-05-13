using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts.Dev;

public partial class ProceduralPlayer : StaticBody2D
{
    [GetNode("LeftLeg")]
    private Node2D _leftLeg;

    [GetNode("RightLeg")]
    private Node2D _rightLeg;

    [GetNode("LeftRay")]
    private RayCast2D _leftRay;

    [GetNode("RightRay")]
    private RayCast2D _rightRay;

    private Vector2 _leftLegPosition;
    private Vector2 _rightLegPosition;

    public override void _Ready()
    {
        this.AutoWire();
        _leftRay.Enabled = true;
    }

    public override void _Process(double delta)
    {
        Movement(delta);
        var getRayPointHit = _rightRay.GetCollider();
        var maxAngleDiff = Mathf.Pi / 4;

        if (_leftRay.IsColliding())
        {
            var hitPoint = _leftRay.GetCollisionPoint();
            GD.Print("Hit point: " + hitPoint);
            GD.Print("Current leg position: " + _leftLegPosition);

            if (_leftLegPosition == Vector2.Zero)
            {
                _leftLegPosition = hitPoint;
                GD.Print("initial");
            }

            if (_leftLegPosition.DistanceTo(hitPoint) > 0.1)
            {
                _leftLegPosition = hitPoint;
                GD.Print("switching");
            }

            var angleToLeftRay = GlobalPosition.AngleTo(_leftLegPosition);
            _leftLeg.Rotation = -angleToLeftRay;
        }
        else
        {
            _leftLeg.Rotation = 0;
        }
    }

    private void Movement(double delta)
    {
        var input = new Vector2();
        if (Input.IsActionPressed("ui_right"))
            input.X += 1;
        if (Input.IsActionPressed("ui_left"))
            input.X -= 1;

        var speed = 100f;
        var velocity = input.Normalized() * speed;
    
        Position += velocity * (float)delta;
    }
}