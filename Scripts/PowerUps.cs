using System;
using System.Linq;
using Godot;
using PhotonPhighters.Scripts.Events;

namespace PhotonPhighters.Scripts;

public static class PowerUps
{
  /// <summary>
  ///   Different power up rarities, and their percentage chance of being selected.
  ///   Must add up to exactly 100.
  /// </summary>
  public enum Rarity
  {
    Legendary = 2,
    Epic = 8,
    Rare = 25,
    Common = 65
  }

  public static bool RaritySumIs100 => Enum.GetValues(typeof(Rarity)).Cast<Rarity>().Sum(rarity => (int)rarity) == 100;

  /// <summary>
  ///   Stateless power up applier. Creates a new instance of the power up every time it is applied.
  /// </summary>
  public interface IPowerUpApplier
  {
    string Name { get; }
    Rarity Rarity { get; }
    void Apply(Player playerWhoSelected, Player otherPlayer);
  }

  public class AirWalker : IPowerUpApplier
  {
    public string Name => "Air Walker";

    public Rarity Rarity => Rarity.Common;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.PlayerMovementDelegate.MaxJumps++;
    }
  }

  public class BunnyBoost : IPowerUpApplier
  {
    public string Name => "Bunny Boost";

    public Rarity Rarity => Rarity.Common;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.PlayerMovementDelegate.JumpForce += 300;
    }
  }

  public class GeneratorEngine : IPowerUpApplier
  {
    public string Name => "Generator Engine";

    public Rarity Rarity => Rarity.Rare;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.FireRate += 0.9f;
      playerWhoSelected.Gun.BulletSpread *= 1.06f;
    }
  }

  public class Gravitronizer : IPowerUpApplier
  {
    public string Name => "Gravitronizer";

    public Rarity Rarity => Rarity.Common;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.BulletGravity = 0.0f;
    }
  }

  public class HealthBoost : IPowerUpApplier
  {
    public string Name => "Health Boost";

    public Rarity Rarity => Rarity.Common;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.MaxHealth = (int)(playerWhoSelected.MaxHealth * 1.5f);
    }
  }

  public class PhotonAccelerator : IPowerUpApplier
  {
    public string Name => "Photon Accelerator";

    public Rarity Rarity => Rarity.Common;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.BulletSpeed += 300.0f;
      playerWhoSelected.Gun.BulletSpread *= 1.05f;
    }
  }

  public class PhotonBoost : IPowerUpApplier
  {
    public string Name => "Photon Boost";

    public Rarity Rarity => Rarity.Common;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.PlayerMovementDelegate.Speed += 200;
    }
  }

  public class PhotonEnlarger : IPowerUpApplier
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

  public class PhotonMultiplier : IPowerUpApplier
  {
    public string Name => "Photon Multiplier";

    public Rarity Rarity => Rarity.Rare;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.BulletCount = (int)Math.Ceiling(playerWhoSelected.Gun.BulletCount * 1.5f);
      playerWhoSelected.Gun.BulletSpread *= 1.05f;
    }
  }

  public class PhotonMuncher : IPowerUpApplier
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

  public class MiniGun : IPowerUpApplier
  {
    public string Name => "1 000 000 lumen";

    public Rarity Rarity => Rarity.Legendary;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.BulletCount += 4;
      playerWhoSelected.Gun.BulletDamage = 1;
      playerWhoSelected.Gun.BulletSpread += 0.3f;
      playerWhoSelected.Gun.BulletSpeed /= 1.4f;
      playerWhoSelected.Gun.FireRate += 1.8f;
    }
  }

  public class Sniper : IPowerUpApplier
  {
    public string Name => "Photon Sniper";

    public Rarity Rarity => Rarity.Legendary;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.BulletCount = 1;
      playerWhoSelected.Gun.BulletDamage = 130;
      playerWhoSelected.Gun.BulletSpread = 0.00001f;
      playerWhoSelected.Gun.BulletSpeed = 3000;
      playerWhoSelected.Gun.FireRate = 2.5f;
    }
  }

  public class SteelBootsCurse : IPowerUpApplier
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
  public class StickyThickyCurse : IPowerUpApplier
  {
    public string Name => "Sticky Thicky Curse";
    public Rarity Rarity => Rarity.Legendary;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      otherPlayer.PlayerMovementDelegate.Speed /= 2;
      otherPlayer.PlayerMovementDelegate.Acceleration /= 3;
      otherPlayer.PlayerMovementDelegate.JumpForce /= 2;
      otherPlayer.MaxHealth += 50; // TODO: Possibly make it relative to the player's max health
    }
  }

  public class MomentumMaster : IPowerUpApplier
  {
    public string Name => "Momentum Master";

    public Rarity Rarity => Rarity.Rare;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.PlayerMovementDelegate.Speed += 300;
      playerWhoSelected.PlayerMovementDelegate.Acceleration += 6;
    }
  }

  public class BulletRain : IPowerUpApplier
  {
    public string Name => "Bullet Rain";

    public Rarity Rarity => Rarity.Rare;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.BulletCount *= 2; // Double the bullets
      playerWhoSelected.Gun.BulletGravity *= 2; // Bullets drop faster
    }
  }

  public class WallSpider : IPowerUpApplier
  {
    // Wall-jumping briefly increases movement speed
    public string Name => "Wall Spider";
    public Rarity Rarity => Rarity.Common;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      new StatefulWallSpider().Apply(playerWhoSelected);
    }

    private class StatefulWallSpider
    {
      private ulong _msecSinceLastWallJump;

      private void GiveSpeedBoost(PlayerMovementEvent playerMovementEvent)
      {
        var currentTimeMsec = Time.GetTicksMsec();
        if (currentTimeMsec - _msecSinceLastWallJump < 6000)
        {
          playerMovementEvent.Speed *= 1.5f;
        }
      }

      private void RecordTimeSinceWallJump(PlayerMovementEvent playerMovementEvent)
      {
        _msecSinceLastWallJump = Time.GetTicksMsec();
      }

      public void Apply(Player playerWhoSelected)
      {
        playerWhoSelected.PlayerMovementDelegate.PlayerMove += GiveSpeedBoost;
        playerWhoSelected.PlayerMovementDelegate.PlayerWallJump += RecordTimeSinceWallJump;
      }
    }
  }

  public class OingoBoingoCurse : IPowerUpApplier
  {
    // Opponent is always bouncing
    public string Name => "Oingo Boingo Curse";
    public Rarity Rarity => Rarity.Rare;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      otherPlayer.PlayerMovementDelegate.PlayerLand += ApplyBounce;
    }

    private static void ApplyBounce(PlayerMovementEvent playerMovementEvent)
    {
      playerMovementEvent.Velocity = new Vector2(playerMovementEvent.Velocity.X, -500);
    }
  }

  public class PostLegDayCurse : IPowerUpApplier
  {
    // Opponent has to briefly rest between jumps
    public string Name => "Post Leg Day Curse";
    public Rarity Rarity => Rarity.Epic;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      new StatefulPostLegDayCurse().Apply(otherPlayer);
    }

    private class StatefulPostLegDayCurse
    {
      private ulong _msecSinceLastJump;

      private void DelayJump(PlayerMovementEvent playerMovementEvent)
      {
        var currentTimeMsec = Time.GetTicksMsec();
        if (currentTimeMsec - _msecSinceLastJump < 1000)
        {
          playerMovementEvent.CanJump = false;
        }
        else
        {
          _msecSinceLastJump = currentTimeMsec;
        }
      }

      public void Apply(Player otherPlayer)
      {
        otherPlayer.PlayerMovementDelegate.PlayerJump += DelayJump;
      }
    }
  }

  public class Chronostasis : IPowerUpApplier
  {
    // Photons briefly freeze the opponent
    public string Name => "Chronostasis";
    public Rarity Rarity => Rarity.Epic;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      new StatefulChronostasis().Apply(otherPlayer);
    }

    private class StatefulChronostasis
    {
      private ulong _msecSinceLastFreeze;

      public void Apply(Player otherPlayer)
      {
        otherPlayer.PlayerHurt += RecordTimeSinceFreeze;
        otherPlayer.PlayerMovementDelegate.PlayerMove += FreezePlayer;
      }

      private void FreezePlayer(PlayerMovementEvent movementEvent)
      {
        var currentTimeMsec = Time.GetTicksMsec();
        if (currentTimeMsec - _msecSinceLastFreeze < 400)
        {
          movementEvent.CanMove = false;
          movementEvent.CanJump = false;
        }
      }

      private void RecordTimeSinceFreeze(Player player, int damage, PlayerHurtEvent playerHurtEvent)
      {
        _msecSinceLastFreeze = Time.GetTicksMsec();
      }
    }
  }

  public class EliasSpecialSauce : IPowerUpApplier
  {
    public string Name => "Elias' Special Sauce";

    public Rarity Rarity => Rarity.Epic;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.FireRate = 11;
      playerWhoSelected.Gun.BulletDamage = 4;
      playerWhoSelected.Gun.BulletCount = 1;
      playerWhoSelected.Gun.BulletSpread = 0;
    }
  }

  public class BrownianMotionCurse : IPowerUpApplier
  {
    // Technically stateful but does not deserve its own class
    private readonly Random _rnd = new();

    // Opponent's photons move erratically
    public string Name => "Brownian Motion Curse";
    public Rarity Rarity => Rarity.Rare;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      otherPlayer.Gun.BulletFlying += RandomizeDirection;
    }

    private void RandomizeDirection(BulletEvent bulletFlyingEvent)
    {
      bulletFlyingEvent.Velocity += new Vector2(_rnd.Next(-150, 150), _rnd.Next(-150, 150));
    }
  }

  public class FluorescentBurst : IPowerUpApplier
  {
    // Getting hurt briefly increases movement speed
    public string Name => "Fluorescent Burst";
    public Rarity Rarity => Rarity.Common;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      new StatefulFluorescentBurst().Apply(playerWhoSelected);
    }

    private class StatefulFluorescentBurst
    {
      private ulong _msecSinceLastHurt;

      public void Apply(Player playerWhoSelected)
      {
        playerWhoSelected.PlayerMovementDelegate.PlayerMove += GiveSpeedBoost;
        playerWhoSelected.PlayerHurt += RecordTimeSinceHurt;
      }

      private void GiveSpeedBoost(PlayerMovementEvent playerMovementEvent)
      {
        var currentTimeMsec = Time.GetTicksMsec();
        if (currentTimeMsec - _msecSinceLastHurt < 2000)
        {
          playerMovementEvent.Speed *= 1.5f;
        }
      }

      private void RecordTimeSinceHurt(Player player, int damage, PlayerHurtEvent playerHurtEvent)
      {
        _msecSinceLastHurt = Time.GetTicksMsec();
      }
    }
  }

  public class SimpleTrigonometry : IPowerUpApplier
  {
    // Photons move toward the other player
    public string Name => "Simple Trigonometry";
    public Rarity Rarity => Rarity.Rare;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      new StatefulSimpleTrigonometry().Apply(playerWhoSelected, otherPlayer);
    }

    private class StatefulSimpleTrigonometry
    {
      private Player OtherPlayer { get; set; }

      public void Apply(Player playerWhoSelected, Player otherPlayer)
      {
        playerWhoSelected.Gun.BulletSpeed /= 1.5f;
        OtherPlayer = otherPlayer;
        playerWhoSelected.Gun.BulletFlying += MoveToOtherPlayer;
      }

      private void MoveToOtherPlayer(BulletEvent bulletEvent)
      {
        var vector = OtherPlayer.Position - bulletEvent.Area2D.Position;
        var attractionStrenth = 20;
        bulletEvent.Velocity += vector.Normalized() * attractionStrenth;
      }
    }
  }

  public class LuminogravitonFluxCurse : IPowerUpApplier
  {
    // The opponent's gravity is reversed
    public string Name => "Luminograviton Flux Curse";
    public Rarity Rarity => Rarity.Legendary;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      otherPlayer.Rotate((float)Math.PI);
      otherPlayer.PlayerMovementDelegate.PlayerMove += ReverseGravity;
    }

    private static void ReverseGravity(PlayerMovementEvent movementEvent)
    {
      movementEvent.Gravity *= -1;
      movementEvent.JumpForce *= -1;
    }
  }

  public class PhotonReversifierCurse : IPowerUpApplier
  {
    // The opponent's movement is reversed
    public string Name => "Photon Reversifier Curse";
    public Rarity Rarity => Rarity.Epic;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      otherPlayer.PlayerMovementDelegate.PlayerMove += ReverseMovement;
    }

    private static void ReverseMovement(PlayerMovementEvent movementEvent)
    {
      movementEvent.InputDirection *= new Vector2(-1, 1);
    }
  }

  public class PheedingPhrenzy : IPowerUpApplier
  {
    // Hurting the opponent grows your photons
    public string Name => "Pheeding Phrenzy";
    public Rarity Rarity => Rarity.Legendary;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      new StatefulPheedingPhrenzy().Apply(playerWhoSelected, otherPlayer);
    }

    private class StatefulPheedingPhrenzy
    {
      private int _photonDamage;
      private float _photonSize;

      public void Apply(Player playerWhoSelected, Player otherPlayer)
      {
        otherPlayer.PlayerHurt += IncreasePhotonSize;
        playerWhoSelected.Gun.GunShoot += ApplyPhotonSize;
      }

      private void ApplyPhotonSize(GunFireEvent shootEvent)
      {
        shootEvent.BulletDamage += _photonDamage;
        shootEvent.BulletSizeFactor += _photonSize;
      }

      private void IncreasePhotonSize(Player player, int damage, PlayerHurtEvent playerHurtEvent)
      {
        _photonDamage = Math.Min(_photonDamage + 1, 30);
        _photonSize = MathF.Min(_photonSize + 0.05f, 10f);
      }
    }
  }

  public class Randomizer5000 : IPowerUpApplier
  {
    // Technically stateful but does not deserve its own class
    private readonly Random _rnd = new();

    // Photons are randomized
    public string Name => "Randomizer 5000";
    public Rarity Rarity => Rarity.Common;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.GunShoot += ApplyRandomization;
    }

    private void ApplyRandomization(GunFireEvent shootEvent)
    {
      shootEvent.BulletCount += _rnd.Next(0, 3);
      shootEvent.BulletDamage += _rnd.Next(-2, 2);
      shootEvent.BulletGravity += (float)_rnd.NextDouble();
      shootEvent.BulletSizeFactor += (float)_rnd.NextDouble() * _rnd.Next(-1, 2);
      shootEvent.BulletSpeed += _rnd.Next(-100, 100);
      shootEvent.BulletSpread += (float)_rnd.NextDouble();
    }
  }

  public class PhotonPhlyer : IPowerUpApplier
  {
    // Player can fly
    public string Name => "Photon Phlyer";
    public Rarity Rarity => Rarity.Legendary;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.PlayerMovementDelegate.PlayerMove += DisableGravity;
    }

    private static void DisableGravity(PlayerMovementEvent playerMovementEvent)
    {
      playerMovementEvent.CanJump = false;
      playerMovementEvent.Gravity = 0;
      playerMovementEvent.Velocity = playerMovementEvent.InputDirection * playerMovementEvent.Speed;
    }
  }

  public class NikeAirJordans : IPowerUpApplier
  {
    // Player can double tap to dash
    public string Name => "Nike Air Jordans";
    public Rarity Rarity => Rarity.Common;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.PlayerMovementDelegate.PlayerDoubleTapped += Dash;
    }

    private static void Dash(PlayerMovementEvent playerMovementEvent)
    {
      playerMovementEvent.Velocity += new Vector2(
        playerMovementEvent.InputDirection.X * (playerMovementEvent.Speed * 7),
        0
      );
    }
  }

  public class FakeJordans : IPowerUpApplier
  {
    // Messes with the opponents movement
    public string Name => "Fake Jordans";
    public Rarity Rarity => Rarity.Rare;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      otherPlayer.PlayerMovementDelegate.Speed -= 75.0f;
      otherPlayer.PlayerMovementDelegate.JumpForce -= 100.0f;
    }
  }

  public class SketchyPillsGood : IPowerUpApplier
  {
    // Makes the player smaller and faster (gamba)
    public string Name => "Sketchy Pills";
    public Rarity Rarity => Rarity.Rare;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Scale *= new Vector2(0.5f, 0.5f);
      playerWhoSelected.PlayerMovementDelegate.Speed += 200;
    }
  }

  public class SketchyPillsBad : IPowerUpApplier
  {
    // Makes the player bigger and slower (gamba)
    public string Name => "Sketchy Pills";
    public Rarity Rarity => Rarity.Rare;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Scale *= new Vector2(1.25f, 1.25f);
      playerWhoSelected.PlayerMovementDelegate.Speed -= 100;
    }
  }

  public class BerserkerJuice : IPowerUpApplier
  {
    // When below 50% HP, grants bonus stats
    public string Name => "Berserker Juice";
    public Rarity Rarity => Rarity.Epic;

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      new StatefulBerserkerJuice().Apply(playerWhoSelected);
    }

    private class StatefulBerserkerJuice
    {
      private Player _player;
      private float _threshold = 0.666f;

      public void Apply(Player playerWhoSelected)
      {
        _player = playerWhoSelected;
        playerWhoSelected.PlayerMovementDelegate.PlayerMove += IncreaseSpeed;
        playerWhoSelected.PlayerMovementDelegate.PlayerJump += IncreaseJump;
        playerWhoSelected.Gun.GunShoot += IncreaseDamage;
      }

      private void IncreaseSpeed(PlayerMovementEvent playerMovementEvent)
      {
        if (_player.Health < (_player.MaxHealth * _threshold))
        {
          playerMovementEvent.Speed += 150;
        }
      }

      private void IncreaseJump(PlayerMovementEvent playerMovementEvent)
      {
        if (_player.Health < (_player.MaxHealth * _threshold))
        {
          playerMovementEvent.JumpForce += 100;
          playerMovementEvent.MaxJumps++;
        }
      }

      private void IncreaseDamage(GunFireEvent shootEvent)
      {
        if (_player.Health < (_player.MaxHealth * _threshold))
        {
          shootEvent.BulletDamage += 5;
          shootEvent.BulletCount += 1;
        }
      }
    }
  }
}
