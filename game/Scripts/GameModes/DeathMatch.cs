namespace PhotonPhighters.Scripts.GameModes;

/// <summary>
///   A game mode where the team with the most kills wins.
/// </summary>
public sealed class DeathMatch : IGameMode
{
  public bool SpawnLights => false;

  public Score GetResults(World world)
  {
    return new Score
    {
      Light = world.RoundState.DarkDeaths,
      Dark = world.RoundState.LightDeaths,
      Ties = 0
    };
  }

  public void RoundStarted(World world) { }
}
