using System;
using Godot;

namespace PhotonPhighters.Scripts;

public static class PowerUps
{
  public enum Rarity
  {
    Legendary = 1,
    Rare = 6,
    Common = 11
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
      playerWhoSelected.Gun.FireRate += 0.9f;
      playerWhoSelected.Gun.BulletSpread *= 1.06f;
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
      playerWhoSelected.Gun.BulletDamage = Mathf.RoundToInt(playerWhoSelected.Gun.BulletDamage * 1.333f);
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
      playerWhoSelected.MaxHealth = Mathf.RoundToInt(playerWhoSelected.MaxHealth * 1.42f);
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
      playerWhoSelected.Gun.BulletCount += 7;
      playerWhoSelected.Gun.BulletDamage = 1;
      playerWhoSelected.Gun.BulletSpread += 0.3f;
      playerWhoSelected.Gun.BulletSpeed /= 1.4f;
      playerWhoSelected.Gun.FireRate += 2.5f;
    }
  }

  public class Sniper : IPowerUp
  {
    public string Name => "Photon Sniper";

    public Rarity Rarity => Rarity.Legendary;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.BulletCount = 1;
      playerWhoSelected.Gun.BulletDamage = 100;
      playerWhoSelected.Gun.BulletSpread = 0.00001f;
      playerWhoSelected.Gun.BulletSpeed = 3000;
      playerWhoSelected.Gun.FireRate = 1.6f;
    }
  }

  public class SteelBootsCurse : IPowerUp
  {
    public string Name => "Steel Boots Curse";
    public Rarity Rarity => Rarity.Rare;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      otherPlayer.PlayerMovementDelegate.JumpForce /= 1.33f;
    }
  }

  // TODO: Make the player thicc
  // TODO: Add sticky sound effect when walking
  // TODO: Sticky shoot sounds
  public class StickyThickyCurse : IPowerUp
  {
    public string Name => "Sticky Thicky Curse";
    public Rarity Rarity => Rarity.Legendary;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      otherPlayer.PlayerMovementDelegate.Speed /= 2;
      otherPlayer.PlayerMovementDelegate.Acceleration /= 3;
      otherPlayer.MaxHealth += 50; // TODO: Possibly make it relative to the player's max health
    }
  }

  public class MomentumMaster : IPowerUp
  {
    public string Name => "Momentum Master";

    public Rarity Rarity => Rarity.Rare;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.PlayerMovementDelegate.Speed += 300;
      playerWhoSelected.PlayerMovementDelegate.Acceleration += 6;
    }
  }

  public class BulletRain : IPowerUp
  {
    public string Name => "Bullet Rain";

    public Rarity Rarity => Rarity.Rare;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.BulletCount *= 2; // Double the bullets
      playerWhoSelected.Gun.BulletGravity *= 2; // Bullets drop faster
    }
  }

  public class WallSpider : IPowerUp
  {
    // Walljumping briefly increases movement speed
    public string Name => "Wall Spider";
    public Rarity Rarity => Rarity.Common;
    private ulong _msecSinceLastWallJump;

    public void Apply(Player player, Player otherPlayer)
    {
      player.PlayerMovementDelegate.PlayerMove += GiveSpeedBoost;
      player.PlayerMovementDelegate.PlayerWallJump += RecordTimeSinceWallJump;
    }

    public void GiveSpeedBoost(Events.PlayerMovementEvent playerMovementEvent)
    {
      var currentTimeMsec = Time.GetTicksMsec();
      if (currentTimeMsec - _msecSinceLastWallJump < 3000)
      {
        playerMovementEvent._speed *= 1.5f;
      }
    }

    public void RecordTimeSinceWallJump(Events.PlayerMovementEvent playerMovementEvent)
    {
      _msecSinceLastWallJump = Time.GetTicksMsec();
    }
  }

  public class OingoBoingoCurse : IPowerUp
  {
    // Opponent is always bouncing
    public string Name => "Oingo Boingo Curse";
    public Rarity Rarity => Rarity.Common;

    public void Apply(Player player, Player otherPlayer)
    {
      otherPlayer.PlayerMovementDelegate.PlayerLand += ApplyBounce;
    }

    public static void ApplyBounce(Events.PlayerMovementEvent playerMovementEvent)
    {
      playerMovementEvent._velocity.Y -= 1000;
    }
  }

  public class PostLegDayCurse : IPowerUp
  {
    // Opponent has to briefly rest between jumps
    public string Name => "Post Leg Day Curse";
    public Rarity Rarity => Rarity.Rare;
    private ulong _msecSinceLastJump;

    public void Apply(Player player, Player otherPlayer)
    {
      otherPlayer.PlayerMovementDelegate.PlayerJump += DelayJump;
    }

    public void DelayJump(Events.PlayerMovementEvent playerMovementEvent)
    {
      var currentTimeMsec = Time.GetTicksMsec();
      if (currentTimeMsec - _msecSinceLastJump < 2000)
      {
        playerMovementEvent._canJump = false;
      }
      else
      {
        _msecSinceLastJump = currentTimeMsec;
      }
    }
  }

  public class Chronostasis : IPowerUp
  {
    // Photons briefly freeze the opponent
    public string Name => "Chronostasis";
    public Rarity Rarity => Rarity.Legendary;
    public ulong _msecSinceLastFreeze;

    public void Apply(Player player, Player otherPlayer)
    {
      otherPlayer.PlayerHurt += RecordTimeSinceFreeze;
      otherPlayer.PlayerMovementDelegate.PlayerMove += FreezePlayer;
    }

    public void FreezePlayer(Events.PlayerMovementEvent movementEvent)
    {
      var currentTimeMsec = Time.GetTicksMsec();
      if (currentTimeMsec - _msecSinceLastFreeze < 5000)
      {
        movementEvent._canMove = false;
        movementEvent._canJump = false;
      }
    }

    public void RecordTimeSinceFreeze(Player player, int damage, Events.PlayerHurtEvent playerHurtEvent)
    {
      _msecSinceLastFreeze = Time.GetTicksMsec();
    }
  }

  /* public class RayleighScattering : IPowerUp
  {
    // Photons scatter when hitting surfaces
    public string Name => "Rayleigh Scattering";
    public Rarity Rarity => Rarity.Rare;

    public void Apply(Player player, Player otherPlayer)
    {
      player.Gun.BulletCollideFloor += ScatterPhotons;
    }

    public void ScatterPhotons(BulletEvents.BulletEvent bulletCollideFloorEvent)
    {
      // TODO: Spawn bullets
    }
  } */

  public class BrownianMotionCurse : IPowerUp
  {
    // Opponent's photons move erratically
    public string Name => "Brownian Motion Curse";
    public Rarity Rarity => Rarity.Rare;
    private ulong _msecSinceRandomization;
    private readonly Random _rnd = new();

    public void Apply(Player player, Player otherPlayer)
    {
      otherPlayer.Gun.BulletFlying += RandomizeDirection;
    }

    public void RandomizeDirection(Events.BulletEvent bulletFlyingEvent)
    {
      var currentTimeMsec = Time.GetTicksMsec();
      if (currentTimeMsec - _msecSinceRandomization > 100)
      {
        bulletFlyingEvent._velocity.X += _rnd.Next(-200, 200);
        bulletFlyingEvent._velocity.Y += _rnd.Next(-200, 200);
      }
      else
      {
        _msecSinceRandomization = currentTimeMsec;
      }
    }
  }

  public class FluorescentBurst : IPowerUp
  {
    // Getting hurt briefly increases movement speed
    public string Name => "Fluorescent Burst";
    public Rarity Rarity => Rarity.Rare;

    public ulong _msecSinceLastHurt;

    public void Apply(Player player, Player otherPlayer)
    {
      player.PlayerMovementDelegate.PlayerMove += GiveSpeedBoost;
      player.PlayerHurt += RecordTimeSinceHurt;
    }

    public void GiveSpeedBoost(Events.PlayerMovementEvent playerMovementEvent)
    {
      var currentTimeMsec = Time.GetTicksMsec();
      if (currentTimeMsec - _msecSinceLastHurt < 1000)
      {
        playerMovementEvent._speed *= 1.5f;
      }
    }

    public void RecordTimeSinceHurt(Player player, int damage, Events.PlayerHurtEvent playerHurtEvent)
    {
      _msecSinceLastHurt = Time.GetTicksMsec();
    }
  }

  public class SimpleTrigonometry : IPowerUp
  {
    // Photons move toward the other player
    public string Name => "Simple Trigonometry";
    public Rarity Rarity => Rarity.Rare;

    public Player _otherPlayer;

    public void Apply(Player player, Player otherPlayer)
    {
      this._otherPlayer = otherPlayer;
      player.Gun.BulletFlying += MoveToOtherPlayer;
    }

    public void MoveToOtherPlayer(Events.BulletEvent bulletEvent)
    {
      var vector = this._otherPlayer.Position - bulletEvent._area2D.Position;
      var magnitude = vector.Length();
      bulletEvent._velocity.X += vector.X / (magnitude / 20);
      bulletEvent._velocity.Y += vector.Y / (magnitude / 20);
    }
  }

  public class LuminogravitonFluxCurse : IPowerUp
  {
    // The opponent's gravity is reversed
    public string Name => "Luminograviton Flux Curse";
    public Rarity Rarity => Rarity.Legendary;

    public void Apply(Player player, Player otherPlayer)
    {
      otherPlayer.PlayerMovementDelegate.PlayerMove += ReverseGravity;
    }

    public static void ReverseGravity(Events.PlayerMovementEvent movementEvent)
    {
      movementEvent._gravity *= -1;
      movementEvent._jumpForce *= -1;
    }
  }

  public class PhotonReversifierCurse : IPowerUp
  {
    // The opponent's movement is reversed
    public string Name => "Photon Reversifier Curse";
    public Rarity Rarity => Rarity.Rare;

    public void Apply(Player player, Player otherPlayer)
    {
      otherPlayer.PlayerMovementDelegate.PlayerMove += ReverseMovement;
    }

    public static void ReverseMovement(Events.PlayerMovementEvent movementEvent)
    {
      movementEvent._inputDirection.X *= -1;
    }
  }

  public class PheedingPhrenzy : IPowerUp
  {
    // Hurting the opponent grows your photons
    public string Name => "Pheeding Phrenzy";
    public Rarity Rarity => Rarity.Legendary;
    public int _photonDamage;
    public float _photonSize;

    public void Apply(Player player, Player otherPlayer)
    {
      otherPlayer.PlayerHurt += IncreasePhotonSize;
      player.Gun.GunShoot += ApplyPhotonSize;
    }

    public void ApplyPhotonSize(Events.GunFireEvent shootEvent)
    {
      shootEvent._bulletDamage += _photonDamage;
      shootEvent._bulletSizeFactor += _photonSize;
    }

    public void IncreasePhotonSize(Player player, int damage, Events.PlayerHurtEvent playerHurtEvent)
    {
      _photonDamage++;
      _photonSize += 0.5f;
    }
  }

  public class Randomizer5000 : IPowerUp
  {
    // Photons are randomized
    public string Name => "Randomizer 5000";
    public Rarity Rarity => Rarity.Common;
    private readonly Random _rnd = new();

    public void Apply(Player player, Player otherPlayer)
    {
      player.Gun.GunShoot += ApplyRandomization;
    }

    public void ApplyRandomization(Events.GunFireEvent shootEvent)
    {
      shootEvent._bulletCount += _rnd.Next(0, 3);
      shootEvent._bulletDamage += _rnd.Next(-2, 2);
      shootEvent._bulletGravity += (float)_rnd.NextDouble();
      shootEvent._bulletSizeFactor += (float)_rnd.NextDouble() * _rnd.Next(-1, 2);
      shootEvent._bulletSpeed += _rnd.Next(-100, 100);
      shootEvent._bulletSpread += (float)_rnd.NextDouble();
    }
  }
}
