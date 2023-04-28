using System;
using System.Collections.Generic;
using System.Linq;
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

    public void SetNode(MemberInfo memberInfo, Node node)
    {
        var childNode = node.GetNodeOrNull(_path);

        if (childNode == null)
        {
            HandleError($"Cannot find Node for NodePath '{_path}'", node);
        }

        Type expectedType = memberInfo is FieldInfo fieldInfo ? fieldInfo.FieldType : ((PropertyInfo)memberInfo).PropertyType;

        if (childNode.GetType() != expectedType && !childNode.GetType().IsSubclassOf(expectedType))
        {
            HandleError($"Node is not a valid type. Expected {expectedType} got {childNode.GetType()}", node);
        }

        if (memberInfo is FieldInfo)
            ((FieldInfo)memberInfo).SetValue(node, childNode);
        else
            ((PropertyInfo)memberInfo).SetValue(node, childNode);
    }

    private void HandleError(string err, Node node)
    {
        GD.PrintErr(err);
        if (FailHard) node.GetTree().Quit();
        throw new Exception(err);
    }
}

public static class NodeAutoWire
{
    public static void AutoWire(this Node node)
    {
        WireMembers(node, GetFields(node));
        WireMembers(node, GetProperties(node));
    }

    private static void WireMembers<T>(Node node, IEnumerable<T> members) where T : MemberInfo
    {
        foreach (var member in members)
        {
            member.GetCustomAttribute<GetNodeAttribute>()?.SetNode(member, node);
        }
    }

    private static IEnumerable<FieldInfo> GetFields(Node node)
    {
        return GetMembers<FieldInfo>(node);
    }

    private static IEnumerable<PropertyInfo> GetProperties(Node node)
    {
        return GetMembers<PropertyInfo>(node);
    }

    private static IEnumerable<T> GetMembers<T>(Node node) where T : MemberInfo
    {
        if (node == null) return new List<T>();

        var members = node.GetType().GetMembers(
            BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance
        ).OfType<T>();

        return new List<T>(members);
    }
}
