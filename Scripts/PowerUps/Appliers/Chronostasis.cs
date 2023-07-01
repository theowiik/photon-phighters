using Godot;
using PhotonPhighters.Scripts.Events;
using static PhotonPhighters.Scripts.PowerUps.PowerUps;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public static partial class PowerUps
{
  public class Chronostasis : AbstractPowerUpApplier
  {
    // Photons briefly freeze the opponent
    public override string Name => "Chronostasis";
    public override Rarity Rarity => Rarity.Epic;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      new StatefulChronostasis().Apply(otherPlayer);
    }

    private class StatefulChronostasis
    {
      private ulong _msecSinceLastFreeze;

      public void Apply(Player otherPlayer)
      {
        otherPlayer.PlayerHurt += RecordTimeSinceFreeze;
        otherPlayer.PlayerMovementDelegate.PlayerMove += FreezePlayer;
      }

      private void FreezePlayer(PlayerMovementEvent movementEvent)
      {
        var currentTimeMsec = Time.GetTicksMsec();
        if (currentTimeMsec - _msecSinceLastFreeze >= 400)
        {
          return;
        }

        movementEvent.CanMove = false;
        movementEvent.CanJump = false;
      }

      private void RecordTimeSinceFreeze(Player player, int damage, PlayerHurtEvent playerHurtEvent)
      {
        _msecSinceLastFreeze = Time.GetTicksMsec();
      }
    }
  }
}
