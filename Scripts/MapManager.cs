using System;
using Godot;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PhotonPhighters.Scripts;

public partial class MapManager : Node2D
{
  private const string MapsFolder = "res://Scenes/Maps";
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
    AddChild(map);
    map.OutOfBounds.BodyEntered += body =>
    {
      if (body is Player player)
      {
        OutOfBoundsEventListeners?.Invoke(player);
      }
    };
  }

  private Map GetRandomMap()
  {
    var maps = GetAllFilesInDirectory(MapsFolder, "tscn");
    var mapName = maps[GD.RandRange(0, maps.Count - 1)];
    GD.Print("Loading map: " + mapName + "...");
    var mapScene = GD.Load<PackedScene>(mapName);
    return mapScene.Instantiate<Map>();
  }

  private IList<string> GetAllFilesInDirectory(string directory, string extension)
  {
    var dir = DirAccess.Open(directory);
    if (dir == null)
      HandleError("Could not open directory: " + directory);

    var files = new List<string>();

    dir.ListDirBegin();
    var fileName = dir.GetNext();
    while (fileName != "")
    {
      if (!dir.CurrentIsDir())
      {
        files.Add($"{directory}/{fileName}");
      }

      fileName = dir.GetNext();
    }

    return files.Where(file => file.EndsWith(extension)).ToList();
  }

  private void HandleError(string message)
  {
    Console.WriteLine(message);
    GD.PrintErr(message);
    GetTree().Quit();
  }
}
