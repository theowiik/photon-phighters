using System.Collections.Generic;
using System.Linq;
using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts.PowerUps;

public abstract class AbstractPowerUpApplier : IPowerUpApplier
{
  private readonly IList<Team> _haveTaken = new List<Team>();

  public virtual string GetMarkName(Team team)
  {
    return null;
  }

  public abstract string Name { get; }
  public abstract Rarity Rarity { get; }

  public void Apply(Player playerWhoSelected, Player otherPlayer)
  {
    _Apply(playerWhoSelected, otherPlayer);
    _haveTaken.Add(playerWhoSelected.Team);

    var playerToAddEffect = IsCurse ? otherPlayer : playerWhoSelected;
    playerToAddEffect.EffectsDelegate.DisplayPowerUpEffect(this);
  }

  public abstract bool IsCurse { get; }

  protected int TimesTakenBy(Team team)
  {
    return _haveTaken.Count(t => t == team);
  }

  protected abstract void _Apply(Player playerWhoSelected, Player otherPlayer);

  protected string LazyGetMarkName(int max, Team team)
  {
    var timesTaken = TimesTakenBy(team);
    var mark = Mathf.Min(timesTaken + 1, max);
    return $"MK.{NumberUtil.ToRoman(mark)}";
  }
}
