using Godot;

public partial class Light : Node2D
{
    private PointLight2D _light2D;
    public LightMode LightState { get; private set; } = LightMode.Neutral;
    private Color _whiteColor = new(1, 1, 1);
    private Color _blackColor = new(0, 0, 0);

    // Used during development
    private bool _isWhite = false;

    public override void _Ready()
    {
        _light2D = GetNode<PointLight2D>("Light2D");
        _light2D.Enabled = false;
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("ui_accept"))
        {
            _light2D.Enabled = true;
            var newColor = _isWhite ? _blackColor : _whiteColor;
            LightState = _isWhite ? LightMode.On : LightMode.Off;
            _light2D.Color = newColor;
            _isWhite = !_isWhite;
        }
    }

    public void SetLights(bool on)
    {
        _light2D.Enabled = on;
    }

    public enum LightMode
    {
        On,
        Off,
        Neutral
    }
}
