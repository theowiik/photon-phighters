﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;
using PhotonPhighters.Scripts.Exceptions;

namespace PhotonPhighters.Scripts.Utils;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class GetNodeAttribute : Attribute
{
  // Whether to exit the application if the node cannot be found
  private const bool FailHard = true;

  private readonly string _path;

  public GetNodeAttribute(string nodePath)
  {
    _path = nodePath;
  }

  public void SetNode(MemberInfo memberInfo, Node node)
  {
    var childNode = node.GetNodeOrNull(_path);

    if (childNode == null)
    {
      node.GetTree().Quit();
      throw new NodeNotFoundException($"Cannot find Node for NodePath '{_path}'");
    }

    var expectedType = memberInfo is FieldInfo fieldInfo
      ? fieldInfo.FieldType
      : ((PropertyInfo)memberInfo).PropertyType;

    if (childNode.GetType() != expectedType && !childNode.GetType().IsSubclassOf(expectedType))
    {
      node.GetTree().Quit();
      throw new ArgumentException($"Node is not a valid type. Expected {expectedType} got {childNode.GetType()}");
    }

    switch (memberInfo)
    {
      case FieldInfo fieldInformation:
        fieldInformation.SetValue(node, childNode);
        break;
      case PropertyInfo propertyInformation:
        propertyInformation.SetValue(node, childNode);
        break;
      default:
        throw new ArgumentException(
          $"MemberInfo is not a valid type. Expected {nameof(FieldInfo)} or {nameof(PropertyInfo)} got {memberInfo.GetType()}"
        );
    }
  }
}

public static class NodeAutoWire
{
  public static void AutoWire(this Node node)
  {
    WireMembers(node, GetFields(node));
    WireMembers(node, GetProperties(node));
  }

  private static IEnumerable<FieldInfo> GetFields(Node node)
  {
    return GetMembers<FieldInfo>(node);
  }

  private static IEnumerable<T> GetMembers<T>(Node node)
    where T : MemberInfo
  {
    if (node == null)
    {
      return new List<T>();
    }

    var members = node.GetType()
      .GetMembers(BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
      .OfType<T>();

    return new List<T>(members);
  }

  private static IEnumerable<PropertyInfo> GetProperties(Node node)
  {
    return GetMembers<PropertyInfo>(node);
  }

  private static void WireMembers<T>(Node node, IEnumerable<T> members)
    where T : MemberInfo
  {
    foreach (var member in members)
    {
      member.GetCustomAttribute<GetNodeAttribute>()?.SetNode(member, node);
    }
  }
}
