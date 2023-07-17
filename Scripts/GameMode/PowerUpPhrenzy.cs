using System.Linq;
using PhotonPhighters.Scripts.PowerUps;

namespace PhotonPhighters.Scripts.GameMode;

public sealed class PowerUpPhrenzy : IGameMode
{
  public bool SpawnLights => true;
  public Score GetResults(World world)
  {
    return LightUtils.CountScore(world.GetTree().GetNodesInGroup("lights").Cast<Light>());
  }

  public void RoundStarted(World world)
  {
    var powerUp = PowerUpManager.GetUniquePowerUps(1).First();
    powerUp.Apply(world._lightPlayer, world._darkPlayer);
  }
}
