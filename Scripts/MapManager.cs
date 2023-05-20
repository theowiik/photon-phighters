using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;

public partial class MapManager : Node2D
{
  public delegate void OutOfBoundsEvent(Player player);

  private const string MapsFolder = "res://Scenes/Maps";

  /// <summary>
  ///   A queue of maps to play. When the queue is empty, all maps in the MapsFolder will be added to the queue.
  /// </summary>
  private Queue<string> _mapsQueue = new();

  public Node2D DarkSpawn => CurrentMap.DarkSpawn;
  public Node2D LightSpawn => CurrentMap.LightSpawn;
  public OutOfBoundsEvent OutOfBoundsEventListeners { get; set; }
  private Map CurrentMap => GetChildOrNull<Map>(0);

  /// <summary>
  ///   Spawns the next map but does not enable map specific logic. See <see cref="StartNextMap" />.
  /// </summary>
  public void InitNextMap()
  {
    var cm = CurrentMap;
    if (cm != null)
    {
      RemoveChild(cm);
      cm.QueueFree();
    }

    // Start new map
    var map = GetNextMap();
    AddChild(map);
    map.CollisionsEnabled = false;
    map.OutOfBounds.BodyEntered += body =>
    {
      if (body is Player player)
      {
        OutOfBoundsEventListeners?.Invoke(player);
      }
    };
  }

  /// <summary>
  ///   Enables map specific logic.
  /// </summary>
  public void StartNextMap()
  {
    CurrentMap.CollisionsEnabled = true;
  }

  private IEnumerable<string> GetAllFilesInDirectory(string directory, string extension)
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

  private Map GetNextMap()
  {
    if (_mapsQueue.Count == 0)
    {
      var maps = GetAllFilesInDirectory(MapsFolder, "tscn");
      _mapsQueue = new Queue<string>(maps.Shuffled());
    }

    var mapName = _mapsQueue.Dequeue();
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
