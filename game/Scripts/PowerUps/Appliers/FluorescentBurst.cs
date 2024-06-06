using Godot;
using PhotonPhighters.Scripts.Events;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

/// <summary>
///   Getting hurt briefly increases movement speed.
/// </summary>
public class FluorescentBurst : AbstractPowerUpApplier
{
  public override string Name => "Fluorescent Burst";
  public override Rarity Rarity => Rarity.Common;
  public override bool IsCurse => false;

  protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
  {
    new StatefulFluorescentBurst().Apply(playerWhoSelected);
  }

  private class StatefulFluorescentBurst
  {
    private ulong _msecSinceLastHurt;

    public void Apply(Player playerWhoSelected)
    {
      playerWhoSelected.PlayerMovementDelegate.PlayerMove += GiveSpeedBoost;
      playerWhoSelected.PlayerHurt += RecordTimeSinceHurt;
    }

    private void GiveSpeedBoost(PlayerMovementEvent playerMovementEvent)
    {
      var currentTimeMsec = Time.GetTicksMsec();
      if (currentTimeMsec - _msecSinceLastHurt < 2000)
      {
        playerMovementEvent.Speed *= 1.5f;
      }
    }

    private void RecordTimeSinceHurt(Player player, int damage, PlayerHurtEvent playerHurtEvent)
    {
      _msecSinceLastHurt = Time.GetTicksMsec();
    }
  }
}
