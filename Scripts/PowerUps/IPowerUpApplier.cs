namespace PhotonPhighters.Scripts.PowerUps;

/// <summary>
///   Stateless power up applier. Creates a new instance of the power up every time it is applied.
/// </summary>
public interface IPowerUpApplier
{
  string Name { get; }
  PowerUps.Rarity Rarity { get; }
  string GetMarkName(Player player);
  void Apply(Player playerWhoSelected, Player otherPlayer);
  bool IsCurse { get; }
}
