using Godot;
using System;

public partial class Mob : RigidBody2D
{
	private AnimatedSprite2D _animatedSprite2D;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SetUpAnimatedSprites();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// We also specified this function name in PascalCase in the editor's connection window.
	private void OnVisibleOnScreenNotifier2DScreenExited()
	{
			QueueFree();
	}

	private void SetUpAnimatedSprites()
	{
		_animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	string[] mobTypes = _animatedSprite2D.SpriteFrames.GetAnimationNames();
	_animatedSprite2D.Play(mobTypes[GD.Randi() % mobTypes.Length]);
	}
}
