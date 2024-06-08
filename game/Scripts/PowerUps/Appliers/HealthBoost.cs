namespace PhotonPhighters.Scripts.PowerUps.Appliers;

/// <summary>
///   Increases the player's health by 50%.
/// </summary>
public class HealthBoost : AbstractPowerUpApplier
{
  public override string Name => "Health Boost";
  public override Rarity Rarity => Rarity.Common;
  public override bool IsCurse => false;

  protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
  {
    playerWhoSelected.MaxHealth = (int)(playerWhoSelected.MaxHealth * 1.5f);
  }
}
