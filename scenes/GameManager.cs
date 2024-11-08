using Godot;


public partial class GameManager : Node2D
{

	// RESPAWN POINT

		public Marker2D RespawnPoint;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void RespawnPlayer(){

		PlayerController pc = GetNode<PlayerController>("Player");

		pc.GlobalPosition = RespawnPoint.GlobalPosition;
		pc.RespawnPlayer();
	}	
}
