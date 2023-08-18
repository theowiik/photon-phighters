using System.Text.RegularExpressions;

namespace PhotonPhighters.Scripts.Utils.ResourceWrapper;

public static class StringUtil
{
  public static string ToSnakeCase(string str)
  {
    return string.IsNullOrEmpty(str) ? str : Regex.Replace(str, "(?<=.)([A-Z])", "_$1").ToLower();
  }
}
