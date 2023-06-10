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
    private ulong MsecSinceLastWallJump = 0;

    public void Apply(Player player, Player otherPlayer)
    {
      player.PlayerMovementDelegate.PlayerMove += GiveSpeedBoost;
      player.PlayerMovementDelegate.PlayerWallJump += RecordTimeSinceWallJump;
    }

    public void GiveSpeedBoost(MovementEvents.PlayerMovementEvent playerMovementEvent)
    {
      ulong CurrentTimeMsec = Time.GetTicksMsec();
      if (CurrentTimeMsec - MsecSinceLastWallJump < 3000)
      {
        playerMovementEvent.Speed *= 1.5f;
      }
    }

    public void RecordTimeSinceWallJump(MovementEvents.PlayerMovementEvent playerMovementEvent)
    {
      MsecSinceLastWallJump = Time.GetTicksMsec();
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

    public void ApplyBounce(MovementEvents.PlayerMovementEvent playerMovementEvent)
    {
      playerMovementEvent.Velocity.Y -= 1000;
    }
  }

  public class PostLegDayCurse : IPowerUp
  {
    // Opponent has to briefly rest between jumps
    public string Name => "Post Leg Day Curse";
    public Rarity Rarity => Rarity.Rare;
    private ulong MsecSinceLastJump = 0;

    public void Apply(Player player, Player otherPlayer)
    {
      otherPlayer.PlayerMovementDelegate.PlayerJump += DelayJump;
    }

    public void DelayJump(MovementEvents.PlayerMovementEvent playerMovementEvent)
    {
      ulong CurrentTimeMsec = Time.GetTicksMsec();
      if (CurrentTimeMsec - MsecSinceLastJump < 2000)
      {
        playerMovementEvent.CanJump = false;
      }
      else
      {
        MsecSinceLastJump = CurrentTimeMsec;
      }
    }
  }

  public class Chronostasis : IPowerUp
  {
    // Photons briefly freeze the opponent
    public string Name => "Chronostasis";
    public Rarity Rarity => Rarity.Legendary;
    public ulong MsecSinceLastFreeze = 0;

    public void Apply(Player player, Player otherPlayer)
    {
      otherPlayer.PlayerHurt += RecordTimeSinceFreeze;
      otherPlayer.PlayerMovementDelegate.PlayerMove += FreezePlayer;
    }

    public void FreezePlayer(MovementEvents.PlayerMovementEvent movementEvent)
    {
      ulong CurrentTimeMsec = Time.GetTicksMsec();
      if (CurrentTimeMsec - MsecSinceLastFreeze < 5000)
      {
        movementEvent.CanMove = false;
        movementEvent.CanJump = false;
      }
    }

    public void RecordTimeSinceFreeze(Player player, int damage, PlayerEvents.PlayerHurtEvent playerHurtEvent)
    {
      MsecSinceLastFreeze = Time.GetTicksMsec();
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
    private ulong MsecSinceRandomization = 0;
    private Random rnd = new Random();

    public void Apply(Player player, Player otherPlayer)
    {
      otherPlayer.Gun.BulletFlying += RandomizeDirection;
    }

    public void RandomizeDirection(BulletEvents.BulletEvent bulletFlyingEvent)
    {
      ulong CurrentTimeMsec = Time.GetTicksMsec();
      if (CurrentTimeMsec - MsecSinceRandomization > 100)
      {
        bulletFlyingEvent.Velocity.X += rnd.Next(-200, 200);
        bulletFlyingEvent.Velocity.Y += rnd.Next(-200, 200);
      }
      else
      {
        MsecSinceRandomization = CurrentTimeMsec;
      }
    }
  }

  public class FluorescentBurst : IPowerUp
  {
    // Getting hurt briefly increases movement speed
    public string Name => "Fluorescent Burst";
    public Rarity Rarity => Rarity.Rare;

    public ulong MsecSinceLastHurt = 0;

    public void Apply(Player player, Player otherPlayer)
    {
      player.PlayerMovementDelegate.PlayerMove += GiveSpeedBoost;
      player.PlayerHurt += RecordTimeSinceHurt;
    }

    public void GiveSpeedBoost(MovementEvents.PlayerMovementEvent playerMovementEvent)
    {
      ulong CurrentTimeMsec = Time.GetTicksMsec();
      if (CurrentTimeMsec - MsecSinceLastHurt < 1000)
      {
        playerMovementEvent.Speed *= 1.5f;
      }
    }

    public void RecordTimeSinceHurt(Player player, int damage, PlayerEvents.PlayerHurtEvent playerHurtEvent)
    {
      MsecSinceLastHurt = Time.GetTicksMsec();
    }
  }

  public class SimpleTrigonometry : IPowerUp
  {
    // Photons move toward the other player
    public string Name => "Simple Trigonometry";
    public Rarity Rarity => Rarity.Rare;

    public Player otherPlayer;

    public void Apply(Player player, Player otherPlayer)
    {
      this.otherPlayer = otherPlayer;
      player.Gun.BulletFlying += MoveToOtherPlayer;
    }

    public void MoveToOtherPlayer(BulletEvents.BulletEvent bulletEvent)
    {
      var vector = this.otherPlayer.Position - bulletEvent.Area2D.Position;
      var magnitude = vector.Length();
      bulletEvent.Velocity.X += vector.X / (magnitude / 20);
      bulletEvent.Velocity.Y += vector.Y / (magnitude / 20);
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

    public void ReverseGravity(MovementEvents.PlayerMovementEvent movementEvent)
    {
      movementEvent.Gravity *= -1;
      movementEvent.JumpForce *= -1;
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

    public void ReverseMovement(MovementEvents.PlayerMovementEvent movementEvent)
    {
      movementEvent.InputDirection.X *= -1;
    }
  }

  public class PheedingPhrenzy : IPowerUp
  {
    // Hurting the opponent grows your photons
    public string Name => "Pheeding Phrenzy";
    public Rarity Rarity => Rarity.Legendary;
    public int PhotonDamage = 0;
    public float PhotonSize = 0;

    public void Apply(Player player, Player otherPlayer)
    {
      otherPlayer.PlayerHurt += IncreasePhotonSize;
      player.Gun.GunShoot += ApplyPhotonSize;
    }

    public void ApplyPhotonSize(GunEvents.ShootEvent shootEvent)
    {
      shootEvent.BulletDamage += PhotonDamage;
      shootEvent.BulletSizeFactor += PhotonSize;
    }

    public void IncreasePhotonSize(Player player, int damage, PlayerEvents.PlayerHurtEvent playerHurtEvent)
    {
      PhotonDamage += 1;
      PhotonSize += 0.5f;
    }
  }

  public class Randomizer5000 : IPowerUp
  {
    // Photons are randomized
    public string Name => "Randomizer 5000";
    public Rarity Rarity => Rarity.Common;
    private Random rnd = new Random();

    public void Apply(Player player, Player otherPlayer)
    {
      player.Gun.GunShoot += ApplyRandomization;
    }

    public void ApplyRandomization(GunEvents.ShootEvent shootEvent)
    {
      shootEvent.BulletCount += rnd.Next(0, 3);
      shootEvent.BulletDamage += rnd.Next(-2, 2);
      shootEvent.BulletGravity += (float)rnd.NextDouble();
      shootEvent.BulletSizeFactor += (float)rnd.NextDouble() * rnd.Next(-1, 2);
      shootEvent.BulletSpeed += rnd.Next(-100, 100);
      shootEvent.BulletSpread += (float)rnd.NextDouble();
    }
  }
}
