using System;

namespace PhotonPhighters.Scripts;

public static class PowerUps
{
  public enum Rarity
  {
    Legendary = 1,
    Rare = 2,
    Common = 4
  }

  public interface IPowerUp
  {
    string Name { get; }
    Rarity Rarity { get; }

    void Apply(Player playerWhoSelected, Player otherPlayer);
  }

  public class AirWalker : IPowerUp
  {
    public string Name => "Air Walker";

    public Rarity Rarity => Rarity.Common;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.PlayerMovementDelegate.MaxJumps++;
    }
  }

  public class BunnyBoost : IPowerUp
  {
    public string Name => "Bunny Boost";

    public Rarity Rarity => Rarity.Common;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.PlayerMovementDelegate.JumpForce += 300;
    }
  }

  public class GeneratorEngine : IPowerUp
  {
    public string Name => "Generator Engine";

    public Rarity Rarity => Rarity.Rare;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.FireRate -= 0.7f;
      playerWhoSelected.Gun.BulletSpread *= 1.05f;
    }
  }

  public class GlassCannon : IPowerUp
  {
    public string Name => "Glass Cannon";

    public Rarity Rarity => Rarity.Rare;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.MaxHealth /= 2;
      playerWhoSelected.Gun.BulletDamage *= 3;
      playerWhoSelected.Gun.BulletSpread *= 1.15f;
    }
  }

  public class Gravitronizer : IPowerUp
  {
    public string Name => "Gravitronizer";

    public Rarity Rarity => Rarity.Common;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.BulletGravity = 0.0f;
    }
  }

  public class HealthBoost : IPowerUp
  {
    public string Name => "Health Boost";

    public Rarity Rarity => Rarity.Common;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.MaxHealth = (int)(playerWhoSelected.MaxHealth * 1.5f);
    }
  }

  public class PhotonAccelerator : IPowerUp
  {
    public string Name => "Photon Accelerator";

    public Rarity Rarity => Rarity.Common;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.BulletSpeed += 300.0f;
      playerWhoSelected.Gun.BulletSpread *= 1.05f;
    }
  }

  public class PhotonBoost : IPowerUp
  {
    public string Name => "Photon Boost";

    public Rarity Rarity => Rarity.Common;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.PlayerMovementDelegate.Speed += 200;
    }
  }

  public class PhotonEnlarger : IPowerUp
  {
    public string Name => "Photon Enlarger";

    public Rarity Rarity => Rarity.Common;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.BulletSizeFactor += 1.5f;
      playerWhoSelected.Gun.BulletDamage += 10;
      playerWhoSelected.Gun.BulletSpeed -= 150.0f;
      playerWhoSelected.Gun.BulletSpread *= 1.25f;
    }
  }

  public class PhotonMultiplier : IPowerUp
  {
    public string Name => "Photon Multiplier";

    public Rarity Rarity => Rarity.Rare;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.BulletCount = (int)Math.Ceiling(playerWhoSelected.Gun.BulletCount * 1.5f);
      playerWhoSelected.Gun.BulletSpread *= 1.05f;
    }
  }

  public class PhotonMuncher : IPowerUp
  {
    public string Name => "Mega Photon Muncher";

    public Rarity Rarity => Rarity.Rare;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.MaxHealth *= 2;
      playerWhoSelected.PlayerMovementDelegate.Speed -= -200.0f;
      playerWhoSelected.Gun.BulletSpread *= 1.2f;
    }
  }

  public class MiniGun : IPowerUp
  {
    public string Name => "1 000 000 lumen";

    public Rarity Rarity => Rarity.Legendary;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.BulletCount += 8;
      playerWhoSelected.Gun.BulletDamage = 1;
      playerWhoSelected.Gun.BulletSpread += 0.3f;
      playerWhoSelected.Gun.FireRate += 3;
    }
  }

  public class Sniper : IPowerUp
  {
    public string Name => "Photon Sniper";

    public Rarity Rarity => Rarity.Legendary;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.BulletCount = 1;
      playerWhoSelected.Gun.BulletDamage = 50;
      playerWhoSelected.Gun.BulletSpread = 0.01f;
      playerWhoSelected.Gun.FireRate -= 3;
    }
  }

  public class SteelBootsCurse : IPowerUp
  {
    public string Name => "Steel Boots Curse";
    public Rarity Rarity => Rarity.Rare;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      otherPlayer.PlayerMovementDelegate.JumpForce -= 500;
    }
  }
}
