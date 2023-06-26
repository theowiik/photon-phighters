using System;
using Godot;
using static PhotonPhighters.Scripts.PowerUps.PowerUps;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public static partial class PowerUps
{
  public class MiniGun : AbstractPowerUpApplier
  {
    public override string Name => "1 000 000 lumen";
    public override Rarity Rarity => Rarity.Legendary;

    public override string GetMarkName(Player player)
    {
      var timesTaken = TimesTakenBy(player);

      return timesTaken switch
      {
        0 => "",
        >= 1 => BuildMarkName(2),
        _ => throw new ArgumentOutOfRangeException(nameof(player), "This should never happen")
      };
    }

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      var taken = TimesTakenBy(playerWhoSelected);

      switch (taken)
      {
        case 0:
          playerWhoSelected.Gun.BulletCount += 4;
          playerWhoSelected.Gun.BulletDamage = 1;
          playerWhoSelected.Gun.BulletSpread += 0.3f;
          playerWhoSelected.Gun.BulletSpeed /= 1.4f;
          playerWhoSelected.Gun.FireRate += 1.8f;
          break;
        case >= 1:
          playerWhoSelected.Gun.BulletSpread = Mathf.Pi * 2;
          playerWhoSelected.Gun.BulletCount += 10;
          break;
      }
    }
  }
}
