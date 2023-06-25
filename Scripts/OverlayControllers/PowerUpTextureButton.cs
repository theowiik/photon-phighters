using Godot;
using PhotonPhighters.Scripts.GoSharper.AutoWiring;
using PhotonPhighters.Scripts.GoSharper.Instancing;

namespace PhotonPhighters.Scripts.OverlayControllers;

[GsInstancer("res://UI/PowerUpTextureButton.tscn")]
public partial class PowerUpTextureButton : TextureButton
{
  [GsAutoWiring("RichTextLabel")]
  private RichTextLabel _richTextLabel;

  public override void _Ready()
  {
    this.AutoWire();
    ApplyShader();
  }

  private void ApplyShader()
  {
    var shader = GD.Load<Shader>("res://Assets/Shaders/shine.gdshader");
    Material = new ShaderMaterial { Shader = shader };
  }

  public void SetLabel(string text)
  {
    _richTextLabel.Text = $"[center]{text}[/center]";
  }
}
