using System.Collections.Generic;
using System.Linq;

namespace PhotonPhighters.Scripts;

public static class LightUtils
{
  public static Score CountScore(IEnumerable<Light> lights)
  {
    var lightsArr = lights as Light[] ?? lights.ToArray();
    return new Score
    {
      Dark = lightsArr.Count(light => light.LightState == Light.LightMode.Dark),
      Light = lightsArr.Count(light => light.LightState == Light.LightMode.Light),
      Ties = lightsArr.Count(light => light.LightState == Light.LightMode.None)
    };
  }
}
