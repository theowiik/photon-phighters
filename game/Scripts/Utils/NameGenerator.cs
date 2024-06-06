using System.Collections.Generic;
using System.Linq;

namespace PhotonPhighters.Scripts.Utils;

/// <summary>
///   Generates guaranteed unique names (unique within the same session).
/// </summary>
public static class NameGenerator
{
  private static readonly IList<string> s_predefinedNames = new List<string>
  {
    "Photon",
    "Ray",
    "Glim",
    "Flick",
    "Spark",
    "Flash",
    "Beam",
    "Glow",
    "Twink",
    "Laser",
    "Flare",
    "Blaze",
    "Lumen",
    "Prism",
    "Neon",
    "Glint",
    "Radi",
    "Shine",
    "Blip",
    "Aurora",
    "Glimp",
    "Sparkle",
    "Star",
    "Sun",
    "Dazz",
    "Glare"
  }
    .Shuffled()
    .ToList();

  private static readonly ISet<string> s_usedNames = new HashSet<string>();
  private static int s_counter = 1;

  public static string Get()
  {
    foreach (var name in s_predefinedNames)
    {
      if (!s_usedNames.Contains(name))
      {
        s_usedNames.Add(name);
        return name;
      }
    }

    // If all predefined names are taken, generate a new unique name
    string newName;
    do
    {
      newName = s_predefinedNames[s_counter % s_predefinedNames.Count] + s_counter;
      s_counter++;
    } while (s_usedNames.Contains(newName));

    s_usedNames.Add(newName);
    return newName;
  }
}
