using System.Linq;
using PhotonPhighters.Scripts.PowerUps;

namespace PhotonPhighters.Scripts.GameMode;

public sealed class PowerUpPhrenzyDelegate
{
  public void ApplyPowerUp(Player player1, Player player2)
  {
    var powerUp = PowerUpManager.GetUniquePowerUps(1).First();
    powerUp.Apply(player1, player2);
  }
}
