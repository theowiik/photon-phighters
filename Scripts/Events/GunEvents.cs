using System;
using Godot;

namespace PhotonPhighters.Scripts.Events;

public partial class GunFireEvent : Node
{
  public int BulletCount { get; set; }
  public int BulletDamage { get; set; }
  public float BulletGravity { get; set; }
  public float BulletSizeFactor { get; set; }
  public float BulletSpeed { get; set; }
  public float BulletSpread { get; set; }

  public GunFireEvent(
    int bulletCount,
    int bulletDamage,
    float bulletGravity,
    float bulletSizeFactor,
    float bulletSpeed,
    float bulletSpread
  )
  {
    BulletCount = bulletCount;
    BulletDamage = bulletDamage;
    BulletGravity = bulletGravity;
    BulletSizeFactor = bulletSizeFactor;
    BulletSpeed = bulletSpeed;
    BulletSpread = bulletSpread;
  }
}
