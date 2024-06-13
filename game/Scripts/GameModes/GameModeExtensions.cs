using System;
using PhotonPhighters.Scripts.GameModes;

public static class GameModeExtensions
{
  public static string Title(this GameMode gameMode)
  {
    return gameMode switch
    {
      GameMode.PhotonPhight => "Photon Phight",
      GameMode.DeathMatch => "Death Match",
      GameMode.BotBrawl => "Bot Brawl",
      _ => throw new ArgumentOutOfRangeException(nameof(gameMode), gameMode, null)
    };
  }
}
