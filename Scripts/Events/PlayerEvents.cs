using System;
using Godot;

namespace PhotonPhighters.Scripts.Events;

public partial class PlayerHurtEvent : Node
{
  public int _damage;

  public PlayerHurtEvent(int damage)
  {
    this._damage = damage;
  }
}
