using Godot;
using PhotonPhighters.Scripts.GoSharper;

namespace PhotonPhighters.Scripts.Utils.ResourceWrapper;

public static class ShaderResourceWrapper
{
  public static Shader RainbowShader => Gs.LoadOrExplode<Shader>("res://Assets/Shaders/rainbow.gdshader");
  public static Shader ShineShader => Gs.LoadOrExplode<Shader>("res://Assets/Shaders/shine.gdshader");
}
