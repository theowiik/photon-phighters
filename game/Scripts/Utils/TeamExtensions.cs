using System.Collections.Generic;
using Godot;

namespace PhotonPhighters.Scripts.Utils;

public static class TeamExtensions
{
  private static readonly IDictionary<Team, Color> s_colors = new Dictionary<Team, Color>
  {
    {Team.Dark, Colors.DarkSlateGray},
    {Team.Light, Colors.White}
  };
  
  public static void Color(this Sprite2D sprite2D, Team color)
  {
    if (!s_colors.TryGetValue(color, out var value))
    {
      throw new KeyNotFoundException($"Color for team {color} not found.");
    }

    sprite2D.Modulate = value;
  }
}
