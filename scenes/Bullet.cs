using Godot;

public partial class Bullet : Area2D
{
	[Export]
	public float Speed = 1.0f;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()

	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += new Vector2(Speed * (float) delta , 0.0f);
	}
}
