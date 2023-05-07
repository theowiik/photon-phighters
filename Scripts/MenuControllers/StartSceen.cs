using Godot;

namespace PhotonPhighters.Scripts.MenuControllers;
public partial class StartSceen : Node2D
{
    public override void _Ready()
    {
        const string buttonsRoot = "CanvasLayer/VBoxContainer/";
        var startButton = GetNode<Button>(buttonsRoot + "StartButton");
        var quitButton = GetNode<Button>(buttonsRoot + "QuitButton");

        startButton.Pressed += StartGame;
        quitButton.Pressed += QuitGame;
    }

    private void StartGame() => GetTree().ChangeSceneToFile("res://Scenes/Levels/BaseLevel.tscn");

    private void QuitGame() => GetTree().Quit();
}