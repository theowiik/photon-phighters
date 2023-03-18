using Godot;

public partial class Light : Area2D
{
    private PointLight2D _light2D;
    public LightMode LightState { get; private set; }
    private Color _lightColor = new(1, 1, 1);
    private Color _darkColor = new(1, 1, 0);

    public override void _Ready()
    {
        _light2D = GetNode<PointLight2D>("Light2D");
        _light2D.Enabled = false;
        LightState = LightMode.None;
    }

    public void SetLight(LightMode lightMode)
    {
        _light2D.Enabled = true;
        _light2D.Color = lightMode == LightMode.Light ? _lightColor : _darkColor;
        LightState = lightMode == LightMode.Light ? LightMode.Light : LightMode.Dark;
    }

    public enum LightMode
    {
        Light,
        Dark,
        None
    }
}
