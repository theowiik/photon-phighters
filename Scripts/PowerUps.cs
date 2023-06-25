using System;
using System.Collections.Generic;
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
    int TimesTakenBy(Player player);
    void Apply(Player playerWhoSelected, Player otherPlayer);
  }
  
  public abstract class AbstractPowerUpApplier : IPowerUpApplier
  {
    public int TimesTakenBy(Player player) => _haveTaken.Count(p => p == player);
    private readonly IList<Player> _haveTaken = new List<Player>();
    public abstract string Name { get; }
    public abstract Rarity Rarity { get; }

    public void Apply(Player playerWhoSelected, Player otherPlayer)
    {
      _haveTaken.Add(playerWhoSelected);
      _Apply(playerWhoSelected, otherPlayer);
    }

    protected abstract void _Apply(Player playerWhoSelected, Player otherPlayer);
  }

  public class AirWalker : AbstractPowerUpApplier
  {
    public override string Name => "Air Walker";
    public override Rarity Rarity => Rarity.Common;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      var times = TimesTakenBy(playerWhoSelected);

      playerWhoSelected.PlayerMovementDelegate.MaxJumps++;
    }
  }

  public class BunnyBoost : AbstractPowerUpApplier
  {
    public override string Name => "Bunny Boost";
    public override Rarity Rarity => Rarity.Common;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.PlayerMovementDelegate.JumpForce += 300;
    }
  }

  public class GeneratorEngine : AbstractPowerUpApplier
  {
    public override string Name => "Generator Engine";

    public override Rarity Rarity => Rarity.Rare;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.FireRate += 0.9f;
      playerWhoSelected.Gun.BulletSpread *= 1.06f;
    }
  }

  public class Gravitronizer : AbstractPowerUpApplier
  {
    public override string Name => "Gravitronizer";

    public override Rarity Rarity => Rarity.Common;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.BulletGravity = 0.0f;
    }
  }

  public class HealthBoost : AbstractPowerUpApplier
  {
    public override string Name => "Health Boost";

    public override Rarity Rarity => Rarity.Common;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.MaxHealth = (int)(playerWhoSelected.MaxHealth * 1.5f);
    }
  }

  public class PhotonAccelerator : AbstractPowerUpApplier
  {
    public override string Name => "Photon Accelerator";

    public override Rarity Rarity => Rarity.Common;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.BulletSpeed += 300.0f;
      playerWhoSelected.Gun.BulletSpread *= 1.05f;
    }
  }

  public class PhotonBoost : AbstractPowerUpApplier
  {
    public override string Name => "Photon Boost";

    public override Rarity Rarity => Rarity.Common;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.PlayerMovementDelegate.Speed += 200;
    }
  }

  public class PhotonEnlarger : AbstractPowerUpApplier
  {
    public override string Name => "Photon Enlarger";

    public override Rarity Rarity => Rarity.Common;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.BulletSizeFactor += 1.5f;
      playerWhoSelected.Gun.BulletDamage = Mathf.RoundToInt(playerWhoSelected.Gun.BulletDamage * 1.333f);
      playerWhoSelected.Gun.BulletSpeed -= 150.0f;
      playerWhoSelected.Gun.BulletSpread *= 1.25f;
    }
  }

  public class PhotonMultiplier : AbstractPowerUpApplier
  {
    public override string Name => "Photon Multiplier";

    public override Rarity Rarity => Rarity.Rare;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.BulletCount = (int)Math.Ceiling(playerWhoSelected.Gun.BulletCount * 1.5f);
      playerWhoSelected.Gun.BulletSpread *= 1.05f;
    }
  }

  public class PhotonMuncher : AbstractPowerUpApplier
  {
    public override string Name => "Mega Photon Muncher";

    public override Rarity Rarity => Rarity.Rare;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.MaxHealth = Mathf.RoundToInt(playerWhoSelected.MaxHealth * 1.42f);
      playerWhoSelected.PlayerMovementDelegate.Speed -= -200.0f;
      playerWhoSelected.Gun.BulletSpread *= 1.2f;
    }
  }

  public class MiniGun : AbstractPowerUpApplier
  {
    public override string Name => "1 000 000 lumen";

    public override Rarity Rarity => Rarity.Legendary;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.BulletCount += 4;
      playerWhoSelected.Gun.BulletDamage = 1;
      playerWhoSelected.Gun.BulletSpread += 0.3f;
      playerWhoSelected.Gun.BulletSpeed /= 1.4f;
      playerWhoSelected.Gun.FireRate += 1.8f;
    }
  }

  public class SteelBootsCurse : AbstractPowerUpApplier
  {
    public override string Name => "Steel Boots Curse";
    public override Rarity Rarity => Rarity.Rare;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      otherPlayer.PlayerMovementDelegate.JumpForce /= 1.33f;
    }
  }

  // TODO: Make the player thicc
  // TODO: Add sticky sound effect when walking
  // TODO: Sticky shoot sounds
  public class StickyThickyCurse : AbstractPowerUpApplier
  {
    public override string Name => "Sticky Thicky Curse";
    public override Rarity Rarity => Rarity.Legendary;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      otherPlayer.PlayerMovementDelegate.Speed /= 2;
      otherPlayer.PlayerMovementDelegate.Acceleration /= 3;
      otherPlayer.PlayerMovementDelegate.JumpForce /= 2;
      otherPlayer.MaxHealth += 50; // TODO: Possibly make it relative to the player's max health
    }
  }

  public class MomentumMaster : AbstractPowerUpApplier
  {
    public override string Name => "Momentum Master";

    public override Rarity Rarity => Rarity.Rare;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.PlayerMovementDelegate.Speed += 300;
      playerWhoSelected.PlayerMovementDelegate.Acceleration += 6;
    }
  }

  public class BulletRain : AbstractPowerUpApplier
  {
    public override string Name => "Bullet Rain";

    public override Rarity Rarity => Rarity.Rare;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.BulletCount *= 2; // Double the bullets
      playerWhoSelected.Gun.BulletGravity *= 2; // Bullets drop faster
    }
  }

  public class WallSpider : AbstractPowerUpApplier
  {
    // Wall-jumping briefly increases movement speed
    public override string Name => "Wall Spider";
    public override Rarity Rarity => Rarity.Common;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
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

  public class OingoBoingoCurse : AbstractPowerUpApplier
  {
    // Opponent is always bouncing
    public override string Name => "Oingo Boingo Curse";
    public override Rarity Rarity => Rarity.Rare;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      otherPlayer.PlayerMovementDelegate.PlayerLand += _ApplyBounce;
    }

    private static void _ApplyBounce(PlayerMovementEvent playerMovementEvent)
    {
      playerMovementEvent.Velocity = new Vector2(playerMovementEvent.Velocity.X, -500);
    }
  }

  public class Chronostasis : AbstractPowerUpApplier
  {
    // Photons briefly freeze the opponent
    public override string Name => "Chronostasis";
    public override Rarity Rarity => Rarity.Epic;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
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
        if (currentTimeMsec - _msecSinceLastFreeze >= 400)
        {
          return;
        }

        movementEvent.CanMove = false;
        movementEvent.CanJump = false;
      }

      private void RecordTimeSinceFreeze(Player player, int damage, PlayerHurtEvent playerHurtEvent)
      {
        _msecSinceLastFreeze = Time.GetTicksMsec();
      }
    }
  }

  public class EliasSpecialSauce : AbstractPowerUpApplier
  {
    public override string Name => "Elias' Special Sauce";

    public override Rarity Rarity => Rarity.Epic;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.FireRate = 11;
      playerWhoSelected.Gun.BulletDamage = 4;
      playerWhoSelected.Gun.BulletCount = 1;
      playerWhoSelected.Gun.BulletSpread = 0;
    }
  }

  public class BrownianMotionCurse : AbstractPowerUpApplier
  {
    // Technically stateful but does not deserve its own class
    private readonly Random _rnd = new();

    // Opponent's photons move erratically
    public override string Name => "Brownian Motion Curse";
    public override Rarity Rarity => Rarity.Rare;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      otherPlayer.Gun.BulletFlying += RandomizeDirection;
    }

    private void RandomizeDirection(BulletEvent bulletFlyingEvent)
    {
      bulletFlyingEvent.Velocity += new Vector2(_rnd.Next(-150, 150), _rnd.Next(-150, 150));
    }
  }

  public class FluorescentBurst : AbstractPowerUpApplier
  {
    // Getting hurt briefly increases movement speed
    public override string Name => "Fluorescent Burst";
    public override Rarity Rarity => Rarity.Common;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
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

  public class SimpleTrigonometry : AbstractPowerUpApplier
  {
    // Photons move toward the other player
    public override string Name => "Simple Trigonometry";
    public override Rarity Rarity => Rarity.Rare;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
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
        const int AttractionStrenth = 20;
        bulletEvent.Velocity += vector.Normalized() * AttractionStrenth;
      }
    }
  }

  public class PhotonReversifierCurse : AbstractPowerUpApplier
  {
    // The opponent's movement is reversed
    public override string Name => "Photon Reversifier Curse";
    public override Rarity Rarity => Rarity.Epic;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      otherPlayer.PlayerMovementDelegate.PlayerMove += ReverseMovement;
    }

    private static void ReverseMovement(PlayerMovementEvent movementEvent)
    {
      movementEvent.InputDirection *= new Vector2(-1, 1);
    }
  }

  public class PheedingPhrenzy : AbstractPowerUpApplier
  {
    // Hurting the opponent grows your photons
    public override string Name => "Pheeding Phrenzy";
    public override Rarity Rarity => Rarity.Legendary;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
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
        playerWhoSelected.Gun.GunShoot += _ApplyPhotonSize;
      }

      private void _ApplyPhotonSize(GunFireEvent shootEvent)
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

  public class Randomizer5000 : AbstractPowerUpApplier
  {
    // Technically stateful but does not deserve its own class
    private readonly Random _rnd = new();

    // Photons are randomized
    public override string Name => "Randomizer 5000";
    public override Rarity Rarity => Rarity.Common;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Gun.GunShoot += _ApplyRandomization;
    }

    private void _ApplyRandomization(GunFireEvent shootEvent)
    {
      shootEvent.BulletCount += _rnd.Next(0, 3);
      shootEvent.BulletDamage += _rnd.Next(-2, 2);
      shootEvent.BulletGravity += (float)_rnd.NextDouble();
      shootEvent.BulletSizeFactor += (float)_rnd.NextDouble() * _rnd.Next(-1, 2);
      shootEvent.BulletSpeed += _rnd.Next(-100, 100);
      shootEvent.BulletSpread += (float)_rnd.NextDouble();
    }
  }

  public class PhotonPhlyer : AbstractPowerUpApplier
  {
    // Player can fly
    public override string Name => "Photon Phlyer";
    public override Rarity Rarity => Rarity.Legendary;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
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

  public class NikeAirJordans : AbstractPowerUpApplier
  {
    // Player can double tap to dash
    public override string Name => "Nike Air Jordans";
    public override Rarity Rarity => Rarity.Common;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
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

  public class FakeJordans : AbstractPowerUpApplier
  {
    // Messes with the opponents movement
    public override string Name => "Fake Jordans";
    public override Rarity Rarity => Rarity.Rare;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      otherPlayer.PlayerMovementDelegate.Speed -= 75.0f;
      otherPlayer.PlayerMovementDelegate.JumpForce -= 100.0f;
    }
  }

  public class SketchyPillsGood : AbstractPowerUpApplier
  {
    // Makes the player smaller and faster (gamba)
    public override string Name => "Sketchy Pills";
    public override Rarity Rarity => Rarity.Rare;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Scale *= new Vector2(0.5f, 0.5f);
      playerWhoSelected.PlayerMovementDelegate.Speed += 200;
    }
  }

  public class SketchyPillsBad : AbstractPowerUpApplier
  {
    // Makes the player bigger and slower (gamba)
    public override string Name => "Sketchy Pills";
    public override Rarity Rarity => Rarity.Rare;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      playerWhoSelected.Scale *= new Vector2(1.25f, 1.25f);
      playerWhoSelected.PlayerMovementDelegate.Speed -= 100;
    }
  }

  public class BerserkerJuice : AbstractPowerUpApplier
  {
    // When below 50% HP, grants bonus stats
    public override string Name => "Berserker Juice";
    public override Rarity Rarity => Rarity.Epic;

    protected override void _Apply(Player playerWhoSelected, Player otherPlayer)
    {
      new StatefulBerserkerJuice().Apply(playerWhoSelected);
    }

    private class StatefulBerserkerJuice
    {
      private const float Treshold = 0.666f;
      private Player _player;

      public void Apply(Player playerWhoSelected)
      {
        _player = playerWhoSelected;
        playerWhoSelected.PlayerMovementDelegate.PlayerMove += IncreaseSpeed;
        playerWhoSelected.PlayerMovementDelegate.PlayerJump += IncreaseJump;
        playerWhoSelected.Gun.GunShoot += IncreaseDamage;
      }

      private void IncreaseSpeed(PlayerMovementEvent playerMovementEvent)
      {
        if (_player.Health < _player.MaxHealth * Treshold)
        {
          playerMovementEvent.Speed += 150;
        }
      }

      private void IncreaseJump(PlayerMovementEvent playerMovementEvent)
      {
        if (_player.Health <= _player.MaxHealth * Treshold)
        {
          return;
        }

        playerMovementEvent.JumpForce += 100;
        playerMovementEvent.MaxJumps++;
      }

      private void IncreaseDamage(GunFireEvent shootEvent)
      {
        if (_player.Health >= _player.MaxHealth * Treshold)
        {
          return;
        }

        shootEvent.BulletDamage += 5;
        shootEvent.BulletCount += 1;
      }
    }
  }
}
