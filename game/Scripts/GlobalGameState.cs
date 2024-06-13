using System;
using System.Collections.Generic;
using PhotonPhighters.Scripts.GameModes;

namespace PhotonPhighters.Scripts;

/// <summary>
///   A static class to hold global game state.
///   This should be used as sparingly as possible.
///   Used for managing states between scenes.
/// </summary>
public static class GlobalGameState
{
  public static int RoundsToWin { get; set; } = -1;
  public static int RoundTime { get; set; } = -1;

  /// <summary>
  ///   A list of players in the game.
  ///   (Device ID, PlayerValue)
  /// </summary>
  public static IList<Tuple<int, PlayerValues>> Players { get; } = new List<Tuple<int, PlayerValues>>();

  public static GameMode GameMode { get; set; }

  public struct PlayerValues
  {
    public Team Team { get; }
    public string Name { get; }

    public PlayerValues(Team team, string name)
    {
      Team = team;
      Name = name;
    }
  }
}
