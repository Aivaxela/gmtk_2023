using Godot;
using System;

public partial class Kraken : CharacterBody2D
{
    [Export] public float speed;
    [Export] StaticBody2D tentacleShield;
    [Export] Timer tentacleShieldDurationTimer;
    [Export] Timer tentacleShieldCooldownTimer;

    Vector2 velocty;

    Main main;


    public override void _Ready()
    {
        main = GetNode<Main>("/root/main");

        tentacleShield.Visible = false;
        tentacleShield.CollisionLayer = 0;
    }

    public override void _Process(double delta)
    {
        if (!main.levelEnded)
        {
            if (Input.IsActionPressed("move-left"))
            {
                velocty.X = -speed;
            }
            else if (Input.IsActionPressed("move-right"))
            {
                velocty.X = speed;
            }
            else
            {
                velocty.X = 0;
            }

            if (Input.IsActionJustPressed("tentacle-shield") && tentacleShieldCooldownTimer.TimeLeft <= 0 && main.tentacleCount > 0)
            {
                tentacleShieldDurationTimer.Start();
                tentacleShield.Visible = true;
                tentacleShield.CollisionLayer = 2;
                main.tentacleCount--;
                main.shieldReadyLabel.Visible = false;
                tentacleShieldCooldownTimer.Start();
            }

            if (tentacleShieldDurationTimer.TimeLeft <= 0 && tentacleShield.Visible)
            {
                tentacleShield.Visible = false;
                tentacleShield.CollisionLayer = 0;
                main.tentacleCount++;
            }

            if (tentacleShieldCooldownTimer.TimeLeft <= 0)
            {
                main.shieldReadyLabel.Visible = true;
            }
        }
        else if (main.levelEnded)
        {
            velocty.X = 0;
        }

        Velocity = velocty;
        MoveAndSlide();
    }
}
