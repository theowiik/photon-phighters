namespace PhotonPhighters.Scripts.PowerUps;

public static partial class PowerUps
{
  /// <summary>
  ///   Stateless power up applier. Creates a new instance of the power up every time it is applied.
  /// </summary>
  public interface IPowerUpApplier
  {
    string Name { get; }
    Rarity Rarity { get; }
    string GetMarkName(Player player);
    void Apply(Player playerWhoSelected, Player otherPlayer);
    bool IsCurse { get; }
  }
}
