using System.Collections.Generic;
using System.IO;
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
    map.SetCollisionsEnabled(false);
    map.OutOfBounds.BodyEntered += body =>
    {
      if (body is Player player)
      {
        OutOfBoundsEventListeners?.Invoke(player);
      }
    };

    PlaceLights();
  }

  /// <summary>
  ///   Enables map specific logic.
  /// </summary>
  public void StartNextMap()
  {
    CurrentMap.SetCollisionsEnabled(true);
  }

  private IEnumerable<string> GetAllFilesInDirectory(string directory, string extension)
  {
    var dir = DirAccess.Open(directory);
    if (dir == null)
    {
      var msg = "Could not open directory: " + directory;
      GD.PrintErr(msg);
      GetTree().Quit();
      throw new FileNotFoundException(msg);
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

  public Node2D GetRandomSpawnPoint()
  {
    return CurrentMap.GetRandomSpawnPoint();
  }

  private async void PlaceLights()
  {
    CurrentMap.LightPlacingAutomata.PossibleLightPositionFound += (globalPos) =>
    {
      GD.Print("Light placed at " + globalPos);

      foreach (var existingLight in GetTree().GetNodesInGroup("lights").Cast<Light>())
      {
        const int RadiusToNotPlace = 5;
        var distance = globalPos.DistanceTo(existingLight.GlobalPosition);
        if (distance < RadiusToNotPlace)
        {
          GD.Print("Light too close to existing light");
          return;
        }
      }

      var lightScene = GD.Load<PackedScene>("res://Objects/Light.tscn");
      var light = lightScene.Instantiate<Light>();
      AddChild(light);
      light.GlobalPosition = globalPos;
    };

    var positions = CurrentMap.GetCellsToCheckLights();
    foreach (var p in positions)
    {
      await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
      CurrentMap.LightPlacingAutomata.GlobalPosition = p;
    }
  }
}
