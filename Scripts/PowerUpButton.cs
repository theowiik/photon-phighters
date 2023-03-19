using Godot;

public partial class PowerUpButton : Button
{
    private PowerUpManager.IPowerUpApplier _powerUpApplier;
    private static readonly Texture2D air_walker_icon = ResourceLoader.Load("res://Assets/Power Ups/air-walker.svg") as Texture2D;
    private static readonly Texture2D bunny_booster_icon = ResourceLoader.Load("res://Assets/Power Ups/bunny-booster.svg") as Texture2D;
    private static readonly Texture2D friction_free_icon = ResourceLoader.Load("res://Assets/Power Ups/friction-free.svg") as Texture2D;
    private static readonly Texture2D glass_cannon_icon = ResourceLoader.Load("res://Assets/Power Ups/glass-cannon.svg") as Texture2D;
    private static readonly Texture2D grav_blaster_icon = ResourceLoader.Load("res://Assets/Power Ups/grav-blaster.svg") as Texture2D;
    private static readonly Texture2D photon_accelerator_icon = ResourceLoader.Load("res://Assets/Power Ups/photon-accelerator.svg") as Texture2D;
    private static readonly Texture2D photon_booster_icon = ResourceLoader.Load("res://Assets/Power Ups/photon-booster.svg") as Texture2D;
    private static readonly Texture2D photon_enlarger_icon = ResourceLoader.Load("res://Assets/Power Ups/photon-enlarger.svg") as Texture2D;
    private static readonly Texture2D photon_gobbler_icon = ResourceLoader.Load("res://Assets/Power Ups/photon-gobbler.svg") as Texture2D;
    private static readonly Texture2D photon_multiplier_icon = ResourceLoader.Load("res://Assets/Power Ups/photon-multiplier.svg") as Texture2D;

    public override void _Ready()
    {
        _powerUpApplier = PowerUpManager.GetRandomPowerup();
        Texture2D icon;
        switch (_powerUpApplier.Name)
        {
            case "Air Walker":
                icon = air_walker_icon;
                break;
            case "Bunny Boost":
                icon = bunny_booster_icon;
                break;
            case "Frictionless movement":
                icon = friction_free_icon;
                break;
            case "Glass Cannon":
                icon = glass_cannon_icon;
                break;
            case "Gravitational Neuron Blaster":
                icon = grav_blaster_icon;
                break;
            case "Photon Accelerator":
                icon = photon_accelerator_icon;
                break;
            case "Photon Booster":
                icon = photon_booster_icon;
                break;
            case "Photon Enlarger":
                icon = photon_enlarger_icon;
                break;
            case "Photon Gobbler":
                icon = photon_gobbler_icon;
                break;
            case "Photon Multiplier":
                icon = photon_multiplier_icon;
                break;
            default:
                icon = bunny_booster_icon;
                break;
        }
        this.Icon = icon;
    }

    public void ApplyPowerUp(Player player)
    {
        _powerUpApplier.Apply(player);
    }
}
