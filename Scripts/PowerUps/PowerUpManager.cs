using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using PhotonPhighters.Scripts.Utils;
using static PhotonPhighters.Scripts.PowerUps.Appliers.PowerUps;
using static PhotonPhighters.Scripts.PowerUps.PowerUps.Rarity;

namespace PhotonPhighters.Scripts.PowerUps;

public static class PowerUpManager
{
  public static readonly IEnumerable<PowerUps.IPowerUpApplier> PowerUps;

  static PowerUpManager()
  {
    if (!RaritySumIs100)
    {
      throw new FormatException("Rarities sum must be 100");
    }

    PowerUps = new List<PowerUps.IPowerUpApplier>
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
      new SteelBootsCurse(),
      new StickyThickyCurse(),
      new BulletRain(),
      new MomentumMaster(),
      new WallSpider(),
      new OingoBoingoCurse(),
      new Chronostasis(),
      new BrownianMotionCurse(),
      new FluorescentBurst(),
      new SimpleTrigonometry(),
      new PhotonReversifierCurse(),
      new PheedingPhrenzy(),
      new Randomizer5000(),
      new PhotonPhlyer(),
      new NikeAirJordans(),
      new FakeJordans(),
      new SketchyPillsGood(),
      new SketchyPillsBad(),
      new BerserkerJuice(),
      new EliasSpecialSauce()
    };

    CalculateOdds();
  }

  private static bool RaritySumIs100 =>
    Enum.GetValues(typeof(PowerUps.Rarity)).Cast<PowerUps.Rarity>().Sum(rarity => (int)rarity) == 100;

  private static RarityCumulative RarityCumulativeOdds
  {
    get
    {
      const int CommonCumulative = (int)Common;
      const int RareCumulative = CommonCumulative + (int)Rare;
      const int EpicCumulative = RareCumulative + (int)Epic;
      const int LegendaryCumulative = EpicCumulative + (int)Legendary;

      return new RarityCumulative
      {
        Common = CommonCumulative,
        Rare = RareCumulative,
        Epic = EpicCumulative,
        Legendary = LegendaryCumulative
      };
    }
  }

  public static IEnumerable<PowerUps.IPowerUpApplier> GetUniquePowerUps(int n)
  {
    if (n <= 0)
    {
      throw new ArgumentException("n must be greater than 0");
    }

    var output = new List<PowerUps.IPowerUpApplier>();
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

  private static PowerUps.IPowerUpApplier GetRandomPowerUpWithOdds()
  {
    var rarity = GetRandomRarityWithOdds();
    return PowerUps.Where(p => p.Rarity == rarity).ToList().Sample();
  }

  private static PowerUps.Rarity GetRandomRarityWithOdds()
  {
    var random = new Random();
    var randomNumber = random.Next(1, RarityCumulativeOdds.Legendary + 1);

    if (randomNumber <= RarityCumulativeOdds.Common)
    {
      return Common;
    }

    if (randomNumber <= RarityCumulativeOdds.Rare)
    {
      return Rare;
    }

    if (randomNumber <= RarityCumulativeOdds.Epic)
    {
      return Epic;
    }

    if (randomNumber <= RarityCumulativeOdds.Legendary)
    {
      return Legendary;
    }

    throw new FormatException("Rarity must sum to 100");
  }

  private static void CalculateOdds()
  {
    var powerUpProbabilites = new List<Tuple<string, float>>();

    foreach (var rarity in Enum.GetValues(typeof(PowerUps.Rarity)).Cast<PowerUps.Rarity>())
    {
      var powerUpsOfRarity = PowerUps.Where(p => p.Rarity == rarity).ToList();
      var probabilityPerPowerUp = (int)rarity / (float)powerUpsOfRarity.Count;
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

  private readonly struct RarityCumulative : IEquatable<RarityCumulative>
  {
    public int Common { get; init; }
    public int Rare { get; init; }
    public int Epic { get; init; }
    public int Legendary { get; init; }

    public bool Equals(RarityCumulative other)
    {
      return Common == other.Common && Rare == other.Rare && Epic == other.Epic && Legendary == other.Legendary;
    }

    public override bool Equals(object obj)
    {
      return obj is RarityCumulative other && Equals(other);
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(Common, Rare, Epic, Legendary);
    }
  }
}
