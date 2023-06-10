using System;
using Godot;

namespace PhotonPhighters.Scripts;

public static class PowerUpsTest
{
  public enum RarityTest
  {
    Legendary = 1,
    Curse = 3,
    Rare = 6,
    Common = 11
  }

  public interface IPowerUpTest
  {
    string Name { get; }
    string Description { get; }
    RarityTest RarityTest { get; }
    void Apply(Player player, Player otherPlayer);
  }

  public class WallSpider : IPowerUpTest
  {
    public string Name => "Wall Spider";
    public string Description => "Walljumping briefly increases movement speed";
    public RarityTest RarityTest => RarityTest.Common;
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

  public class OingoBoingoCurse : IPowerUpTest
  {
    public string Name => "Oingo Boingo Curse";
    public string Description => "Opponent is always bouncing";
    public RarityTest RarityTest => RarityTest.Curse;

    public void Apply(Player player, Player otherPlayer)
    {
      otherPlayer.PlayerMovementDelegate.PlayerLand += ApplyBounce;
    }

    public void ApplyBounce(MovementEvents.PlayerMovementEvent playerMovementEvent)
    {
      playerMovementEvent.Velocity.Y -= 1000;
    }
  }

  public class PostLegDayCurse : IPowerUpTest
  {
    public string Name => "Post Leg Day Curse";
    public string Description => "Opponent has to briefly rest between jumps";
    public RarityTest RarityTest => RarityTest.Curse;
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

  public class Chronostasis : IPowerUpTest
  {
    public string Name => "Chronostasis";
    public string Description => "Photons briefly freeze the opponent";
    public RarityTest RarityTest => RarityTest.Legendary;
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

  public class RayleighScattering : IPowerUpTest
  {
    public string Name => "Rayleigh Scattering";
    public string Description => "Photons scatter when hitting surfaces";
    public RarityTest RarityTest => RarityTest.Rare;

    public void Apply(Player player, Player otherPlayer)
    {
      player.Gun.BulletCollideFloor += ScatterPhotons;
    }

    public void ScatterPhotons(BulletEvents.BulletEvent bulletCollideFloorEvent)
    {
      // TODO: Spawn bullets
    }
  }

  public class BrownianMotionCurse : IPowerUpTest
  {
    public string Name => "Brownian Motion Curse";
    public string Description => "Opponent's photons move erratically";
    public RarityTest RarityTest => RarityTest.Curse;
    private ulong MsecSinceRandomization = 0;
    private Random rnd = new Random();

    public void Apply(Player player, Player otherPlayer)
    {
      player.Gun.BulletFlying += RandomizeDirection;
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

  public class FluorescentBurst : IPowerUpTest
  {
    public string Name => "Fluorescent Burst";
    public string Description => "Getting hurt briefly increases movement speed";
    public RarityTest RarityTest => RarityTest.Rare;

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

  public class SimpleTrigonometry : IPowerUpTest
  {
    public string Name => "Simple Trigonometry";
    public string Description => "Photons move toward the other player";
    public RarityTest RarityTest => RarityTest.Rare;

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

  public class LuminogravitonFluxCurse : IPowerUpTest
  {
    public string Name => "Luminograviton Flux Curse";
    public string Description => "The opponent's gravity is reversed";
    public RarityTest RarityTest => RarityTest.Legendary;

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

  public class PhotonReversifierCurse : IPowerUpTest
  {
    public string Name => "Photon Reversifier Curse";
    public string Description => "The opponent's movement is reversed";
    public RarityTest RarityTest => RarityTest.Rare;

    public void Apply(Player player, Player otherPlayer)
    {
      otherPlayer.PlayerMovementDelegate.PlayerMove += ReverseMovement;
    }

    public void ReverseMovement(MovementEvents.PlayerMovementEvent movementEvent)
    {
      movementEvent.InputDirection.X *= -1;
    }
  }

  public class PheedingPhrenzy : IPowerUpTest
  {
    public string Name => "Pheeding Phrenzy";
    public string Description => "Hurting the opponent grows your photons";
    public RarityTest RarityTest => RarityTest.Legendary;
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

  public class Randomizer5000 : IPowerUpTest
  {
    public string Name => "Randomizer 5000";
    public string Description => "Photons are randomized";
    public RarityTest RarityTest => RarityTest.Rare;
    private Random rnd = new Random();

    public void Apply(Player player, Player otherPlayer)
    {
      player.Gun.GunShoot += ApplyRandomization;
    }

    public void ApplyRandomization(GunEvents.ShootEvent shootEvent)
    {
      shootEvent.BulletDamage += rnd.Next(-2, 2);
      shootEvent.BulletGravity += (float)rnd.NextDouble();
      shootEvent.BulletSizeFactor += (float)rnd.NextDouble() * rnd.Next(-1, 2);
      shootEvent.BulletSpeed += rnd.Next(-100, 100);
      shootEvent.BulletSpread += (float)rnd.NextDouble();
    }
  }
}
