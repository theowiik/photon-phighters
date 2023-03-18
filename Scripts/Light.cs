using Godot;

public partial class Light : Area2D
{
    private PointLight2D _pointLight;
    private AnimationPlayer _animationPlayer;
    public LightMode LightState { get; private set; }

    public override void _Ready()
    {
        _pointLight = GetNode<PointLight2D>("PointLight2D");
        _pointLight.Enabled = false;
        LightState = LightMode.None;
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }

    public void SetLight(LightMode lightMode)
    {
        _animationPlayer.Play("pulsate");

        _pointLight.Enabled = true;
        LightState = lightMode == LightMode.Light ? LightMode.Light : LightMode.Dark;
        _pointLight.BlendMode = lightMode == LightMode.Light ? Light2D.BlendModeEnum.Add : Light2D.BlendModeEnum.Sub;
    }

    public enum LightMode
    {
        Light,
        Dark,
        None
    }
}
