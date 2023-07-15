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
  public static GameModes GameMode { get; set; } = GameModes.PhotonPhight;

  public enum GameModes
  {
    PhotonPhight,
    Deathmatch,
    PowerUpPhrenzy,
  }
}
