using static PhotonPhighters.Scripts.PowerUps.PowerUps;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public static partial class PowerUps
{
  public class StickyThickyCurse : AbstractPowerUpApplier
  {
    public override string Name => "Sticky Thicky Curse";
    public override Rarity Rarity => Rarity.Legendary;
    public override bool IsCurse => true;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      otherPlayer.PlayerMovementDelegate.Speed /= 2;
      otherPlayer.PlayerMovementDelegate.Acceleration /= 3;
      otherPlayer.PlayerMovementDelegate.JumpForce /= 2;
      otherPlayer.MaxHealth += 50; // TODO: Possibly make it relative to the player's max health
    }
  }
}
