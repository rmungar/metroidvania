using Godot;
using System;

public partial class PauseMenu : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	private void _on_resume_pressed(){
		this.Hide();
	}
	private void _on_inventory_pressed(){
		
	}
	private void _on_quit_pressed(){
		GetTree().Quit();
	}

	private void _on_controls_pressed(){

	}
}