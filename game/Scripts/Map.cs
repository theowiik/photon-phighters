using System.Collections.Generic;
using System.Linq;
using Godot;
using GodotSharper.AutoGetNode;
using PhotonPhighters.Scripts.GSAlpha;
using PhotonPhighters.Scripts.Utils;

namespace PhotonPhighters.Scripts;

public partial class Map : Node2D
{
  private Node2D _lastSpawnPoint;

  [GetNode("SpawnPointsContainer")]
  private Node2D _spawnPointsContainer;

  [GetNode("TileMap")]
  private TileMap _tileMap;

  [GetNode("OB")]
  public Area2D OutOfBounds { get; private set; }

  [GetNode("LightPlacingAutomata")]
  public LightPlacingAutomata LightPlacingAutomata { get; private set; }

  public override void _Ready()
  {
    this.GetNodes();
  }

  public void SetCollisionsEnabled(bool value)
  {
    OutOfBounds.Monitoring = value;
  }

  /// <summary>
  ///   Pseudo-randomly selects a spawn point from the SpawnPointsContainer.
  ///   The same spawn point will not be returned twice in a row.
  /// </summary>
  /// <returns>
  ///   A spawn point from the SpawnPointsContainer.
  /// </returns>
  public Node2D GetRandomSpawnPoint()
  {
    Node2D nextSpawn = null;
    var spawnPoints = _spawnPointsContainer.GetNodesOfType<Node2D>().ToList();

    while (nextSpawn == null || nextSpawn == _lastSpawnPoint)
    {
      nextSpawn = spawnPoints.Sample();
    }

    _lastSpawnPoint = nextSpawn;
    return nextSpawn;
  }

  /// <summary>
  ///   Possible cells to check for light placements.
  /// </summary>
  /// <returns>
  ///   A list of possible cells to check for light placements.
  /// </returns>
  public IEnumerable<Vector2> GetCellsToCheckLights()
  {
    var positions = new List<Vector2>();
    var offsets = new List<Vector2> { new(0, -1), new(0, 1), new(-1, 0), new(1, 0) };

    foreach (var cellCoordinate in _tileMap.GetUsedCells(0))
    {
      foreach (var (x, y) in offsets)
      {
        var c = cellCoordinate + new Vector2I((int)x, (int)y);
        positions.Add(ToGlobal(_tileMap.MapToLocal(c)));
      }
    }

    return positions.Distinct().ToList().Shuffled();
  }
}
