using Godot;
using System;

public partial class RespawnPoint : Marker2D
{

	[Export]
	public int PositionX = 0;

	[Export]
	public int PositionY = 0;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GlobalPosition = new Vector2(PositionX, PositionY);
	}

	public void updateCheckPoint(float PositionX, float PositionY){
		this.GlobalPosition = new Vector2(PositionX,PositionY);

	}




}
