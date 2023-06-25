namespace PhotonPhighters.Scripts.PowerUps;

public static partial class PowerUps
{
  /// <summary>
  ///   Different power up rarities, and their percentage chance of being selected.
  ///   Must add up to exactly 100.
  /// </summary>
  public enum Rarity
  {
    Legendary = 2,
    Epic = 8,
    Rare = 25,
    Common = 65
  }
}
