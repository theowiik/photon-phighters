using Godot;
using static PhotonPhighters.Scripts.PowerUps.PowerUps;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public static partial class PowerUps
{
  public class PhotonMuncher : AbstractPowerUpApplier
  {
    public override string Name => "Mega Photon Muncher";
    public override Rarity Rarity => Rarity.Rare;
    public override bool IsCurse => false;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.MaxHealth = Mathf.RoundToInt(playerWhoSelected.MaxHealth * 1.42f);
      playerWhoSelected.PlayerMovementDelegate.Speed -= -200.0f;
      playerWhoSelected.Gun.BulletSpread *= 1.2f;
    }
  }
}
