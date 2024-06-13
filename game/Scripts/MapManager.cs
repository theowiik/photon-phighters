using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Godot;
using GodotSharper.AutoGetNode;
using GodotSharper.Instancing;
using PhotonPhighters.Scripts.GameModes;
using PhotonPhighters.Scripts.Utils;
using PhotonPhighters.Scripts.Utils.ResourceWrapper;

namespace PhotonPhighters.Scripts;

public partial class MapManager : Node2D
{
  public delegate void OutOfBoundsEvent(Player player);

  private readonly string _mapsFolder = SceneResourceWrapper.MapsFolder;
  private IEnumerable<string> _maps = new List<string>();

  /// <summary>
  ///   A queue of maps to play. When the queue is empty, all maps in the MapsFolder will be added to the queue.
  /// </summary>
  private Queue<string> _mapsQueue = new();

  public IGameMode GameMode { get; set; }

  public OutOfBoundsEvent OutOfBoundsEventListeners { get; set; }
  private Map CurrentMap => GetChildOrNull<Map>(0);

  public override void _Ready()
  {
    this.GetNodes();
    _maps = GetAllFilesInDirectory(_mapsFolder, ".tscn").Shuffled();
  }

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

    if (GameMode.SpawnLights)
    {
      PlaceLights();
    }
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
      _mapsQueue = new Queue<string>(_maps);
    }

    var mapName = _mapsQueue.Dequeue();
    var mapScene = GD.Load<PackedScene>(mapName);
    return mapScene.Instantiate<Map>();
  }

  public Node2D GetRandomSpawnPoint()
  {
    return CurrentMap.GetRandomSpawnPoint();
  }

  private async Task PlaceLights()
  {
    CurrentMap.LightPlacingAutomata.PossibleLightPositionFound += globalPos =>
    {
      foreach (var existingLight in GetTree().GetNodesInGroup("lights").Cast<Light>())
      {
        const int RadiusToNotPlace = 20;
        var distance = globalPos.DistanceTo(existingLight.GlobalPosition);
        if (distance < RadiusToNotPlace)
        {
          return;
        }
      }

      var light = Instanter.Instantiate<Light>();
      CurrentMap.AddChild(light);
      light.GlobalPosition = globalPos;
    };

    foreach (var p in CurrentMap.GetCellsToCheckLights())
    {
      await ToSignal(GetTree(), "physics_frame");
      CurrentMap.LightPlacingAutomata.GlobalPosition = p;
    }

    CurrentMap.LightPlacingAutomata.Enabled = false;
  }
}
