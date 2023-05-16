using System;
using Godot;

namespace PhotonPhighters.Scripts.Utils;

public static class TimerFactory
{
  public static Timer OneShotStartedTimer(double waitTime, Action onTimeout = null)
  {
    var timer = new Timer
    {
      Autostart = true,
      OneShot = true,
      WaitTime = waitTime
    };

    if (onTimeout != null)
    {
      timer.Timeout += onTimeout;
    }

    return timer;
  }

  public static Timer StartedTimer(int timeBetweenCapturePoint)
  {
    var timer = new Timer
    {
      Autostart = true,
      OneShot = false,
      WaitTime = timeBetweenCapturePoint
    };

    return timer;
  }
}
