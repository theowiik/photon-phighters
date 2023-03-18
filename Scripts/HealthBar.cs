using Godot;
using System;

public partial class HealthBar : Control
{
    private Label _healthLabel;

    public override void _Ready()
    {
        _healthLabel = GetNode<Label>("HealthLabel");
    }

    public void SetHealth(int health, int maxHealth)
    {
        _healthLabel.Text = $"{health}/{maxHealth}";
    }
}
