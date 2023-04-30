using System;
using System.Collections.Generic;

public static class PowerUpManager
{
    private static readonly IList<IPowerUpApplier> _powerUps;

    static PowerUpManager()
    {
        _powerUps = new List<IPowerUpApplier>() {
            new PhotonBoostPowerup(),
            new HealthPowerup(),
            new BunnyBoostPowerup(),
            new FrictionlessPowerup(),
            new PhotonMultiplier(),
            new PhotonEnlarger(),
            new PhotonAccelerator(),
            new GlassCannon(),
            new GravitationalNeuronBlaster(),
            new PhotonMuncher(),
            new AirWalker(),

        };
    }

    public static IPowerUpApplier GetRandomPowerup()
    {
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
            // player.PlayerMovementDelegate.Speed += 100;
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

    public class BunnyBoostPowerup : IPowerUpApplier
    {
        public void Apply(Player player)
        {
            // player.PlayerMovementDelegate.JumpHeight += 100;
            // player.PlayerMovementDelegate.UpdateMovementVars();
        }

        public string Name => "Bunny Boost";
    }

    public class FrictionlessPowerup : IPowerUpApplier
    {
        public void Apply(Player player)
        {
            // var playerSpeed = player.PlayerMovementDelegate.Speed;
            // player.PlayerMovementDelegate.FrictionAccelerate = playerSpeed;
            // player.PlayerMovementDelegate.FrictionDecelerate = playerSpeed;
        }

        public string Name => "Frictionless movement";
    }

    public class AirWalker : IPowerUpApplier
    {
        public void Apply(Player player)
        {
            // player.PlayerMovementDelegate.JumpHeight += 25;
            // player.PlayerMovementDelegate.NrPossibleJumps += 1;
            // player.PlayerMovementDelegate.UpdateMovementVars();
        }

        public string Name => "Air Walker";
    }

    public class PhotonMuncher : IPowerUpApplier
    {
        public void Apply(Player player)
        {
            player.MaxHealth += 50;
            // player.PlayerMovementDelegate.Speed -= -50.0f;
        }

        public string Name => "Photon Muncher";
    }

    public class GravitationalNeuronBlaster : IPowerUpApplier
    {
        public void Apply(Player player)
        {
            player.Gun.BulletGravity = 0.0f;
        }

        public string Name => "Gravitational Neuron Blaster";
    }

    public class PhotonAccelerator : IPowerUpApplier
    {
        public void Apply(Player player)
        {
            player.Gun.BulletSpeed += 250.0f;
        }

        public string Name => "Photon Accelerator";
    }

    public class PhotonMultiplier : IPowerUpApplier
    {
        public void Apply(Player player)
        {
            player.Gun.BulletCount += 2;
        }

        public string Name => "Photon Multiplier";
    }

    public class PhotonEnlarger : IPowerUpApplier
    {
        public void Apply(Player player)
        {
            player.Gun.BulletSizeFactor += 1;
            player.Gun.BulletDamage += 10;
            player.Gun.BulletSpeed += 100.0f;
        }

        public string Name => "Photon Enlarger";
    }

    public class GlassCannon : IPowerUpApplier
    {
        public void Apply(Player player)
        {
            player.MaxHealth = 1;
            player.Gun.BulletDamage = 100;
        }

        public string Name => "Glass Cannon";
    }
}