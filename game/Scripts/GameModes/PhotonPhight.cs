using System.Linq;

namespace PhotonPhighters.Scripts.GameModes;

/// <summary>
///   A game mode where the team with the most lights wins.
/// </summary>
public class PhotonPhight : IGameMode
{
  public bool SpawnLights => true;

  public Score GetResults(World world)
  {
    return LightUtils.CountScore(world.GetTree().GetNodesInGroup("lights").Cast<Light>());
  }

  public void RoundStarted(World world) { }
}
