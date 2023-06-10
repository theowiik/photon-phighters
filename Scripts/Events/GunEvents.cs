using System;
using Godot;

namespace PhotonPhighters.Scripts;

public partial class GunEvents
{
  public partial class ShootEvent : Node
  {
    public int BulletDamage;
    public float BulletGravity;
    public float BulletSizeFactor;
    public float BulletSpeed;
    public float BulletSpread;

    public ShootEvent(
      int BulletDamage,
      float BulletGravity,
      float BulletSizeFactor,
      float BulletSpeed,
      float BulletSpread
    )
    {
      this.BulletDamage = BulletDamage;
      this.BulletGravity = BulletGravity;
      this.BulletSizeFactor = BulletSizeFactor;
      this.BulletSpeed = BulletSpeed;
      this.BulletSpread = BulletSpread;
    }
  }
}
