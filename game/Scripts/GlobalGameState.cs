using System.Collections.Generic;

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
  ///   A dictionary of players in the game.
  ///   Key is the device id of the player.
  /// </summary>
  public static IDictionary<int, Team> Players { get; } = new Dictionary<int, Team>();
}
