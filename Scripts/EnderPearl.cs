using Godot;
using System;

public partial class EnderPearl : Node2D
{

	public float Speed = 1f;

	private Vector2 lastPosition = new Vector2();
	
	[Signal]
	public delegate void onEnderPearlCollisionEventHandler(Vector2 position);


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += new Vector2(Speed * (float) delta , 0.0f);
		GlobalPosition = Position;
	}


	public void _on_area_2d_body_entered(Node2D body){
		if(body is Front){
			lastPosition = Position;
			GD.Print("choque");
			QueueFree();
		}
	}
}
