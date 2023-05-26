using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
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

    CalculateOdds(s_powerUpsRarity);
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
      // Force selection of rare power-ups until minimum is reached
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

  private static void CalculateOdds(IEnumerable<PowerUps.IPowerUp> powerUps)
  {
    var total = powerUps.Count();
    var uniquePowerUps = powerUps.Distinct();

    var things = (
      from powerUp in uniquePowerUps
      let count = powerUps.Count(p => p == powerUp)
      let percent = (float)count / total * 100
      select new Tuple<string, float>($"({powerUp.Rarity}) - {powerUp.Name}", percent)
    ).OrderBy(item => item.Item2).ToList();

    WriteTupleListToFile(things, "probabilities.csv");
  }

  private static void WriteTupleListToFile(List<Tuple<string, float>> tupleList, string filename)
  {
    using var writer = new StreamWriter(filename);

    writer.WriteLine("Power Up, Probability");

    foreach (var tuple in tupleList)
    {
      writer.WriteLine("{0}, {1}%", tuple.Item1, Math.Round(tuple.Item2, 2));
    }
  }
}
