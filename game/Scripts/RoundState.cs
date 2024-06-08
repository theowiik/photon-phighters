namespace PhotonPhighters.Scripts;

public sealed class RoundState
{
  public int LightDeaths { get; set; }
  public int DarkDeaths { get; set; }
  public int RoundsToWin { get; set; } = 10;
  public int RoundTime { get; set; } = 40;

  public void Reset()
  {
    LightDeaths = 0;
    DarkDeaths = 0;
  }

  public void IncrementDeathForTeam(Team team)
  {
    if (team == Team.Light)
    {
      LightDeaths++;
    }
    else
    {
      DarkDeaths++;
    }
  }
}
