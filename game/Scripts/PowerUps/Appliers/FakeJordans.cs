namespace PhotonPhighters.Scripts.PowerUps.Appliers;

/// <summary>
///   Curse messing with the opponents movement.
/// </summary>
public class FakeJordans : AbstractPowerUpApplier
{
  public override string Name => "Fake Jordans";
  public override Rarity Rarity => Rarity.Rare;
  public override bool IsCurse => true;

  protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
  {
    otherPlayer.PlayerMovementDelegate.Speed -= 75.0f;
    otherPlayer.PlayerMovementDelegate.JumpForce -= 100.0f;
  }
}
