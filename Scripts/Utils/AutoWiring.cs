using System;
using System.Collections.Generic;
using System.Reflection;
using Godot;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class GetNodeAttribute : Attribute
{
    private readonly string _path;

    public GetNodeAttribute(string nodePath)
    {
        _path = nodePath;
    }

    public void SetNode(FieldInfo fieldInfo, Node node)
    {
        var childNode = node.GetNodeOrNull(_path);

        if (childNode == null)
            throw new Exception($"Cannot find Node for NodePath '{_path}'");

        if (childNode.GetType() == fieldInfo.FieldType || childNode.GetType().IsSubclassOf(fieldInfo.FieldType))
            fieldInfo.SetValue(node, childNode);
        else
            throw new Exception($"Node is not a valid type. Expected {fieldInfo.FieldType} got {childNode.GetType()}");
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
            field.GetCustomAttribute<GetNodeAttribute>()?.SetNode(field, node);
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
