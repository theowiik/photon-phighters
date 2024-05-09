using System.Text;

namespace PhotonPhighters.Scripts.Utils;

public static class NumberUtil
{
  public static string ToRoman(int number)
  {
    if (number is < 1 or > 3999)
    {
      return "Invalid Input. The range for Roman numerals is 1-3999.";
    }

    var digits = new[] { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
    var values = new[] { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };

    var result = new StringBuilder();

    for (var i = 0; i < values.Length; i++)
    {
      while (number >= values[i])
      {
        number -= values[i];
        result.Append(digits[i]);
      }
    }

    return result.ToString();
  }
}
