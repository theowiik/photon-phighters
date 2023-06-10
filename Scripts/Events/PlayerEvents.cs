using System;
using Godot;

namespace PhotonPhighters.Scripts;

public partial class PlayerEvents
{
  public partial class PlayerHurtEvent : Node
  {
    public int Damage;

    public PlayerHurtEvent(int Damage)
    {
      this.Damage = Damage;
    }
  }
}
