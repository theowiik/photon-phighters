using Godot;
using System;

public partial class ScoreCounter : Node
{
    public struct Results {
        public int On;
        public int Off;
        public int Neutral;
    }

    public Results GetResults() {
        var lights = GetTree().GetNodesInGroup("Light");
        var results = new Results();

        foreach (var light in lights) {
            var lightNode = light as Light;

            if (lightNode == null) {
                throw new Exception("Light node is not a Light!!");
            }

            switch (lightNode.LightState) {
                case Light.LightMode.On:
                    results.On++;
                    break;
                case Light.LightMode.Off:
                    results.Off++;
                    break;
                case Light.LightMode.Neutral:
                    results.Neutral++;
                    break;
            }
        }

        return results;
    }
}
