namespace PhotonPhighters.Scripts.PowerUps;

/// <summary>
///   Stateless power up applier. Creates a new instance of the power up every time it is applied.
/// </summary>
public interface IPowerUpApplier
{
  string Name { get; }
  Rarity Rarity { get; }
  bool IsCurse { get; }

  /// <summary>
  ///   Get the mark name for the power up.
  ///   A mark is a an upgrade of the power up.
  ///   For example, a power up that can be taken 3 times will be mark 3.
  /// </summary>
  /// <param name="team">The team.</param>
  /// <returns>The mark name.</returns>
  string GetMarkName(Team team);

  void Apply(Player playerWhoSelected, Player otherPlayer);
}
