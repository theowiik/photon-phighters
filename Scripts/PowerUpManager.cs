using System;
using System.Collections.Generic;
using System.Linq;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;

public static class PowerUpManager
{
  private static readonly IList<PowerUps.IPowerUp> s_allPowerUps;

  // List of power ups, with duplicates to represent the rarity
  private static readonly IList<PowerUps.IPowerUp> s_powerUpsRarity;

  static PowerUpManager()
  {
    s_allPowerUps = new List<PowerUps.IPowerUp>
    {
      new PowerUps.PhotonBoost(),
      new PowerUps.HealthBoost(),
      new PowerUps.BunnyBoost(),
      new PowerUps.PhotonMultiplier(),
      new PowerUps.PhotonEnlarger(),
      new PowerUps.PhotonAccelerator(),
      new PowerUps.GlassCannon(),
      new PowerUps.Gravitronizer(),
      new PowerUps.PhotonMuncher(),
      new PowerUps.AirWalker(),
      new PowerUps.GeneratorEngine(),
      new PowerUps.MiniGun(),
      new PowerUps.Sniper()
    };

    s_powerUpsRarity = new List<PowerUps.IPowerUp>();
    foreach (var powerUp in s_allPowerUps)
    {
      for (var i = 0; i < (int)powerUp.Rarity; i++)
      {
        s_powerUpsRarity.Add(powerUp);
      }
    }
  }

  public static IEnumerable<PowerUps.IPowerUp> GetAllPowerUps()
  {
    return s_allPowerUps;
  }

  public static PowerUps.IPowerUp GetRandomPowerup()
  {
    var random = new Random();
    var randomIndex = random.Next(0, s_allPowerUps.Count);
    return s_allPowerUps[randomIndex];
  }

  // Gets n power ups. At least nRare should be rare or better (legendary)
  public static IEnumerable<PowerUps.IPowerUp> GetUniquePowerUpsWithRarity(int n, int nRare)
  {
    if (n <= 0 || nRare <= 0)
    {
      throw new ArgumentException("n and nRare must be greater than 0");
    }

    if (nRare > n)
    {
      throw new ArgumentException("Rare amount must be lesser than the total amount");
    }

    var rares = s_powerUpsRarity.Where(p => p.Rarity == PowerUps.Rarity.Rare).ToList();
    var raresAdded = 0;

    var output = new List<PowerUps.IPowerUp>();
    while (output.Count < n)
    {
      // Force selection of rare powerups until minimum is reached
      var p = raresAdded < nRare ? rares.Sample() : s_powerUpsRarity.Sample();

      if (output.Contains(p))
      {
        continue;
      }

      if (p.Rarity == PowerUps.Rarity.Rare)
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
  public static IEnumerable<PowerUps.IPowerUp> GetUniquePowerUps(int n)
  {
    if (n < 0)
    {
      throw new ArgumentException("n must be greater than or equal to 0");
    }

    var random = new Random();
    var powerUps = new List<PowerUps.IPowerUp>();
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
}
