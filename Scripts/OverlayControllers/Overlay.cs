using System;
using Godot;
using PhotonPhighters.Scripts.GoSharper.AutoWiring;
using static PhotonPhighters.Scripts.World;

namespace PhotonPhighters.Scripts.OverlayControllers;

public partial class Overlay : Control
{
  [GsAutoWiring("VBox/Logs")]
  private RichTextLabel _logs;

  [GsAutoWiring("VBox/RoundScoreBar")]
  private ProgressBar _roundScoreBar;

  [GsAutoWiring("VBox/RoundScoreLabel")]
  private Label _roundScoreLabel;

  [GsAutoWiring("VBox/RoundTimerLabel")]
  private Label _timerLabel;

  [GsAutoWiring("VBox/TotalScoreLabel")]
  private Label _totalScoreLabel;

  public void SetRoundScore(Results value)
  {
    var pLight = value.Light / (float)(value.Light + value.Dark);
    _roundScoreBar.Value = pLight;
    _roundScoreLabel.Text = $"{Math.Round(pLight * 100)}% - {Math.Round((1 - pLight) * 100)}%";
  }

  public void SetTime(string value)
  {
    _timerLabel.Text = value;
  }

  public void SetTotalScore(string value)
  {
    _totalScoreLabel.Text = value;
  }

  public override void _Ready()
  {
    this.AutoWire();
  }

  private void Log(string msg)
  {
    _logs.Text += msg + "\n";
  }
}
