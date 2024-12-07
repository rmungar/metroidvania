using Godot;
using System;

public partial class RespawnPointsController : Node2D
{


	[Signal]
	public delegate void firstCheckPointEventHandler( float PositionX , float PositionY );

	[Signal]
	public delegate void secondCheckPointEventHandler( float PositionX, float PositionY );

	[Signal]
	public delegate void thirdCheckPointEventHandler( float PositionX, float PositionY );

	[Signal]
	public delegate void fourthCheckPointEventHandler( float PositionX, float PositionY );



	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}



	public void _on_first_checkPoint(Node2D body){
		



		if (body is CharacterBody2D){
			if (body is PlayerController){
				GD.Print("Actualizando RP");
				EmitSignal(nameof(firstCheckPoint), 479.5, 272.5);

			}
		}


	

	}
	public void _on_second_checkPoint(Node2D body){
		
		if (body is CharacterBody2D){
			if (body is PlayerController){
				GD.Print("Actualizando RP");
				EmitSignal(nameof(secondCheckPoint), 1548.0, 150.0);

			}
		}

		

	}
	public void _on_third_checkPoint(Node2D body){
		

		if (body is CharacterBody2D){
			if (body is PlayerController){
				GD.Print("Actualizando RP");
				EmitSignal(nameof(thirdCheckPoint), 2224.0, 1048.0);

			}
		}
		

	}
	public void _on_fourth_checkPoint(Node2D body){
		
		if (body is CharacterBody2D){
			if (body is PlayerController){
				GD.Print("Actualizando RP");
				EmitSignal(nameof(fourthCheckPoint), 2864.0, 359.5);

			}
		}

		

	}
}
