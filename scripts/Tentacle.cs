using Godot;
using System;

public partial class Tentacle : Node2D
{
    public float speedReductionFromEnemy = 0;
    public float pointsGiven = 0;

    float baseSpeed = -5;
    Vector2 destination;

    Main main;


    public override void _Ready()
    {
        main = GetNode<Main>("/root/main");
    }

    public override void _Process(double delta)
    {
        if (GlobalPosition.Y <= destination.Y)
        {
            baseSpeed = Mathf.Abs(baseSpeed);
        }

        if (GlobalPosition.Y > 560)
        {
            QueueFree();
            main.tentacleCount++;
            main.pointsAccumulated += pointsGiven;
        }

        GlobalPosition += new Vector2(0, baseSpeed - speedReductionFromEnemy);
    }

    public void MoveTentacle(Vector2 newDestination)
    {
        if (newDestination.Y < 40)
        {
            destination.Y = 40;
        }
        else if (newDestination.Y > 500)
        {
            destination.Y = 500;
        }
        else
        {
            destination = newDestination;
        }
    }
}