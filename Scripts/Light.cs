using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;

public partial class Light : Area2D
{
    [GetNode("LightSprite")]
    private Sprite2D _lightSprite;

    [GetNode("AnimationPlayer")]
    private AnimationPlayer _animationPlayer;

    public LightMode LightState { get; private set; }
    private readonly Color _lightColorModulate = new Color(1, 1, 1, 0.5f);
    private readonly Color _darkColorModulate = new Color(0, 0, 0, 0.5f);

    public override void _Ready()
    {
        this.AutoWire();
        _lightSprite.Visible = false;
        LightState = LightMode.None;
    }

    public void SetLight(LightMode lightMode)
    {
        if (LightState == lightMode)
            return;

        if (lightMode == LightMode.None)
        {
            _lightSprite.Visible = false;
            LightState = LightMode.None;
            return;
        }

        // _animationPlayer.Play("pulsate");
        LightState = lightMode;
        _lightSprite.Visible = true;
        _lightSprite.Modulate = LightState == LightMode.Light ? _lightColorModulate : _darkColorModulate;

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
        const bool debugDraw = false;

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