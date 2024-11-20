using Godot;
using System;


public partial class Player : CharacterBody2D
{


	// PLAYER RELATED PARAMS

		// GENERAL

		private bool _isDead = false;
		private AnimatedSprite2D animatedSprite;

		private bool _isFalling = false;

		// SPEED AND JUMP

		[Export]
		public float Speed = 300.0f;

		[Export]
		public float JumpVelocity = -220.0f;

		private int _jumpcount = 0;

		private bool _isJumping = false;

		private bool _canJump = true;

		private double totalJumpTime = 0.6;

		private double jumpTimer = 0.6;


		// ATTACKING

		[Export]
		public double totalAttackDuration = 0.7;
		public double attackTimer = 0.7;
		public bool _isAttacking = false;
		public bool _canAttack = true;

		// DASHING

		[Export]
		public double totalDashDuration = 0.6;

		[Export]
		public int dashDistance = 70;

		public double dashTimer = 0.6;
		private Vector2 _dashDirection = new Vector2(1, 0);
		private bool _canDash = true;
		private bool _isDashing = false;


	// PROYECTILE RELATED PARAMS

		[Export]
		public Vector2 BulletOffset = new Vector2(0, -5);

		[Export]
		public float ButlletSpeed = 1;

		private PackedScene bullet;

	




	public override void _Ready()
	{
	   animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	   bullet = (PackedScene) ResourceLoader.Load("res://scenes/proyectil.tscn");

	   
	}


	public override void _PhysicsProcess(double delta)
	{
		
		Vector2 velocity = Velocity;
		float directionX = Input.GetAxis("moveLeft", "moveRight");
				
		if (Input.IsActionJustPressed("dash") && _canDash){
				_isDashing = true;
			}

			
		if (Input.IsActionJustPressed("attack") && _canAttack){
			_isAttacking = true;
		}

		Dash();
		Attack();
		Jump();


		if (!_isDead){
			
			// Add the gravity.
			if (!IsOnFloor()) {

				velocity += GetGravity() * (float)delta;
				
				if (directionX > 0){
					velocity.X = directionX * Speed;
					animatedSprite.FlipH = false;

					if (!_isAttacking && !_isDashing && !_isJumping){						
						if (Falling()){
							animatedSprite.Play("fall");
						}
					}
					else if (_isAttacking && !_isDashing && !_isJumping){
						Attack();
						AttackTimer(delta);
						_isAttacking = false;
					}
					else if (!_isAttacking && _isDashing && !_isJumping){
						DashTimer(delta);
						Dash();
						_isDashing = false;
					}
					else if(!_isAttacking && !_isDashing && _isJumping){
						JumpTimer(delta);
						Jump();
						_isJumping = false;
					}

				}
				else if (directionX < 0){
					velocity.X = directionX * Speed;
					animatedSprite.FlipH = true;

					if (!_isAttacking && !_isDashing && !_isJumping){					
						if (Falling()){
							animatedSprite.Play("fall");
						}
					}
					else if (_isAttacking && !_isDashing && !_isJumping){
						Attack();
						AttackTimer(delta);
						_isAttacking = false;
					}
					else if (!_isAttacking && _isDashing && !_isJumping){
						DashTimer(delta);
						Dash();
						_isDashing = false;
					}
					else{
						JumpTimer(delta);
						Jump();
						_isJumping = false;
					}
					
				}
				else{
					velocity.X = directionX * Speed;
					if (!_isAttacking && !_isDashing && !_isJumping){
						if (Falling()){
							animatedSprite.Play("fall");
						}
					}
					else if (_isAttacking && !_isDashing && !_isJumping){
						Attack();
						AttackTimer(delta);
						_isAttacking = false;
					}
					else if (!_isAttacking && _isDashing && !_isJumping){
						DashTimer(delta);
						Dash();
						_isDashing = false;
					}
					else{
						JumpTimer(delta);
						Jump();
						_isJumping = false;
					}
				}		
			}

			
			// JUMP
			if (Input.IsActionJustPressed("jump") && IsOnFloor()) {
				Jump();
				velocity.Y = JumpVelocity;
				if (velocity.X < 0) {
					animatedSprite.FlipH = true;
				}
				else if (velocity.X > 0){
					animatedSprite.FlipH = false;
				}
					
			}
			
			
			


			if (Input.IsActionJustPressed("projectile")) { 

				Bullet instBullet = (Bullet) bullet.Instantiate();
				instBullet.Speed = ButlletSpeed;
				instBullet.Position = GlobalPosition + BulletOffset;

				if(animatedSprite.FlipH){
					instBullet.Speed *= -1;
					instBullet.GetChild<CollisionShape2D>(0).GetChild<Sprite2D>(0).FlipH = true;
				}


				GetTree().Root.AddChild(instBullet);


			}

			
			if (IsOnFloor()){
				_canJump = true;
				if(directionX > 0) {

					animatedSprite.FlipH = false;
					velocity.X = Speed;
					if(!_isAttacking && !_isDashing){
						animatedSprite.Play("walk");
					}
					else if(!_isDashing){
						Attack();
						AttackTimer(delta);
						_isAttacking = false;
					}
					else{
						
						DashTimer(delta);
						Dash();
						_isDashing = false;
					}
					
					
					
				}
				else if(directionX < 0){

					animatedSprite.FlipH = true;
					velocity.X = -Speed;
					if(!_isAttacking && !_isDashing){
						animatedSprite.Play("walk");
					}
					else if(!_isDashing){
						AttackTimer(delta);
						Attack();
						_isAttacking = false;
					}
					else{
						
						DashTimer(delta);						
						Dash();
						_isDashing = false;
					}
					
				}
				else {
					
					if(!_isAttacking && !_isDashing){
						animatedSprite.Play("idle");
						velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
					}
					else if(!_isDashing){
						AttackTimer(delta);
						Attack();
						_isAttacking = false;
					}
					else{	
											
						DashTimer(delta);
						Dash();
						_isDashing = false;
					}
					

					
				}
			}	

		
		}
		
		Velocity = velocity;
		MoveAndSlide();
	}




	private void Jump(){
		if (Input.IsActionJustPressed("jump") && _canJump){

			_canJump = false;
			animatedSprite.Play("jump");
		}
	}




	// Simplified attack function
	private void Attack(){

		
		if (Input.IsActionJustPressed("attack") && _canAttack){
			
			_canAttack = false;
			animatedSprite.Play("attack");
			_canAttack = true;

		}
		
		
	}


	//Simplified dash function

	private void Dash() {

		if (Input.IsActionJustPressed("dash") && _canDash){
			_canDash = false;
			animatedSprite.Play("dash");
			Velocity = _dashDirection.Normalized() * 200;

			if(!animatedSprite.FlipH){
				
					this.Position = new Vector2(this.Position.X + (dashDistance	), this.Position.Y);
				
			}
			else{
				
					this.Position = new Vector2(this.Position.X - (dashDistance), this.Position.Y);
				
			
			}
	
			_canDash = true;
			
					
		}
	}




	private bool Falling(){
		return !IsOnFloor() && this.Velocity.Y > 0;
	}



	private void DashTimer(Double delta){
		dashTimer -= delta;
		if(dashTimer <= 0){
			dashTimer = totalDashDuration;
		}
	}

	
	private void JumpTimer(Double delta){

		jumpTimer -= delta;
		if(jumpTimer <= 0){
			jumpTimer = totalJumpTime;
		}

	}
	

	private void AttackTimer(double delta){
		attackTimer -= delta;
		if (attackTimer <= 0){
			attackTimer = totalAttackDuration;
		}
	}

}
