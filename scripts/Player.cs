using Godot;
using System;

public partial class Player : Area2D
{
	#region Public Properties
	[Export]
	public int Speed { get; set; } = 400; // How fast the player will move (pixels/sec).
	
	public Vector2 ScreenSize; // Size of the game window.
	#endregion
	#region Private Properties
	private Vector2 _velocity;
	#endregion

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		SetVelocityAndSpriteBasedOnPlayerInput(delta);
		SetPositionBasedOnCurrentVelocity(delta);		
	}

	#region Private Methods

	private void SetVelocityAndSpriteBasedOnPlayerInput(double delta) 
	{
		SetVelocityDirectionBasedOnPlayerInput();

		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		if (_velocity.Length() > 0)
		{
			_velocity = _velocity.Normalized() * Speed;
			animatedSprite2D.Play();
		}
		else
		{
			animatedSprite2D.Stop();
		}
	}

	private void SetVelocityDirectionBasedOnPlayerInput()
	{
		_velocity = Vector2.Zero; // The player's movement vector.

		if (Input.IsActionPressed("move_right"))
		{
			_velocity.X += 1;
		}

		if (Input.IsActionPressed("move_left"))
		{
			_velocity.X -= 1;
		}

		if (Input.IsActionPressed("move_down"))
		{
			_velocity.Y += 1;
		}

		if (Input.IsActionPressed("move_up"))
		{
			_velocity.Y -= 1;
		}
	}

	private void SetPositionBasedOnCurrentVelocity(double delta)
	{
		Position += _velocity * (float)delta;
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		);
	}

	#endregion
}
