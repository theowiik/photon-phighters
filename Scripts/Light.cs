using Godot;

public partial class Light : Node2D
{
    public enum LightMode
    {
        On,
        Off,
        Neutral
    }

    private PointLight2D _light;
    private bool _isWhite = true;
    public LightMode LightState { get; private set; }

    public override void _Ready()
    {
        _light = GetNode<PointLight2D>("Light2D");
    }

    public override void _Process(double delta)
    {
        var black = new Color(0, 0, 0);
        var white = new Color(1, 1, 1);

        if (Input.IsActionJustPressed("ui_accept"))
        {
            var newColor = _isWhite ? black : white;
            _isWhite = !_isWhite;
            LightState = _isWhite ? LightMode.On : LightMode.Off;
            _light.Color = newColor;
        }
    }

    public void TurnOn()
    {
        _light.Enabled = true;
    }

    public void TurnOff()
    {
        _light.Enabled = false;
    }
}
