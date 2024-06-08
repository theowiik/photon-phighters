namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public class MoonwalkerCurse : AbstractPowerUpApplier
{
  public override string Name => "Moonwalker Curse";
  public override Rarity Rarity => Rarity.Legendary;
  public override bool IsCurse => true;

  protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
  {
    // Reverse gravity and jump force, but slightly increase jump force
    otherPlayer.PlayerMovementDelegate.Gravity *= -1f;
    otherPlayer.PlayerMovementDelegate.JumpForce /= -2;
    otherPlayer.PlayerMovementDelegate.MaxJumps = 9999999;
    otherPlayer.Gun.BulletGravity *= -1f;
    otherPlayer.FlipSprite();
  }
}
