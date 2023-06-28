using System;
using Godot;
using PhotonPhighters.Scripts.Events;
using static PhotonPhighters.Scripts.PowerUps.PowerUps;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public static partial class PowerUps
{
  public class BrownianMotionCurse : AbstractPowerUpApplier
  {
    // Technically stateful but does not deserve its own class
    private readonly Random _rnd = new();

    // Opponent's photons move erratically
    public override string Name => "Brownian Motion Curse";
    public override Rarity Rarity => Rarity.Rare;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      otherPlayer.Gun.BulletFlying += RandomizeDirection;
    }

    private void RandomizeDirection(BulletEvent bulletFlyingEvent)
    {
      bulletFlyingEvent.Velocity += new Vector2(_rnd.Next(-150, 150), _rnd.Next(-150, 150));
    }
  }
}
