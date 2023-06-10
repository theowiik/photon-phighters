using System;
using Godot;

namespace PhotonPhighters.Scripts.Events;

public partial class GunFireEvent : Node
{
  public int _bulletCount;
  public int _bulletDamage;
  public float _bulletGravity;
  public float _bulletSizeFactor;
  public float _bulletSpeed;
  public float _bulletSpread;

  public GunFireEvent(
    int bulletCount,
    int bulletDamage,
    float bulletGravity,
    float bulletSizeFactor,
    float bulletSpeed,
    float bulletSpread
  )
  {
    this._bulletCount = bulletCount;
    this._bulletDamage = bulletDamage;
    this._bulletGravity = bulletGravity;
    this._bulletSizeFactor = bulletSizeFactor;
    this._bulletSpeed = bulletSpeed;
    this._bulletSpread = bulletSpread;
  }
}
