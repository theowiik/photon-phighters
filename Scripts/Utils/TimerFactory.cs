namespace PhotonPhighters.Scripts.Utils;
using Godot;

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
}
