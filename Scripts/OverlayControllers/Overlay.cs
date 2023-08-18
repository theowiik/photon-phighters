using System;
using Godot;
using PhotonPhighters.Scripts.GoSharper.AutoWiring;

namespace PhotonPhighters.Scripts.OverlayControllers;

public partial class Overlay : Control
{
  [GetNode("VBox/Logs")]
  private RichTextLabel _logs;

  [GetNode("VBox/RoundScoreBar")]
  private ProgressBar _roundScoreBar;

  [GetNode("VBox/RoundScoreLabel")]
  private Label _roundScoreLabel;

  [GetNode("VBox/RoundTimerLabel")]
  private Label _timerLabel;

  [GetNode("VBox/TotalScoreLabel")]
  private Label _totalScoreLabel;

  public void SetRoundScore(Score value)
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
