using Godot;
using System;

public partial class Player : Area2D
{
	#region Signals
	/// <summary>
	/// Some test description
	/// </summary>
	[Signal]
	public delegate void HitEventHandler();
	#endregion

	#region Public Properties
	[Export]
	public int Speed { get; set; } = 400; // How fast the player will move (pixels/sec).
	
	public Vector2 ScreenSize; // Size of the game window.
	#endregion
	#region Private Properties
	private Vector2 _velocity;
	private AnimatedSprite2D _animatedSprite2D;
	#endregion

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ScreenSize = GetViewportRect().Size;
		Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		SetVelocityAndSpriteBasedOnPlayerInput(delta);
		SetPositionBasedOnCurrentVelocity(delta);
		SetAnimationBasedOnCurrentVelocity();
	}

	public void Start(Vector2 position)
	{
			Position = position;
			Show();
			GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}

	#region Private Methods

	#region Signal Handlers
	// We also specified this function name in PascalCase in the editor's connection window.
	private void OnBodyEntered(Node2D body)
	{
			Hide(); // Player disappears after being hit.
			EmitSignal(SignalName.Hit);
			// Must be deferred as we can't change physics properties on a physics callback.
			GetNode<CollisionShape2D>("CollisionShape2D")
				.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
	}
	#endregion

	private void SetVelocityAndSpriteBasedOnPlayerInput(double delta) 
	{
		SetVelocityDirectionBasedOnPlayerInput();

		_animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		if (_velocity.Length() > 0)
		{
			_velocity = _velocity.Normalized() * Speed;
			_animatedSprite2D.Play();
		}
		else
		{
			_animatedSprite2D.Stop();
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

	private void SetAnimationBasedOnCurrentVelocity()
	{
		if (_velocity.X != 0)
		{
				_animatedSprite2D.Animation = "walk";
				_animatedSprite2D.FlipV = false;
				_animatedSprite2D.FlipH = _velocity.X < 0;
		}
		else if (_velocity.Y != 0)
		{
				_animatedSprite2D.Animation = "up";
				_animatedSprite2D.FlipV = _velocity.Y > 0;
		}
			}

	#endregion
}
