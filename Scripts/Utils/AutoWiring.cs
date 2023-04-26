using System;
using System.Collections.Generic;
using System.Reflection;
using Godot;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class GetNodeAttribute : Attribute
{
    private readonly string _path;

    // Whether to exit the application if the node cannot be found
    private const bool FailHard = true;

    public GetNodeAttribute(string nodePath)
    {
        _path = nodePath;
    }

    public void SetNode(FieldInfo fieldInfo, Node node)
    {
        var childNode = node.GetNodeOrNull(_path);

        if (childNode == null)
        {
            var err = $"Cannot find Node for NodePath '{_path}'";
            GD.PrintErr(err);

            if (FailHard) node.GetTree().Quit();
            throw new Exception($"Cannot find Node for NodePath '{_path}'");
        }

        if (childNode.GetType() != fieldInfo.FieldType && !childNode.GetType().IsSubclassOf(fieldInfo.FieldType))
        {
            var err = $"Node is not a valid type. Expected {fieldInfo.FieldType} got {childNode.GetType()}";
            GD.PrintErr(err);

            if (FailHard) node.GetTree().Quit();
            throw new Exception(err);
        }

        fieldInfo.SetValue(node, childNode);
    }
}

public static class NodeAutoWire
{
    public static void AutoWire(this Node node)
    {
        WireFields(node);
    }

    private static void WireFields(Node node)
    {
        foreach (var field in GetFields(node))
        {
            field.GetCustomAttribute<GetNodeAttribute>()?.SetNode(field, node);
        }
    }

    private static IEnumerable<FieldInfo> GetFields(Node node)
    {
        if (node == null) return new List<FieldInfo>();

        var fields = node.GetType().GetFields(
            BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
        );

        return new List<FieldInfo>(fields);
    }
}
