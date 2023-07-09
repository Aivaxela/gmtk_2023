using Godot;
using System;

public partial class Javelin : CharacterBody2D
{
    float speed;
    Vector2 direction;
    Vector2 velocity;
    [Export] int javelinDamage;

    [Export] Area2D hitArea;
    [Export] PackedScene javelinParticlesScene;

    Main main;
    Kraken kraken;


    public override void _Ready()
    {
        main = GetNode<Main>("/root/main");
        kraken = GetNode<Kraken>("/root/main/kraken");

        hitArea.AreaEntered += OnHitAreaAreaEntered;
        hitArea.BodyEntered += OnHitAreaBodyEntered;

        speed = (float)GD.RandRange(200, 200);
        direction = (kraken.GlobalPosition - GlobalPosition).Normalized();
    }

    public override void _Process(double delta)
    {
        velocity = direction * speed;
        Rotation = velocity.Angle();

        Velocity = velocity;
        MoveAndSlide();
    }

    private void OnHitAreaAreaEntered(Area2D _)
    {
        GpuParticles2D newTorpedoParticles = (GpuParticles2D)javelinParticlesScene.Instantiate();
        GetParent().AddChild(newTorpedoParticles);
        newTorpedoParticles.GlobalPosition = GlobalPosition;
        newTorpedoParticles.Emitting = true;
        newTorpedoParticles.GetNode<GpuParticles2D>("bomb-sparkles").Emitting = true;
        newTorpedoParticles.GetNode<GpuParticles2D>("bomb-bubbles").Emitting = true;

        if (!main.isInvulnerable)
        {
            main.isInvulnerable = true;
            main.krakenHP -= javelinDamage;
            main.invulnerabilityTimer.Start();
        }
        QueueFree();
    }

    private void OnHitAreaBodyEntered(Node2D _)
    {
        GpuParticles2D newTorpedoParticles = (GpuParticles2D)javelinParticlesScene.Instantiate();
        GetParent().AddChild(newTorpedoParticles);
        newTorpedoParticles.GlobalPosition = GlobalPosition;
        newTorpedoParticles.Emitting = true;
        newTorpedoParticles.GetNode<GpuParticles2D>("bomb-sparkles").Emitting = true;
        newTorpedoParticles.GetNode<GpuParticles2D>("bomb-bubbles").Emitting = true;

        QueueFree();
    }
}
