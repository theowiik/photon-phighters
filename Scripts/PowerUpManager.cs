using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using PhotonPhighters.Scripts.Utils;
using static PhotonPhighters.Scripts.PowerUps;

namespace PhotonPhighters.Scripts;

public static class PowerUpManager
{
  public static readonly IEnumerable<IPowerUpApplier> PowerUps;

  static PowerUpManager()
  {
    if (!RaritySumIs100)
    {
      throw new Exception("Rarities sum must be 100");
    }

    PowerUps = new List<IPowerUpApplier>
    {
      new PhotonBoost(),
      new HealthBoost(),
      new BunnyBoost(),
      new PhotonMultiplier(),
      new PhotonEnlarger(),
      new PhotonAccelerator(),
      new Gravitronizer(),
      new PhotonMuncher(),
      new AirWalker(),
      new GeneratorEngine(),
      new MiniGun(),
      new Sniper(),
      new SteelBootsCurse(),
      new StickyThickyCurse(),
      new BulletRain(),
      new MomentumMaster(),
      new WallSpider(),
      new OingoBoingoCurse(),
      new PostLegDayCurse(),
      new Chronostasis(),
      new BrownianMotionCurse(),
      new FluorescentBurst(),
      new SimpleTrigonometry(),
      new LuminogravitonFluxCurse(),
      new PhotonReversifierCurse(),
      new PheedingPhrenzy(),
      new Randomizer5000(),
      new PhotonPhlyer(),
      new NikeAirJordans(),
      new FakeJordans(),
      new SketchyPillsGood(),
      new SketchyPillsBad(),
      new BerserkerJuice()
    };

    CalculateOdds();
  }

  private static RarityCumulative RarityCumulativeOdds
  {
    get
    {
      const int CommonCumulative = (int)Rarity.Common;
      const int RareCumulative = CommonCumulative + (int)Rarity.Rare;
      const int EpicCumulative = RareCumulative + (int)Rarity.Epic;
      const int LegendaryCumulative = EpicCumulative + (int)Rarity.Legendary;

      return new RarityCumulative
      {
        Common = CommonCumulative,
        Rare = RareCumulative,
        Epic = EpicCumulative,
        Legendary = LegendaryCumulative
      };
    }
  }

  public static IEnumerable<IPowerUpApplier> GetUniquePowerUps(int n)
  {
    if (n <= 0)
    {
      throw new ArgumentException("n must be greater than 0");
    }

    var output = new List<IPowerUpApplier>();
    while (output.Count < n)
    {
      var p = GetRandomPowerUpWithOdds();

      if (output.Contains(p))
      {
        continue;
      }

      output.Add(p);
    }

    return output.Shuffled();
  }

  private static IPowerUpApplier GetRandomPowerUpWithOdds()
  {
    var rarity = GetRandomRarityWithOdds();
    return PowerUps.Where(p => p.Rarity == rarity).ToList().Sample();
  }

  private static Rarity GetRandomRarityWithOdds()
  {
    var random = new Random();
    var randomNumber = random.Next(1, RarityCumulativeOdds.Legendary + 1);

    if (randomNumber <= RarityCumulativeOdds.Common)
    {
      return Rarity.Common;
    }

    if (randomNumber <= RarityCumulativeOdds.Rare)
    {
      return Rarity.Rare;
    }

    if (randomNumber <= RarityCumulativeOdds.Epic)
    {
      return Rarity.Epic;
    }

    if (randomNumber <= RarityCumulativeOdds.Legendary)
    {
      return Rarity.Legendary;
    }

    throw new Exception("Rarity must sum to 100");
  }

  private static void CalculateOdds()
  {
    var powerUpProbabilites = new List<Tuple<string, float>>();

    foreach (var rarity in Enum.GetValues(typeof(Rarity)).Cast<Rarity>())
    {
      var powerUpsOfRarity = PowerUps.Where(p => p.Rarity == rarity);
      var probabilityPerPowerUp = (int)rarity / (float)powerUpsOfRarity.Count();
      powerUpProbabilites.AddRange(
        powerUpsOfRarity.Select(
          powerUp => new Tuple<string, float>($"{powerUp.Rarity} - {powerUp.Name}", probabilityPerPowerUp)
        )
      );
    }

    WriteTupleListToFile(powerUpProbabilites, "probabilities.csv", "Power Up", "Probability");
  }

  private static void WriteTupleListToFile(
    IEnumerable<Tuple<string, float>> tupleList,
    string filename,
    string column1,
    string column2
  )
  {
    using var writer = new StreamWriter(filename);
    writer.WriteLine($"{column1}, {column2}");

    foreach (var tuple in tupleList)
    {
      writer.WriteLine("{0}, {1}%", tuple.Item1, tuple.Item2.ToString("0.00", CultureInfo.InvariantCulture));
    }
  }

  private struct RarityCumulative
  {
    public int Common { get; set; }
    public int Rare { get; set; }
    public int Epic { get; set; }
    public int Legendary { get; set; }
  }
}
