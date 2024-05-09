using Godot;

namespace PhotonPhighters.Scripts;

public partial class MusicPlayer : AudioStreamPlayer
{
  private const float MinPitch = 1f;
  private const float MaxPitch = 1.4f;

  public void SetPitch(int lightWins, int darkWins, int scoreToWin)
  {
    var greatest = Mathf.Max(lightWins, darkWins);
    PitchScale = Mathf.Lerp(MinPitch, MaxPitch, (float)greatest / scoreToWin);
  }
}
