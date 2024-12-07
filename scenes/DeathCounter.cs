using Godot;
using System;

public partial class DeathCounter : Label
{

	public int deathCounter = 0;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public void _on_player_death(){
		deathCounter += 1;
		Text = "DEATHS: " + deathCounter;
	}


}
