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
      new AirWalker(),
      new GeneratorEngine()
    };
  }

  public interface IPowerUp
  {
    string Name { get; }

    void Apply(Player player);
  }

  public static IEnumerable<IPowerUp> GetAllPowerUps()
  {
    return SPowerUps;
  }

  public static IPowerUp GetRandomPowerup()
  {
    var random = new Random();
    var randomIndex = random.Next(0, SPowerUps.Count);
    return SPowerUps[randomIndex];
  }

  /// <summary>
  ///   Returns a specified number (n) of unique power-ups from a list of available power-ups (_powerUps).
  ///   If n is greater than the total number of unique power-ups, duplicates will be added.
  /// </summary>
  /// <param name="n">The number of power-ups to be returned. Must be greater than or equal to 0.</param>
  /// <returns>An IEnumerable of IPowerUpApplier objects containing the requested number of power-ups.</returns>
  /// <exception cref="ArgumentException">
  ///   Thrown when n is less than 0 or when n is greater than the total number of unique
  ///   power-ups.
  /// </exception>
  public static IEnumerable<IPowerUp> GetUniquePowerUps(int n)
  {
    if (n < 0)
    {
      throw new ArgumentException("n must be greater than or equal to 0");
    }

    var random = new Random();
    var powerUps = new List<IPowerUp>();
    while (powerUps.Count < n)
    {
      var randomIndex = random.Next(0, SPowerUps.Count);
      var powerUp = SPowerUps[randomIndex];

      // Add unique powerup
      if (!powerUps.Contains(powerUp))
      {
        powerUps.Add(powerUp);
      }

      // All unique powerups added, add duplicates
      if (powerUps.Count >= SPowerUps.Count)
      {
        powerUps.Add(powerUp);
      }
    }

    return powerUps.Shuffle();
  }

  private class AirWalker : IPowerUp
  {
    public string Name => "Air Walker";

    public void Apply(Player player)
    {
      player.PlayerMovementDelegate.MaxJumps += 1;
    }
  }

  private class BunnyBoost : IPowerUp
  {
    public string Name => "Bunny Boost";

    public void Apply(Player player)
    {
      player.PlayerMovementDelegate.JumpForce += 300;
    }
  }

  private class GeneratorEngine : IPowerUp
  {
    public string Name => "Generator Engine";

    public void Apply(Player player)
    {
      player.Gun.FireRate -= 0.7f;
      player.Gun.BulletSpread *= 1.05f;
    }
  }

  private class GlassCannon : IPowerUp
  {
    public string Name => "Glass Cannon";

    public void Apply(Player player)
    {
      player.MaxHealth /= 2;
      player.Gun.BulletDamage *= 3;
      player.Gun.BulletSpread *= 1.15f;
    }
  }

  private class Gravitronizer : IPowerUp
  {
    public string Name => "Gravitronizer";

    public void Apply(Player player)
    {
      player.Gun.BulletGravity = 0.0f;
    }
  }

  private class HealthBoost : IPowerUp
  {
    public string Name => "Health Boost";

    public void Apply(Player player)
    {
      player.MaxHealth = (int)(player.MaxHealth * 1.5f);
    }
  }

  private class PhotonAccelerator : IPowerUp
  {
    public string Name => "Photon Accelerator";

    public void Apply(Player player)
    {
      player.Gun.BulletSpeed += 300.0f;
      player.Gun.BulletSpread *= 1.05f;
    }
  }

  private class PhotonBoost : IPowerUp
  {
    public string Name => "Photon Boost";

    public void Apply(Player player)
    {
      player.PlayerMovementDelegate.Speed += 200;
    }
  }

  private class PhotonEnlarger : IPowerUp
  {
    public string Name => "Photon Enlarger";

    public void Apply(Player player)
    {
      player.Gun.BulletSizeFactor += 1.5f;
      player.Gun.BulletDamage += 10;
      player.Gun.BulletSpeed -= 150.0f;
      player.Gun.BulletSpread *= 1.25f;
    }
  }

  private class PhotonMultiplier : IPowerUp
  {
    public string Name => "Photon Multiplier";

    public void Apply(Player player)
    {
      player.Gun.BulletCount = (int)Math.Ceiling(player.Gun.BulletCount * 1.5f);
      player.Gun.BulletSpread *= 1.05f;
    }
  }

  private class PhotonMuncher : IPowerUp
  {
    public string Name => "Photon Muncher";

    public void Apply(Player player)
    {
      player.MaxHealth *= 2;
      player.PlayerMovementDelegate.Speed -= -150.0f;
      player.Gun.BulletSpread *= 1.2f;
    }
  }
}
