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

  public void Apply(Team selected, Team opponent, IEnumerable<Player> allPlayers)
  {
    _haveTaken.Add(selected);

    var selecters = allPlayers.Where(p => p.Team == selected).Shuffled().ToList();
    var opponents = allPlayers.Where(p => p.Team == opponent).Shuffled().ToList();

    for (var i = 0; i < selecters.Count; i++)
    {
      var playerWhoSelected = selecters[i];
      var otherPlayer = opponents[i % opponents.Count];

      _Apply(playerWhoSelected, otherPlayer);

      var playerToAddEffect = IsCurse ? otherPlayer : playerWhoSelected;
      playerToAddEffect.EffectsDelegate.DisplayPowerUpEffect(this);
    }
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
