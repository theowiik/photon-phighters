using System.Collections.Generic;
using System.Linq;

namespace PhotonPhighters.Scripts.PowerUps;

public static partial class PowerUps
{
  public abstract class AbstractPowerUpApplier : IPowerUpApplier
  {
    private readonly IList<Player> _haveTaken = new List<Player>();

    public int TimesTakenBy(Player player)
    {
      return _haveTaken.Count(p => p == player);
    }

    public abstract string Name { get; }
    public abstract Rarity Rarity { get; }

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      _haveTaken.Add(playerWhoSelected);
      _Apply(playerWhoSelected, otherPlayer);
    }

    protected abstract void _Apply(Player playerWhoSelected, Player otherPlayer);
  }
}
