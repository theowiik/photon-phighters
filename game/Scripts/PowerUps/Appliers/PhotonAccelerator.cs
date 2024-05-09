using static PhotonPhighters.Scripts.PowerUps.PowerUps;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public static partial class PowerUps
{
  public class PhotonAccelerator : AbstractPowerUpApplier
  {
    public override string Name => "Photon Accelerator";
    public override Rarity Rarity => Rarity.Common;
    public override bool IsCurse => false;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.BulletSpeed += 300.0f;
      playerWhoSelected.Gun.BulletSpread *= 1.05f;
    }
  }
}
