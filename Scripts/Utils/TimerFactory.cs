using Godot;

namespace PhotonPhighters.Scripts.Utils;

public static class TimerFactory
{
    public static Timer OneShotStartedTimer(double waitTime)
    {
        var timer = new Timer
        {
            Autostart = true,
            OneShot = true,
            WaitTime = waitTime
        };

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