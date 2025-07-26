using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 200.0f;
	public const float JumpVelocity = -400.0f;

	private AnimatedSprite2D spriteAnimation;
	private CollisionShape2D hitBox;

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
        else if (!IsOnFloor() && velocity.Y > 0)
            spriteAnimation.Play("fall");

        // Get the input direction and handle the movement/deceleration.
        Vector2 direction = Input.GetVector("left", "right", "up", "down");
		if (direction != Vector2.Zero)
		{
			spriteAnimation.FlipH = direction.X < 0; // Flip the sprite based on direction
            if (IsOnFloor() && !Input.IsActionJustPressed("up"))//don't play run animation if jump is playing
				spriteAnimation.Play("run");

            velocity.X = direction.X * Speed;
		}
		else if(IsOnFloor()) // Only decelerate if on the floor and not jumping
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, (Speed * 0.15f));
            spriteAnimation.Play("idle");
        }

        Velocity = velocity;
		MoveAndSlide();
	}

    public override void _Process(double delta)
    {
        bool isAttacking = false;
		String hitBoxName = "AnimatedSprite2D/Area2D/SlapHitBox";

        if (Input.IsActionJustPressed("slap"))
		{
			isAttacking = true;
            hitBoxName = "%SlapHitBox";
            GD.Print("Slap attack initiated");
            // Play slap animation
            //change collsion shape to slap hitbox
            //if enemy is hit apply damage... perhaps add a cool down
        }
        else if(Input.IsActionJustPressed("kick"))
		{
			isAttacking = true;
            hitBoxName = "%KickHitBox";
            // Play kick animation
            //change collsion shape to kick hitbox
            //if enemy is hit apply damage... perhaps add a cool down
        }
        else if(Input.IsActionJustPressed("throw"))
		{
			isAttacking = true;
			//hitBoxName = "ThrowHitBox"; //throwing may have a different system as it is dealing with a projectile rather than a melee attack

                                        // Play throw animation
                                        //change collsion shape to throw hitbox
                                        //if enemy is hit apply damage... perhaps add a cool down
        }

        if (spriteAnimation.Frame == 1 && isAttacking)
        {
            GD.Print("Attacking with: " + hitBoxName);
            hitBox = GetNode<CollisionShape2D>(hitBoxName);
			hitBox.Disabled = false; // Enable the hitbox for the attack

        }
		else if (spriteAnimation.Frame == 0 || !isAttacking && !hitBox.Disabled)
		{
            hitBox = GetNode<CollisionShape2D>(hitBoxName);
            isAttacking = false;
            hitBox.Disabled = true; // disable the hitbox after the attack
        }
    }
}
