using System;
using System.Collections.Generic;

public static class PowerUpManager
{
    private static readonly IList<IPowerUpApplier> _powerUps;

    static PowerUpManager()
    {
        _powerUps = new List<IPowerUpApplier>() {
            new PhotonBoostPowerup(),
            new HealthPowerup()
        };
    }

    public static IPowerUpApplier GetRandomPowerup()
    {
        // Test
        return new PhotonBoostPowerup();

        var random = new Random();
        var randomIndex = random.Next(0, _powerUps.Count);
        return _powerUps[randomIndex];
    }

    public interface IPowerUpApplier
    {
        void Apply(Player player);
        string Name { get; }
    }

    public class PhotonBoostPowerup : IPowerUpApplier
    {
        public void Apply(Player player)
        {
            player.PlayerMovementDelegate.Speed += 100;
            player.PlayerMovementDelegate.NrPossibleJumps += 1;
        }

        public string Name => "Photon Boost";
    }

    public class HealthPowerup : IPowerUpApplier
    {
        public void Apply(Player player)
        {
            player.MaxHealth += 50;
        }

        public string Name => "Health Boost";
    }
}