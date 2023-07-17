namespace PhotonPhighters.Scripts.GameMode;

public interface IGameMode
{
  public bool SpawnLights { get; }

  Score GetResults(World world);
  void RoundStarted(World world);
}
