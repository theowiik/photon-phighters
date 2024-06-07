using Godot;
using GodotSharper.AutoGetNode;
using GodotSharper.Instancing;
using PhotonPhighters.Scripts.Utils.ResourceWrapper;

namespace PhotonPhighters.Scripts.OverlayControllers;

[Scene("res://UI/PowerUpTextureButton.tscn")]
public partial class PowerUpTextureButton : TextureButton
{
  [GetNode("MarkLabel")]
  private RichTextLabel _markLabel;

  [GetNode("RichTextLabel")]
  private RichTextLabel _powerUpNameLabel;

  public override void _Ready()
  {
    this.GetNodes();
    ApplyShader();
  }

  private void ApplyShader()
  {
    Material = new ShaderMaterial { Shader = ShaderResourceWrapper.ShineShader };
  }

  public void SetPowerUpName(string text)
  {
    if (_powerUpNameLabel == null)
    {
      return;
    }

    _powerUpNameLabel.Text = $"[center]{text}[/center]";
  }

  public void SetMark(string text)
  {
    _markLabel.Text = $"[center][i]{text}[/i][/center]";
  }
}
