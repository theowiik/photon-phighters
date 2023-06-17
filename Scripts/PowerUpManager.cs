using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;

public static class PowerUpManager
{
  // List of power ups, with duplicates to represent the rarity
  private static readonly IList<PowerUps.IPowerUpApplier> s_powerUpsRarity;

  static PowerUpManager()
  {
    var allPowerUps = new List<PowerUps.IPowerUpApplier>
    {
      new PowerUps.PhotonBoost(),
      new PowerUps.HealthBoost(),
      new PowerUps.BunnyBoost(),
      new PowerUps.PhotonMultiplier(),
      new PowerUps.PhotonEnlarger(),
      new PowerUps.PhotonAccelerator(),
      new PowerUps.Gravitronizer(),
      new PowerUps.PhotonMuncher(),
      new PowerUps.AirWalker(),
      new PowerUps.GeneratorEngine(),
      new PowerUps.MiniGun(),
      new PowerUps.Sniper(),
      new PowerUps.SteelBootsCurse(),
      new PowerUps.StickyThickyCurse(),
      new PowerUps.BulletRain(),
      new PowerUps.MomentumMaster(),
      new PowerUps.WallSpider(),
      new PowerUps.OingoBoingoCurse(),
      new PowerUps.PostLegDayCurse(),
      new PowerUps.Chronostasis(),
      new PowerUps.BrownianMotionCurse(),
      new PowerUps.FluorescentBurst(),
      new PowerUps.SimpleTrigonometry(),
      new PowerUps.LuminogravitonFluxCurse(),
      new PowerUps.PhotonReversifierCurse(),
      new PowerUps.PheedingPhrenzy(),
      new PowerUps.Randomizer5000(),
      new PowerUps.PhotonPhlyer(),
      new PowerUps.NikeAirJordans(),
      new PowerUps.FakeJordans(),
      new PowerUps.SketchyPillsGood(),
      new PowerUps.SketchyPillsBad()
    };

    s_powerUpsRarity = new List<PowerUps.IPowerUpApplier>();
    foreach (var powerUp in allPowerUps)
    {
      for (var i = 0; i < (int)powerUp.Rarity; i++)
      {
        s_powerUpsRarity.Add(powerUp);
      }
    }

    CalculateOdds(s_powerUpsRarity);
  }

  public static IEnumerable<PowerUps.IPowerUpApplier> AllPowerUps => s_powerUpsRarity.Distinct().ToList();

  // Gets n power ups. At least nRare should be rare or better (legendary)
  public static IEnumerable<PowerUps.IPowerUpApplier> GetUniquePowerUpsWithRarity(int n, int nRare)
  {
    if (n <= 0)
    {
      throw new ArgumentException("n must be greater than 0");
    }

    if (nRare > n)
    {
      throw new ArgumentException("Rare amount must be lesser than the total amount");
    }

    var rares = s_powerUpsRarity.Where(p => p.Rarity == PowerUps.Rarity.Rare).ToList();
    var raresAdded = 0;

    var output = new List<PowerUps.IPowerUpApplier>();
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

  private static void CalculateOdds(IEnumerable<PowerUps.IPowerUpApplier> powerUps)
  {
    var total = powerUps.Count();
    var uniquePowerUps = powerUps.Distinct();

    var things = (
      from powerUp in uniquePowerUps
      let count = powerUps.Count(p => p == powerUp)
      let percent = (float)count / total * 100
      select new Tuple<string, float>($"({powerUp.Rarity}) - {powerUp.Name}", percent)
    )
      .OrderBy(item => item.Item2)
      .ToList();

    WriteTupleListToFile(things, "probabilities.csv");
  }

  private static void WriteTupleListToFile(List<Tuple<string, float>> tupleList, string filename)
  {
    using var writer = new StreamWriter(filename);

    writer.WriteLine("Power Up, Probability");

    foreach (var tuple in tupleList)
    {
      writer.WriteLine("{0}, {1}%", tuple.Item1, tuple.Item2.ToString("0.00", CultureInfo.InvariantCulture));
    }
  }
}
