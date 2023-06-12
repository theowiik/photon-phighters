using System;
using Godot;

namespace PhotonPhighters.Scripts.Events;

public partial class PlayerHurtEvent : Node
{
  public int Damage { get; set; }

  public PlayerHurtEvent(int damage)
  {
    Damage = damage;
  }
}
