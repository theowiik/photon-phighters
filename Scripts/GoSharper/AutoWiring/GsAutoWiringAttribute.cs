using System;
using System.Reflection;
using Godot;
using PhotonPhighters.Scripts.Exceptions;

namespace PhotonPhighters.Scripts.GoSharper.AutoWiring;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class GsAutoWiringAttribute : Attribute
{
  private readonly string _path;

  public GsAutoWiringAttribute(string nodePath)
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
