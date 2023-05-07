using System;
using System.Collections.Generic;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;

public static class PowerUpManager
{
    private static readonly IList<IPowerUp> SPowerUps;

    static PowerUpManager()
    {
        SPowerUps = new List<IPowerUp>
        {
            new PhotonBoost(),
            new HealthBoost(),
            new BunnyBoost(),
            new PhotonMultiplier(),
            new PhotonEnlarger(),
            new PhotonAccelerator(),
            new GlassCannon(),
            new Gravitronizer(),
            new PhotonMuncher(),
            new AirWalker()
        };
    }

    public static IPowerUp GetRandomPowerup()
    {
        var random = new Random();
        var randomIndex = random.Next(0, SPowerUps.Count);
        return SPowerUps[randomIndex];
    }

    /// <summary>
    ///     Returns a specified number (n) of unique power-ups from a list of available power-ups (_powerUps).
    ///     If n is greater than the total number of unique power-ups, duplicates will be added.
    /// </summary>
    /// <param name="n">The number of power-ups to be returned. Must be greater than or equal to 0.</param>
    /// <returns>An IEnumerable of IPowerUpApplier objects containing the requested number of power-ups.</returns>
    /// <exception cref="ArgumentException">
    ///     Thrown when n is less than 0 or when n is greater than the total number of unique
    ///     power-ups.
    /// </exception>
    public static IEnumerable<IPowerUp> GetUniquePowerUps(int n)
    {
        if (n < 0) throw new ArgumentException("n must be greater than or equal to 0");

        var random = new Random();
        var powerUps = new List<IPowerUp>();
        while (powerUps.Count < n)
        {
            var randomIndex = random.Next(0, SPowerUps.Count);
            var powerUp = SPowerUps[randomIndex];

            // Add unique powerup
            if (!powerUps.Contains(powerUp)) powerUps.Add(powerUp);

            // All unique powerups added, add duplicates
            if (powerUps.Count >= SPowerUps.Count) powerUps.Add(powerUp);
        }

        return powerUps.Shuffle();
    }

    public interface IPowerUp
    {
        string Name { get; }
        void Apply(Player player);
    }

    private class PhotonBoost : IPowerUp
    {
        public void Apply(Player player)
        {
            player.PlayerMovementDelegate.Speed += 100;
        }

        public string Name => "Photon Boost";
    }

    private class HealthBoost : IPowerUp
    {
        public void Apply(Player player)
        {
            player.MaxHealth += 50;
        }

        public string Name => "Health Boost";
    }

    private class BunnyBoost : IPowerUp
    {
        public void Apply(Player player)
        {
            player.PlayerMovementDelegate.JumpForce += 300;
        }

        public string Name => "Bunny Boost";
    }

    private class AirWalker : IPowerUp
    {
        public void Apply(Player player)
        {
            player.PlayerMovementDelegate.JumpForce += 200;
            player.PlayerMovementDelegate.MaxJumps += 1;
        }

        public string Name => "Air Walker";
    }

    private class PhotonMuncher : IPowerUp
    {
        public void Apply(Player player)
        {
            player.MaxHealth += 50;
            player.PlayerMovementDelegate.Speed -= -100.0f;
        }

        public string Name => "Photon Muncher";
    }

    private class Gravitronizer : IPowerUp
    {
        public void Apply(Player player)
        {
            player.Gun.BulletGravity = 0.0f;
        }

        public string Name => "Gravitronizer";
    }

    private class PhotonAccelerator : IPowerUp
    {
        public void Apply(Player player)
        {
            player.Gun.BulletSpeed += 250.0f;
        }

        public string Name => "Photon Accelerator";
    }

    private class PhotonMultiplier : IPowerUp
    {
        public void Apply(Player player)
        {
            player.Gun.BulletCount += 2;
        }

        public string Name => "Photon Multiplier";
    }

    private class PhotonEnlarger : IPowerUp
    {
        public void Apply(Player player)
        {
            player.Gun.BulletSizeFactor += 1;
            player.Gun.BulletDamage += 10;
            player.Gun.BulletSpeed += 100.0f;
        }

        public string Name => "Photon Enlarger";
    }

    private class GlassCannon : IPowerUp
    {
        public void Apply(Player player)
        {
            player.MaxHealth = 1;
            player.Gun.BulletDamage = 100;
        }

        public string Name => "Glass Cannon";
    }
}