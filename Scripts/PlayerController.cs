using Godot;
using System;

public partial class PlayerController : CharacterBody2D
{


	// PLAYER RELATED PARAMS

		// HEALTH

			[Export]
			private int Hp = 100;

			private bool isTakingDamage = false;

			private bool isDead = false;

			[Signal]
			public delegate void DeathEventHandler();

			public bool isPaused = false;

		// MOVEMENT SPEED & JUMP STRENGTH

			private int facingDirection = 0;


			[Export]
			private int Speed = 200;

			private int Gravity = 400;

			[Export]
			private int JumpHeight = 400;

			Vector2 velocity = new Vector2();

			private bool isInAir = false;

		// DASHING
		
			private int DashSpeed = 400;

			private bool isDashing = false;

			private double dashTimer = .2f;

			private double dashTimerReset = .2f;

			private bool canDash = true;



		// WALL JUMPING

			private bool isWallJumping = false;

			private double wallJumpTimer = .45f;

			private double wallJumpTimerReset = .45f;




		// PROJECTILES

			[Export]
			public Vector2 BulletOffset = new Vector2(0, -5);

			[Export]
			public float ButlletSpeed = 150;

			private PackedScene bullet;


	// OTHERS

		[Signal]
		public delegate void InventoryEventHandler();

		[Signal]
		public delegate void PauseEventHandler();



		
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	   	bullet = (PackedScene) ResourceLoader.Load("res://scenes/proyectil.tscn");

		

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{

		Paused();
		InventoryOpen();

		if(!isDead){
			

			if(!isDashing && !isWallJumping){
				ProcessMovement(delta);
			}

			if (IsOnFloor()){
				if (Input.IsActionPressed("jump")) {
					velocity.Y = -JumpHeight;
					GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("jump");
					isInAir = true;
				}
				else{
					isInAir = false;
				}
				canDash = true;
				
			}
			
			ProcessWallJump(delta);

			if (canDash){
				ProcessDash(delta);
			}

			if (isDashing) {

				dashTimer -= delta;
				if (dashTimer <= 0) {
					isDashing = false;
					velocity = new Vector2 (0, 0);
				}
			} else {
				
				velocity.Y += (float) (Gravity * delta);

			}
			
			
			ShootProjectiles();

			
			Velocity = velocity;
			MoveAndSlide();
		}
		
	}


	public void ProcessMovement(double delta){

		facingDirection = 0;
		if (!isTakingDamage){

			if (Input.IsActionPressed("right")) {
				facingDirection += 1;
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = false;
			}
			if (Input.IsActionPressed("left")) {
				facingDirection -= 1;
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = true;
			}

		}
		
		if (facingDirection != 0){
			velocity.X = facingDirection * Speed;
			
			if (!isInAir) {
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("walk");
			}
			else{
				if (!isTakingDamage){
					if(GetNode<RayCast2D>("RayCastRight").IsColliding() || GetNode<RayCast2D>("RayCastLeft").IsColliding()){
						if(GetNode<RayCast2D>("RayCastRight").IsColliding()){
							GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = false;
						}
						else{
							GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = true;
						}
						GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("wall");
					}
					else{
						if(Velocity.Y > 0){
							GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("fall");
						}
					}
					
				}
				
			}
			isTakingDamage = false;
			
		}
		else{
			velocity.X = Mathf.MoveToward(velocity.X, 0, Speed);
			
			if (velocity.X < 5 && velocity.X > -5) {
				
				if (!isInAir) {
					GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("idle");
				}
				else{
					if (!isTakingDamage){
						if(Velocity.Y > 0){
							GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("fall");
						}
					}
				}
			}
			isTakingDamage = false;
		}
	}


