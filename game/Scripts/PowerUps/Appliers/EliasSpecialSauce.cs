namespace PhotonPhighters.Scripts.PowerUps.Appliers;

/// <summary>
///   Powerful gun with high damage and fire rate.
/// </summary>
public class EliasSpecialSauce : AbstractPowerUpApplier
{
  public override string Name => "Elias' Special Sauce";
  public override Rarity Rarity => Rarity.Epic;
  public override bool IsCurse => false;

  protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
  {
    playerWhoSelected.Gun.FireRate = 11;
    playerWhoSelected.Gun.BulletDamage = 4;
    playerWhoSelected.Gun.BulletCount = 1;
    playerWhoSelected.Gun.BulletSpread = 0;
  }
}
