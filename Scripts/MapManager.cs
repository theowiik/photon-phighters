using System;
using System.Linq;
using Godot;
using System.Collections.Generic;
using System.IO;

namespace PhotonPhighters.Scripts;

public partial class MapManager : Node2D
{
  private const string MapsFolder = "Scenes/Maps";
  public delegate void OutOfBoundsEvent(Player player);
  public OutOfBoundsEvent OutOfBoundsEventListeners;
  public Node2D LightSpawn => CurrentMap.LightSpawn;
  public Node2D DarkSpawn => CurrentMap.DarkSpawn;
  private Map CurrentMap => GetChild<Map>(0);

  public void StartRandomMap()
  {
    // Remove all children
    foreach (var child in GetChildren())
    {
      RemoveChild(child);
      child.QueueFree();
    }

    // Start new map
    var map = GetRandomMap();
    map.OutOfBounds.BodyEntered += body =>
    {
      if (body is Player player)
      {
        OutOfBoundsEventListeners?.Invoke(player);
      }
    };

    AddChild(map);
  }

  private Map GetRandomMap()
  {
    var maps = GetAllFilesInDirectory(MapsFolder, "tscn");
    var mapName = maps[GD.RandRange(0, maps.Count)];
    GD.Print("Loading map: " + mapName + "...");
    var mapScene = GD.Load<PackedScene>(mapName);
    return mapScene.Instantiate<Map>();
  }

  private IList<string> GetAllFilesInDirectory(string directory, string extension)
  {
    try
    {
      var files = Directory.GetFiles(directory, $"*.{extension}");
      return files.Where(file => file.EndsWith(extension)).ToList();
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      GD.PrintErr("Could not find directory: " + directory);
      GetTree().Quit();
      throw;
    }
  }
}
