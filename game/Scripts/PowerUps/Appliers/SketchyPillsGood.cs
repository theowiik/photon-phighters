using Godot;

namespace PhotonPhighters.Scripts.PowerUps.Appliers;

/// <summary>
///   Makes the player smaller and faster (gamba).
/// </summary>
public class SketchyPillsGood : AbstractPowerUpApplier
{
  public override string Name => "Sketchy Pills";
  public override Rarity Rarity => Rarity.Rare;
  public override bool IsCurse => false;

  protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
  {
    playerWhoSelected.Scale *= new Vector2(0.5f, 0.5f);
    playerWhoSelected.PlayerMovementDelegate.Speed += 200;
  }
}
