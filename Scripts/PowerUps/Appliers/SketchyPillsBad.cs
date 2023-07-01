using Godot;
using static PhotonPhighters.Scripts.PowerUps.PowerUps;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

public static partial class PowerUps
{
  public class SketchyPillsBad : AbstractPowerUpApplier
  {
    // Makes the player bigger and slower (gamba)
    public override string Name => "Sketchy Pills";
    public override Rarity Rarity => Rarity.Rare;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Scale *= new Vector2(1.25f, 1.25f);
      playerWhoSelected.PlayerMovementDelegate.Speed -= 100;
    }
  }
}
