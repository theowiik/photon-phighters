using System.Linq;

namespace PhotonPhighters.Scripts.GameMode;

public sealed class PhotonPhight : IGameMode
{
  public bool SpawnLights => true;
  public Score GetResults(World world)
  {
    return LightUtils.CountScore(world.GetTree().GetNodesInGroup("lights").Cast<Light>());
  }

  public void RoundStarted(World world)
  {
  }
}
