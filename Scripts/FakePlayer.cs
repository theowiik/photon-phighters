using Godot;

public partial class FakePlayer : Node2D, IDamageable
{
    private const int MaxHealth = 100;
    private int _health = MaxHealth;
    private HealthBar _healthBar;

    public override void _Ready()
    {
        _healthBar = GetNode<HealthBar>("HealthBar");
        SetHealthLabel();
        var detectionArea = GetNode<Area2D>("BulletDetectionArea");
        detectionArea.AreaEntered += OnBulletEntered;
    }

    private void OnBulletEntered(Area2D area)
    {
        if (area is Bullet bullet)
        {
            TakeDamage(bullet.Damage);
            bullet.QueueFree();
        }
    }

    public void TakeDamage(int damage)
    {
        GD.Print($"Player took {damage} damage!");
        _health -= damage;

        if (_health <= 0)
        {
            GD.Print("Player is dead!");
            QueueFree();
        }

        SetHealthLabel();
    }

    private void SetHealthLabel()
    {
        _healthBar.SetHealth(_health, MaxHealth);
    }
}

// Possibly remove
public interface IDamageable
{
    void TakeDamage(int damage);
}
