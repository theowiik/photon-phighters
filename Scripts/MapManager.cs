using System.Linq;
using Godot;
using System.Collections.Generic;
using System.IO;

namespace PhotonPhighters.Scripts;

public partial class MapManager : Node2D
{
  private const string MapsFolder = "res://Scenes/Maps";
  public delegate void OutOfBoundsEvent(Player player);
  public OutOfBoundsEvent OutOfBoundsEventListeners;

  public void StartRandomMap()
  {
    // Remove all children
    foreach (var child in GetChildren())
    {
      RemoveChild(child);
      child.QueueFree();
    }

    // Start new map
    AddChild(GetRandomMap());
  }

  private static Map GetRandomMap()
  {
    var maps = GetAllFilesInDirectory(MapsFolder, "tscn");
    var mapName = maps[GD.RandRange(0, maps.Count)];
    var mapScene = GD.Load<PackedScene>(mapName);
    return mapScene.Instantiate<Map>();
  }

  private static IList<string> GetAllFilesInDirectory(string directory, string extension)
  {
    return Directory.GetFiles(directory).Where(file => file.EndsWith(extension)).ToList();
  }
}
