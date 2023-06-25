using Godot;
using PhotonPhighters.Scripts.GoSharper.AutoWiring;
using PhotonPhighters.Scripts.GoSharper.Instancing;
using PhotonPhighters.Scripts.Utils.ResourceWrapper;

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
    Material = new ShaderMaterial { Shader = ShaderResourceWrapper.ShineShader };
  }

  public void SetLabel(string text)
  {
    _richTextLabel.Text = $"[center]{text}[/center]";
  }
}
