using System.Collections.Generic;
using System.Linq;
using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts.PowerUps;

public static partial class PowerUps
{
  public abstract class AbstractPowerUpApplier : IPowerUpApplier
  {
    private readonly IList<Player> _haveTaken = new List<Player>();

    public virtual string GetMarkName(Player player)
    {
      return null;
    }

    public abstract string Name { get; }
    public abstract Rarity Rarity { get; }

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      _Apply(playerWhoSelected, otherPlayer);
      _haveTaken.Add(playerWhoSelected);

      if (IsCurse)
      {
        otherPlayer.EffectsDelegate.DisplayPowerUpEffect(this);
      }
      else
      {
        playerWhoSelected.EffectsDelegate.DisplayPowerUpEffect(this);
      }
    }

    public abstract bool IsCurse { get; }

    protected int TimesTakenBy(Player player)
    {
      return _haveTaken.Count(p => p == player);
    }

    protected abstract void _Apply(Player playerWhoSelected, Player otherPlayer);

    protected string LazyGetMarkName(int max, Player player)
    {
      var timesTaken = TimesTakenBy(player);
      var mark = Mathf.Min(timesTaken + 1, max);
      return $"MK.{NumberUtil.ToRoman(mark)}";
    }
  }
}
