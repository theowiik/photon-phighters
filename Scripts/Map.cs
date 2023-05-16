using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;

public partial class Map : Node2D
{
    [GetNode("DarkSpawn")]
    public Node2D DarkSpawn { get; set; }

    [GetNode("LightSpawn")]
    public Node2D LightSpawn { get; set; }

    [GetNode("OB")]
    public Area2D OutOfBounds { get; set; }

    public override void _Ready()
    {
        this.AutoWire();
    }
}
