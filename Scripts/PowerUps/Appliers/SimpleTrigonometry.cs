using PhotonPhighters.Scripts.Events;
using static PhotonPhighters.Scripts.PowerUps.PowerUps;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public static partial class PowerUps
{
  public class SimpleTrigonometry : AbstractPowerUpApplier
  {
    // Photons move toward the other player
    public override string Name => "Simple Trigonometry";
    public override Rarity Rarity => Rarity.Rare;
    public override bool IsCurse => false;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      new StatefulSimpleTrigonometry().Apply(playerWhoSelected, otherPlayer);
    }

    private class StatefulSimpleTrigonometry
    {
      private Player OtherPlayer { get; set; }

      public void Apply(Player playerWhoSelected, Player otherPlayer)
      {
        playerWhoSelected.Gun.BulletSpeed /= 1.5f;
        OtherPlayer = otherPlayer;
        playerWhoSelected.Gun.BulletFlying += MoveToOtherPlayer;
      }

      private void MoveToOtherPlayer(BulletEvent bulletEvent)
      {
        var vector = OtherPlayer.Position - bulletEvent.Area2D.Position;
        const int AttractionStrenth = 20;
        bulletEvent.Velocity += vector.Normalized() * AttractionStrenth;
      }
    }
  }
}
