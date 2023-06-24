using System;

namespace PhotonPhighters.Scripts.Exceptions;

public class NodeNotFoundException : Exception
{
  public NodeNotFoundException(string message)
    : base(message) { }
}
