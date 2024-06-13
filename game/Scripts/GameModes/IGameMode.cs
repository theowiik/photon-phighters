namespace PhotonPhighters.Scripts.GameModes;

public interface IGameMode
{
  public bool SpawnLights { get; }

  Score GetResults(World world);
  void RoundStarted(World world);
}
