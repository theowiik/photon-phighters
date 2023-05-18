using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace PhotonPhighters.Scripts;

public partial class MapManager : Node2D
{
  private const string MapsFolder = "res://Scenes/Maps";

  public delegate void OutOfBoundsEvent(Player player);

  public Node2D DarkSpawn => CurrentMap.DarkSpawn;
  public Node2D LightSpawn => CurrentMap.LightSpawn;
  public OutOfBoundsEvent OutOfBoundsEventListeners { get; set; }
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

  private IList<string> GetAllFilesInDirectory(string directory, string extension)
  {
    var dir = DirAccess.Open(directory);
    if (dir == null)
    {
      HandleError("Could not open directory: " + directory);
    }

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

  private Map GetRandomMap()
  {
    var maps = GetAllFilesInDirectory(MapsFolder, "tscn");
    var mapName = maps[GD.RandRange(0, maps.Count - 1)];
    var mapScene = GD.Load<PackedScene>(mapName);
    return mapScene.Instantiate<Map>();
  }

  private void HandleError(string message)
  {
    Console.WriteLine(message);
    GD.PrintErr(message);
    GetTree().Quit();
  }
}
