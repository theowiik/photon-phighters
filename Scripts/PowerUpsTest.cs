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

  public class SpeedyGonzales : IPowerUpTest
  {
    public string Name => "Speedy Gonzales";
    public string Description => "Walljumping briefly increases movement speed";
    public RarityTest RarityTest => RarityTest.Common;
    private ulong MsecSinceLastWallJump = 0;

    public void Apply(Player player, Player otherPlayer)
    {
      player.PlayerMovementDelegate.PlayerMovement += GiveSpeedBoost;
      player.PlayerMovementDelegate.PlayerWallJumped += RecordTimeSinceWallJump;
    }

    public void GiveSpeedBoost(Events.PlayerMovementEvent playerMovementEvent)
    {
      ulong CurrentTimeMsec = Time.GetTicksMsec();
      if (CurrentTimeMsec - MsecSinceLastWallJump < 5000)
      {
        playerMovementEvent.Speed *= 2;
      }
    }

    public void RecordTimeSinceWallJump(Events.PlayerMovementEvent playerMovementEvent)
    {
      MsecSinceLastWallJump = Time.GetTicksMsec();
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
      player.PlayerMovementDelegate.PlayerJumped += DelayJump;
    }

    public void DelayJump(Events.PlayerMovementEvent playerMovementEvent)
    {
      ulong CurrentTimeMsec = Time.GetTicksMsec();
      if (CurrentTimeMsec - MsecSinceLastJump < 2000)
      {
        playerMovementEvent.MaxJumps = 0;
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
      player.Gun.BulletCollideFloorDelegate += ScatterPhotons;
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

    public void Apply(Player player, Player otherPlayer)
    {
      player.Gun.BulletFlyingDelegate += RandomizeDirection;
    }

    public void RandomizeDirection(Events.BulletFlyingEvent bulletFlyingEvent)
    {
      // TODO, get velocity and stuff into bullet event
      // And randomize direction every so often
    }
  }
}
