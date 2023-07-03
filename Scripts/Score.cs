using System;

namespace PhotonPhighters.Scripts;

public struct Score : IEquatable<Score>
{
  public int Dark { get; set; }
  public int Light { get; set; }
  public int Ties { get; set; }

  public bool Equals(Score other)
  {
    return Dark == other.Dark && Light == other.Light && Ties == other.Ties;
  }

  public override bool Equals(object obj)
  {
    return obj is Score other && Equals(other);
  }

  public override int GetHashCode()
  {
    return HashCode.Combine(Dark, Light, Ties);
  }

  public static bool operator ==(Score left, Score right)
  {
    return left.Equals(right);
  }

  public static bool operator !=(Score left, Score right)
  {
    return !left.Equals(right);
  }
}
