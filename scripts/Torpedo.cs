using Godot;
using System;

public partial class Torpedo : CharacterBody2D
{
    float speed;
    Vector2 direction;
    Vector2 velocity;
    [Export] int torpedoDamage;

    [Export] Area2D hitArea;
    [Export] PackedScene torpedoParticlesScene;

    Main main;
    Kraken kraken;


    public override void _Ready()
    {
        main = GetNode<Main>("/root/main");
        kraken = GetNode<Kraken>("/root/main/kraken");

        hitArea.AreaEntered += OnHitAreaAreaEntered;
        hitArea.BodyEntered += OnHitAreaBodyEntered;

        speed = (float)GD.RandRange(50, 100);
    }

    public override void _Process(double delta)
    {
        direction = (kraken.GlobalPosition - GlobalPosition).Normalized();
        velocity.X = Mathf.Clamp(Mathf.Lerp(velocity.X, direction.X * speed, 0.1f), -20, 20);
        velocity.Y = speed * 0.75f;
        Rotation = velocity.Angle();

        Velocity = velocity;
        MoveAndSlide();
    }

    private void OnHitAreaAreaEntered(Area2D _)
    {
        GpuParticles2D newTorpedoParticles = (GpuParticles2D)torpedoParticlesScene.Instantiate();
        GetParent().AddChild(newTorpedoParticles);
        newTorpedoParticles.GlobalPosition = GlobalPosition;
        newTorpedoParticles.Emitting = true;
        newTorpedoParticles.GetNode<GpuParticles2D>("bomb-sparkles").Emitting = true;
        newTorpedoParticles.GetNode<GpuParticles2D>("bomb-bubbles").Emitting = true;

        if (!main.isInvulnerable)
        {
            main.isInvulnerable = true;
            main.krakenHP -= torpedoDamage;
            main.invulnerabilityTimer.Start();
        }
        QueueFree();
    }

    private void OnHitAreaBodyEntered(Node2D _)
    {
        GpuParticles2D newTorpedoParticles = (GpuParticles2D)torpedoParticlesScene.Instantiate();
        GetParent().AddChild(newTorpedoParticles);
        newTorpedoParticles.GlobalPosition = GlobalPosition;
        newTorpedoParticles.Emitting = true;
        newTorpedoParticles.GetNode<GpuParticles2D>("bomb-sparkles").Emitting = true;
        newTorpedoParticles.GetNode<GpuParticles2D>("bomb-bubbles").Emitting = true;

        QueueFree();
    }
}
