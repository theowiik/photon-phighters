using Godot;
using PhotonPhighters.Scripts.GoSharper.AutoWiring;
using PhotonPhighters.Scripts.GoSharper.Instancing;
using PhotonPhighters.Scripts.Utils.ResourceWrapper;

namespace PhotonPhighters.Scripts.OverlayControllers;

[Instantiable("res://UI/PowerUpTextureButton.tscn")]
public partial class PowerUpTextureButton : TextureButton
{
  [GetNode("MarkLabel")]
  private RichTextLabel _markLabel;

  [GetNode("RichTextLabel")]
  private RichTextLabel _powerUpNameLabel;

  public override void _Ready()
  {
    this.AutoWire();
    ApplyShader();
  }

  private void ApplyShader()
  {
    Material = new ShaderMaterial { Shader = ShaderResourceWrapper.ShineShader };
  }

  public void SetPowerUpName(string text)
  {
    _powerUpNameLabel.Text = $"[center]{text}[/center]";
  }

  public void SetMark(string text)
  {
    GD.Print("hello_" + text);
    _markLabel.Text = $"[center][i]{text}[/i][/center]";
  }
}