	private void ProcessWallJump(double delta) {

		

		if(Input.IsActionJustPressed("jump") && GetNode<RayCast2D>("RayCastRight").IsColliding()) {
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = true;
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("jump");
			velocity.Y = -JumpHeight;
			velocity.X = -JumpHeight;
			isWallJumping = true;

		}else if(Input.IsActionJustPressed("jump") && GetNode<RayCast2D>("RayCastLeft").IsColliding()) {
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH = false;
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("jump");	
			velocity.Y = -JumpHeight;
			velocity.X = JumpHeight;
			isWallJumping = true;
		}

		if (isWallJumping){

			wallJumpTimer -= delta;
			if (wallJumpTimer <= 0){
				isWallJumping = false;
				wallJumpTimer = wallJumpTimerReset;
				canDash = true;
			}

		}
	}


	private void ProcessDash (double delta){
		
		if (Input.IsActionJustPressed("dash") ) {
			
			

			if (Input.IsActionPressed("right")) {
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("dash");
				velocity.X = DashSpeed;
				isDashing = true;
			}
			if (Input.IsActionPressed("left")) {
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("dash");
				velocity.X = -DashSpeed;
				isDashing = true;
			}
			if (Input.IsActionPressed("up")) {
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("dash");
				velocity.Y = -DashSpeed;
				isDashing = true;
			}
			if (Input.IsActionPressed("down") && !IsOnFloor()) {
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("dash");
				velocity.Y = DashSpeed;
				isDashing = true;
			}
			if (Input.IsActionPressed("right") && Input.IsActionPressed("up")) {
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("dash");
				velocity.X = DashSpeed;
				velocity.Y = -DashSpeed;
				isDashing = true;
			}
			if (Input.IsActionPressed("left") && Input.IsActionPressed("up")) {
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("dash");
				velocity.X = -DashSpeed;
				velocity.Y = -DashSpeed;
				isDashing = true;
			}
			if (Input.IsActionPressed("right") && Input.IsActionPressed("down")) {
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("dash");
				velocity.X = DashSpeed;
				velocity.Y = DashSpeed;
				isDashing = true;
			}
			if (Input.IsActionPressed("left") && Input.IsActionPressed("down")) {
				GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("dash");
				velocity.X = -DashSpeed;
				velocity.Y = DashSpeed;
				isDashing = true;
			}
			dashTimer = dashTimerReset;
			canDash = false;
		}
	}


	private void ShootProjectiles(){
		if (Input.IsActionJustPressed("projectile")) { 

				Bullet instBullet = (Bullet) bullet.Instantiate();
				instBullet.Speed = ButlletSpeed;
				instBullet.Position = GlobalPosition + BulletOffset;

				if(GetNode<AnimatedSprite2D>("AnimatedSprite2D").FlipH){
					instBullet.Speed *= -1;
					instBullet.GetChild<CollisionShape2D>(0).GetChild<Sprite2D>(0).FlipH = true;
				}


				GetTree().Root.AddChild(instBullet);


			}

	}



	public void takeDamage(int damage, Vector2 knockBack){

		Hp -= damage;

		velocity.X = knockBack.X * -facingDirection;
		velocity.Y = knockBack.Y;
		isTakingDamage = true;
	
		if (Hp <= 0){
			Hp = 0;
			isDead = true;
			GetNode<AnimatedSprite2D>("AnimatedSprite2D").Play("death"); 
			GD.Print("Skill issue");
		}
	}


	private void _on_animated_sprite_2d_animation_finished(){

		if ( GetNode<AnimatedSprite2D>("AnimatedSprite2D").Animation == "death"){
			
			GD.Print("Animation Finished");
			Hide();
			EmitSignal(nameof(Death));
		}

	}


	public void RespawnPlayer(){
		Show();
		Hp = 1;
		isDead = false;
	}


	private void InventoryOpen(){
		if (Input.IsActionJustPressed("inventory")){

			EmitSignal(nameof(Inventory));

		}
	}

	private void Paused(){

		if (Input.IsActionJustPressed("pause")){
			EmitSignal(nameof(Pause));
		}

	}
}
