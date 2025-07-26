using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -400.0f;

	private AnimatedSprite2D spriteAnimation;

    public override void _Ready()
	{
        spriteAnimation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump.
		if (Input.IsActionJustPressed("up") && IsOnFloor())
		{
            spriteAnimation.Play("jump");

            velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle the movement/deceleration.
		Vector2 direction = Input.GetVector("left", "right", "up", "down");
		if (direction != Vector2.Zero)
		{
			spriteAnimation.FlipH = direction.X < 0; // Flip the sprite based on direction
            if (!spriteAnimation.IsPlaying())//don't play run animation if jump is playing
				spriteAnimation.Play("run");

            velocity.X = direction.X * Speed;
		}
		else
		{
            velocity.X = Mathf.MoveToward(Velocity.X, 0, (Speed * 0.01f));
            spriteAnimation.Play("idle");
        }

        Velocity = velocity;
		MoveAndSlide();
	}
}
