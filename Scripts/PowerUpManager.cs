using System;
using System.Collections.Generic;
using System.Linq;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;

public static class PowerUpManager
{

  public enum Rarity
  {
    Common = 4,
    Rare = 2,
    Legendary = 1
  }

  private static readonly IList<IPowerUp> s_allPowerUps;

  // List of power ups, with duplicates to represent the rarity
  private static readonly IList<IPowerUp> s_powerUpsRarity;

  static PowerUpManager()
  {
    s_allPowerUps = new List<IPowerUp>
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
      new GeneratorEngine(),
      new MiniGun(),
      new Sniper()
    };

    s_powerUpsRarity = new List<IPowerUp>();
    foreach (var powerUp in s_allPowerUps)
    {
      for (var i = 0; i < (int)powerUp.Rarity; i++)
      {
        s_powerUpsRarity.Add(powerUp);
      }
    }
  }

  public static IEnumerable<IPowerUp> GetAllPowerUps()
  {
    return s_allPowerUps;
  }

  public static IPowerUp GetRandomPowerup()
  {
    var random = new Random();
    var randomIndex = random.Next(0, s_allPowerUps.Count);
    return s_allPowerUps[randomIndex];
  }

  // Gets n power ups. At least nRare should be rare or better (legendary)
  public static IEnumerable<IPowerUp> GetUniquePowerUpsWithRarity(int n, int nRare)
  {
    if (n <= 0 || nRare <= 0)
    {
      throw new ArgumentException("n and nRare must be greater than 0");
    }

    if (nRare > n)
    {
      throw new ArgumentException("Rare amount must be lesser than the total amount");
    }

    var rares = s_powerUpsRarity.Where(p => p.Rarity == Rarity.Rare).ToList();
    var raresAdded = 0;

    var output = new List<IPowerUp>();
    while (output.Count < n)
    {
      // Force selection of rare powerups until minimum is reached
      var p = raresAdded < nRare ? rares.Sample() : s_powerUpsRarity.Sample();

      if (output.Contains(p))
      {
        continue;
      }

      if (p.Rarity == Rarity.Rare)
      {
        raresAdded++;
      }

      output.Add(p);
    }

    return output.Shuffled();
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
      var randomIndex = random.Next(0, s_allPowerUps.Count);
      var powerUp = s_allPowerUps[randomIndex];

      // Add unique powerup
      if (!powerUps.Contains(powerUp))
      {
        powerUps.Add(powerUp);
      }

      // All unique powerups added, add duplicates
      if (powerUps.Count >= s_allPowerUps.Count)
      {
        powerUps.Add(powerUp);
      }
    }

    return powerUps.Shuffled();
  }

  public interface IPowerUp
  {
    string Name { get; }
    Rarity Rarity { get; }

    void Apply(Player player);
  }

  private class AirWalker : IPowerUp
  {
    public string Name => "Air Walker";

    public Rarity Rarity => Rarity.Common;

    public void Apply(Player player)
    {
      player.PlayerMovementDelegate.MaxJumps += 1;
    }
  }

  private class BunnyBoost : IPowerUp
  {
    public string Name => "Bunny Boost";

    public Rarity Rarity => Rarity.Common;

    public void Apply(Player player)
    {
      player.PlayerMovementDelegate.JumpForce += 300;
    }
  }

  private class GeneratorEngine : IPowerUp
  {
    public string Name => "Generator Engine";

    public Rarity Rarity => Rarity.Rare;

    public void Apply(Player player)
    {
      player.Gun.FireRate -= 0.7f;
      player.Gun.BulletSpread *= 1.05f;
    }
  }

  private class GlassCannon : IPowerUp
  {
    public string Name => "Glass Cannon";

    public Rarity Rarity => Rarity.Rare;

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

    public Rarity Rarity => Rarity.Common;

    public void Apply(Player player)
    {
      player.Gun.BulletGravity = 0.0f;
    }
  }

  private class HealthBoost : IPowerUp
  {
    public string Name => "Health Boost";

    public Rarity Rarity => Rarity.Common;

    public void Apply(Player player)
    {
      player.MaxHealth = (int)(player.MaxHealth * 1.5f);
    }
  }

  private class PhotonAccelerator : IPowerUp
  {
    public string Name => "Photon Accelerator";

    public Rarity Rarity => Rarity.Common;

    public void Apply(Player player)
    {
      player.Gun.BulletSpeed += 300.0f;
      player.Gun.BulletSpread *= 1.05f;
    }
  }

  private class PhotonBoost : IPowerUp
  {
    public string Name => "Photon Boost";

    public Rarity Rarity => Rarity.Common;

    public void Apply(Player player)
    {
      player.PlayerMovementDelegate.Speed += 200;
    }
  }

  private class PhotonEnlarger : IPowerUp
  {
    public string Name => "Photon Enlarger";

    public Rarity Rarity => Rarity.Common;

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

    public Rarity Rarity => Rarity.Rare;

    public void Apply(Player player)
    {
      player.Gun.BulletCount = (int)Math.Ceiling(player.Gun.BulletCount * 1.5f);
      player.Gun.BulletSpread *= 1.05f;
    }
  }

  private class PhotonMuncher : IPowerUp
  {
    public string Name => "Mega Photon Muncher";

    public Rarity Rarity => Rarity.Rare;

    public void Apply(Player player)
    {
      player.MaxHealth *= 2;
      player.PlayerMovementDelegate.Speed -= -200.0f;
      player.Gun.BulletSpread *= 1.2f;
    }
  }

  private class MiniGun : IPowerUp
  {
    public string Name => "1 000 000 lumen";

    public Rarity Rarity => Rarity.Legendary;

    public void Apply(Player player)
    {
      player.Gun.BulletCount += 8;
      player.Gun.BulletDamage = 1;
      player.Gun.BulletSpread += 0.3f;
      player.Gun.FireRate += 3;
    }
  }

  private class Sniper : IPowerUp
  {
    public string Name => "Photon Sniper";

    public Rarity Rarity => Rarity.Legendary;

    public void Apply(Player player)
    {
      player.Gun.BulletCount = 1;
      player.Gun.BulletDamage = 50;
      player.Gun.BulletSpread = 0.01f;
      player.Gun.FireRate -= 3;
    }
  }

}
