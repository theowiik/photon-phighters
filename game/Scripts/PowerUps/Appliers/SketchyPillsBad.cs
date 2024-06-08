using Godot;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

/// <summary>
///   Makes the player bigger and slower (gamba).
/// </summary>
public class SketchyPillsBad : AbstractPowerUpApplier
{
  //
  public override string Name => "Sketchy Pills";
  public override Rarity Rarity => Rarity.Rare;
  public override bool IsCurse => false;

  protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
  {
    playerWhoSelected.Scale *= new Vector2(1.25f, 1.25f);
    playerWhoSelected.PlayerMovementDelegate.Speed -= 100;
  }
}
