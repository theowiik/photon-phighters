using Godot;
using PhotonPhighters.Scripts.Events;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public class WallSpider : AbstractPowerUpApplier
{
  // Wall-jumping briefly increases movement speed
  public override string Name => "Wall Spider";
  public override Rarity Rarity => Rarity.Common;
  public override bool IsCurse => false;

  protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
  {
    new StatefulWallSpider().Apply(playerWhoSelected);
  }

  private class StatefulWallSpider
  {
    private ulong _msecSinceLastWallJump;

    private void GiveSpeedBoost(PlayerMovementEvent playerMovementEvent)
    {
      var currentTimeMsec = Time.GetTicksMsec();
      if (currentTimeMsec - _msecSinceLastWallJump < 6000)
      {
        playerMovementEvent.Speed *= 1.5f;
      }
    }

    private void RecordTimeSinceWallJump(PlayerMovementEvent playerMovementEvent)
    {
      _msecSinceLastWallJump = Time.GetTicksMsec();
    }

    public void Apply(Player playerWhoSelected)
    {
      playerWhoSelected.PlayerMovementDelegate.PlayerMove += GiveSpeedBoost;
      playerWhoSelected.PlayerMovementDelegate.PlayerWallJump += RecordTimeSinceWallJump;
    }
  }
}
