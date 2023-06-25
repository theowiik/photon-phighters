using Godot;
using static PhotonPhighters.Scripts.PowerUps.PowerUps;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public static partial class PowerUps
{
  public class SketchyPillsGood : AbstractPowerUpApplier
  {
    // Makes the player smaller and faster (gamba)
    public override string Name => "Sketchy Pills";
    public override Rarity Rarity => Rarity.Rare;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Scale *= new Vector2(0.5f, 0.5f);
      playerWhoSelected.PlayerMovementDelegate.Speed += 200;
    }
  }
}
