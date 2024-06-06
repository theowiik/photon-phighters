using System;
using Godot;
using GodotSharper.AutoGetNode;

namespace PhotonPhighters.Scripts.OverlayControllers;

public partial class Overlay : Control
{
  [GetNode("VBox/Logs")]
  private RichTextLabel _logs;

  [GetNode("VBox/RoundScoreBar")]
  private ProgressBar _roundScoreBar;

  [GetNode("VBox/RoundTimerLabel")]
  private Label _timerLabel;

  [GetNode("VBox/TotalScoreLabel")]
  private Label _totalScoreLabel;

  public void SetRoundScore(Score value)
  {
    if (value.Light + value.Dark == 0)
    {
      _roundScoreBar.Value = 0.5f;
      return;
    }

    var pLight = value.Light / (float)(value.Light + value.Dark);
    _roundScoreBar.Value = pLight;
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
    this.GetNodes();
  }

  private void Log(string msg)
  {
    _logs.Text += msg + "\n";
  }
}
