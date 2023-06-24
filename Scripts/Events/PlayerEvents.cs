using Godot;

namespace PhotonPhighters.Scripts.Events;

public partial class PlayerHurtEvent : Node
{
  public PlayerHurtEvent(int damage)
  {
    Damage = damage;
  }

  public int Damage { get; set; }
}
