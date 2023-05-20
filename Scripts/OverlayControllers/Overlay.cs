using System;
using Godot;
using PhotonPhighters.Scripts.Utils;
using static PhotonPhighters.Scripts.World;

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

  [GetNode("VBox/RoundScoreLabel")]
  private Label _roundScoreLabel;

  public Results RoundScore
  {
    set
    {
      var pLight = value.Light / (float)(value.Light + value.Dark);
      _roundScoreBar.Value = pLight;
      _roundScoreLabel.Text = $"{Math.Round(pLight * 100)}% - {Math.Round((1 - pLight) * 100)}%";
    }
  }

  public string Time
  {
    set => _timerLabel.Text = value;
  }

  public string TotalScore
  {
    set => _totalScoreLabel.Text = value;
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
