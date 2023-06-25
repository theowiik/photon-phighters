// Licensed to the.NET Foundation under one or more agreements.
// The.NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
    int TimesTakenBy(Player player);
    void Apply(Player playerWhoSelected, Player otherPlayer);
  }
}
