using static PhotonPhighters.Scripts.PowerUps.PowerUps;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public static partial class PowerUps
{
  public class FakeJordans : AbstractPowerUpApplier
  {
    // Messes with the opponents movement
    public override string Name => "Fake Jordans";
    public override Rarity Rarity => Rarity.Rare;
    public override bool IsCurse => true;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      otherPlayer.PlayerMovementDelegate.Speed -= 75.0f;
      otherPlayer.PlayerMovementDelegate.JumpForce -= 100.0f;
    }
  }
}
