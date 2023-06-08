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

    public void GiveSpeedBoost(Events.PlayerMoveEvent playerMovementEvent)
    {
      ulong CurrentTimeMsec = Time.GetTicksMsec();
      if (CurrentTimeMsec - MsecSinceLastWallJump < 5000)
      {
        playerMovementEvent.Speed *= 1.25f;
      }
    }

    public void RecordTimeSinceWallJump(Events.PlayerMoveEvent playerMovedEvent)
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
      player.PlayerMovementDelegate.PlayerLand += ApplyBounce;
    }

    public void ApplyBounce(Events.PlayerMoveEvent playerMoveEvent)
    {
      playerMoveEvent.Velocity.Y -= 1000;
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
      player.PlayerMovementDelegate.PlayerJump += DelayJump;
    }

    public void DelayJump(Events.PlayerMoveEvent playerMovementEvent)
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

  public class RayleighScattering : IPowerUpTest
  {
    public string Name => "Rayleigh Scattering";
    public string Description => "Photons scatter when hitting surfaces";
    public RarityTest RarityTest => RarityTest.Rare;

    public void Apply(Player player, Player otherPlayer)
    {
      player.Gun.BulletCollideFloor += ScatterPhotons;
    }

    public void ScatterPhotons(Events.BulletCollideFloorEvent bulletCollidePlayerEvent)
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

    public void RandomizeDirection(Events.BulletEvent bulletFlyingEvent)
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
}
