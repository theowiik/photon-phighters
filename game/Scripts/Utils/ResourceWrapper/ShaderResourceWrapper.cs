using Godot;
using GodotSharper;

namespace PhotonPhighters.Scripts.Utils.ResourceWrapper;

public static class ShaderResourceWrapper
{
  public static Shader RainbowShader => GDX.LoadOrFail<Shader>("res://Assets/Shaders/rainbow.gdshader");
  public static Shader ShineShader => GDX.LoadOrFail<Shader>("res://Assets/Shaders/shine.gdshader");
}
