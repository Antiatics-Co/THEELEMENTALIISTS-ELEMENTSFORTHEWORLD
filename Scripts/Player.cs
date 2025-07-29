using Godot;
using System;

public partial class Player : CharacterBody2D
{
	public const float Speed = 200.0f;
	public const float JumpVelocity = -400.0f;

	private AnimatedSprite2D spriteAnimation;
	private CollisionShape2D hitBox;

    private bool isAttacking = false;
    private bool canIdle = true; // Flag to control idle state

    public override void _Ready()
	{
        spriteAnimation = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        hitBox = GetNode<CollisionShape2D>("%SlapHitBox");
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
		else if(IsOnFloor() && canIdle) // Only decelerate if on the floor and not jumping
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, (Speed * 0.15f));
            spriteAnimation.Play("idle");
        }

        Velocity = velocity;
		MoveAndSlide();
	}

    public override void _Process(double delta)
    {
        
		String hitBoxName = "%SlapHitBox";

        if (Input.IsActionJustPressed("slap"))
		{
			isAttacking = true;
            canIdle = false; // Prevent idle state during attack

            hitBoxName = "%SlapHitBox";
            GD.Print("Slap attack initiated");
            spriteAnimation.Play("slap");
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
            isAttacking = false; // Reset the attacking state after enabling hitbox
        }
		else if (spriteAnimation.Frame == 3 && !isAttacking && !hitBox.Disabled)
		{
            hitBox = GetNode<CollisionShape2D>(hitBoxName);
            hitBox.Disabled = true; // disable the hitbox after the attack
            canIdle = true; // Allow idle state after attack
        }
    }
}
