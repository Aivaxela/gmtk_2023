using Godot;
using System;

public partial class Tentacle : Node2D
{
    [Export] GpuParticles2D bubbleParticles;

    public float speedReductionFromEnemy = 0;
    public float pointsGiven = 0;

    float baseSpeed = -500;
    Vector2 destination;

    Main main;


    public override void _Ready()
    {
        main = GetNode<Main>("/root/main");
    }

    public override void _Process(double delta)
    {
        if (GlobalPosition.Y <= destination.Y || (GetChildCount() > 3))
        {
            baseSpeed = Mathf.Abs(baseSpeed);
            bubbleParticles.Emitting = true;
        }

        if (GlobalPosition.Y > 560)
        {
            QueueFree();
            main.tentacleCount++;
            main.killsAccumulated += pointsGiven;
            main.soulsAccumulated += pointsGiven;

            for (int i = 0; i < GetChildCount(); i++)
            {
                if (GetChild(i) is Pizza)
                {
                    main.krakenHP++;
                }
            }
        }

        GlobalPosition += new Vector2(0, baseSpeed - speedReductionFromEnemy) * (float)delta;
    }

    public void MoveTentacle(Vector2 newDestination)
    {
        destination = newDestination;
        destination.Y = 40;
    }
}
