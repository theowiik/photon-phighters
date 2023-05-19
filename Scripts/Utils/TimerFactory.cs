using System;
using Godot;

namespace PhotonPhighters.Scripts.Utils;

public static class TimerFactory
{
  /// <summary>
  ///   Creates a timer that will self destruct (QueueFree) after the timeout.
  /// </summary>
  /// <param name="waitTime">
  ///   The time to wait before the timer times out.
  /// </param>
  /// <param name="onTimeout">
  ///   The action to perform when the timer times out.
  /// </param>
  /// <returns>
  ///   The timer.
  /// </returns>
  public static Timer OneShotSelfDestructingStartedTimer(double waitTime, Action onTimeout = null)
  {
    var timer = OneShotStartedTimer(waitTime);
    timer.Timeout += () =>
    {
      onTimeout?.Invoke();
      timer.QueueFree();
    };
    return timer;
  }

  /// <summary>
  ///   Creates a timer that performs the given action after the timeout.
  /// </summary>
  /// <param name="waitTime">
  ///   The time to wait before the timer times out.
  /// </param>
  /// <param name="onTimeout">
  ///   The action to perform when the timer times out.
  /// </param>
  /// <returns>
  ///   The timer.
  /// </returns>
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

  /// <summary>
  ///   Creates a looping timer.
  /// </summary>
  /// <param name="timeBetweenCapturePoint">
  ///   The time between capture points.
  /// </param>
  /// <returns>
  ///   The timer.
  /// </returns>
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
