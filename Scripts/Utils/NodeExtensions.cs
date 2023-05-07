namespace PhotonPhighters.Scripts.Utils;
using System.Collections.Generic;
using Godot;

public static class NodeExtensions
{
    public static IEnumerable<T> GetNodes<T>(this Node node) where T : Node
    {
        var output = new List<T>();

        foreach (var child in node.GetChildren())
        {
            if (child is T c)
            {
                output.Add(c);
            }
        }

        return output;
    }
}
