using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;

public partial class Light : Area2D
{
    [GetNode("PointLight2D")]
    private PointLight2D _pointLight;

    [GetNode("AnimationPlayer")]
    private AnimationPlayer _animationPlayer;

    public LightMode LightState { get; private set; }

    public override void _Ready()
    {
        this.AutoWire();
        _pointLight.Enabled = false;
        LightState = LightMode.None;
    }

    public void SetLight(LightMode lightMode)
    {
        if (LightState == lightMode)
            return;

        if (lightMode == LightMode.None)
        {
            _pointLight.Enabled = false;
            LightState = LightMode.None;
            return;
        }

        _animationPlayer.Play("pulsate");

        _pointLight.Enabled = true;
        LightState = lightMode == LightMode.Light ? LightMode.Light : LightMode.Dark;
        _pointLight.BlendMode = lightMode == LightMode.Light ? Light2D.BlendModeEnum.Add : Light2D.BlendModeEnum.Sub;

        QueueRedraw(); // TODO: Dev remove
    }

    public enum LightMode
    {
        Light,
        Dark,
        None
    }

    public override void _Draw()
    {
        const bool debugDraw = true;

        if (!debugDraw)
            return;

        var color = Colors.Transparent;

        switch (LightState)
        {
            case LightMode.Light:
                color = Colors.White;
                break;
            case LightMode.Dark:
                color = Colors.Black;
                break;
        }

        DrawCircle(Vector2.Zero, 5, color);
    }
}