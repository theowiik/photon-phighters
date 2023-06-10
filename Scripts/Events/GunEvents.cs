using System;
using Godot;

namespace PhotonPhighters.Scripts;

public partial class GunEvents
{
  public partial class ShootEvent : Node
  {
    public int BulletCount;
    public int BulletDamage;
    public float BulletGravity;
    public float BulletSizeFactor;
    public float BulletSpeed;
    public float BulletSpread;
    public float FireRate;

    public ShootEvent(
      int BulletCount,
      int BulletDamage,
      float BulletGravity,
      float BulletSizeFactor,
      float BulletSpeed,
      float BulletSpread,
      float FireRate
    )
    {
      this.BulletCount = BulletCount;
      this.BulletDamage = BulletDamage;
      this.BulletGravity = BulletGravity;
      this.BulletSizeFactor = BulletSizeFactor;
      this.BulletSpeed = BulletSpeed;
      this.BulletSpread = BulletSpread;
      this.FireRate = FireRate;
    }
  }
}
