using UnityEngine;
using System.Collections;

public class PlayerRaycasting : RaycastController {
	
	
	float maxClimbAngle = 70;
	float maxDescendAngle = 70;
	

	public CollisionInfo collisionData;
	
	public void Move(Vector3 velocity) {
		UpdateRaycastOrigins ();
		collisionData.Reset ();
		collisionData.velocityOld = velocity;
		
		if (velocity.y < 0) {
			DescendSlope(ref velocity);
		}
		
		if (velocity.x != 0) {
			HorizontalCollisions (ref velocity);
		}
		if (velocity.y != 0) {
			VerticalCollisions (ref velocity);
		}
		
		transform.Translate (velocity);
	}
	

	void VerticalCollisions(ref Vector3 velocity)
	{
		
		float directionY = Mathf.Sign (velocity.y);
		float rayLength = Mathf.Abs (velocity.y) + skinWidth;
		
		
		for (int i = 0; i < verticalRayCount; i++) {
			Vector2 rayOrigin = (directionY == -1)?raycastOrigins.bottomLeft:raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.up * directionY, rayLength, collisionMask);
			
			Debug.DrawRay (raycastOrigins.bottomLeft + Vector2.right * horizontalRaySpacing *i ,Vector2.up * -2,Color.red);
			
			if(hit){
				velocity.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;

					if(collisionData.climbingSlope)
					{
						
						velocity.x = velocity.y / Mathf.Tan (collisionData.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
						
					}
					
					collisionData.below = (directionY ==-1);
					
					if(hit.transform.tag != "JumpThrough")
						collisionData.above = (directionY == 1);
				}

			
		}
		
		if (collisionData.climbingSlope) {
			float directionX = Mathf.Sign (velocity.x);
			rayLength = Mathf.Abs (velocity.x) + skinWidth;
			Vector2 rayOrigen = ((directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight) + Vector2.up *velocity.y;
			RaycastHit2D hit = Physics2D.Raycast (rayOrigen, Vector2.right * directionX, rayLength, collisionMask);
			
			if(hit){
		
				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
				// We've collided with a new slope
				if(slopeAngle != collisionData.slopeAngle){
					velocity.x = (hit.distance - skinWidth) *directionX;
					collisionData.slopeAngle = slopeAngle;
					collisionData.rawSlopeAngle = hit.transform.gameObject.transform.rotation.z;
				}
				
			}
			
		}
	}
	
	
	/*
		if (collisionData.DetectedTag != "Icy") {
				else
				{
					print ("yeah on a slope biatch");
					velocity.x = Mathf.Sign (slopeAngle) * -1;
				}

	 * */
	
	
	void HorizontalCollisions(ref Vector3 velocity) {
		float directionX = Mathf.Sign (velocity.x);
		float rayLength = Mathf.Abs (velocity.x) + skinWidth;
		
		for (int i = 0; i < horizontalRayCount; i ++) {
			Vector2 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
			
			Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength,Color.red);
			
			if (hit) {
					float slopeAngle = Vector2.Angle (hit.normal, Vector2.up);
					
					if(i == 0 && slopeAngle <= maxClimbAngle){
						// if we're coming off of a descending slopes
						if(collisionData.descendingSope){
							collisionData.descendingSope = false;
							velocity = collisionData.velocityOld;
						}
						
						float distanceToSlopeStart = 0;
						// we're climbing a slope
						if(slopeAngle != collisionData.slopeAngleOld)
						{
							distanceToSlopeStart = hit.distance-skinWidth;
							velocity.x -= distanceToSlopeStart * directionX;
						}
						
						ClimbSlope(ref velocity, slopeAngle);
						velocity.x += distanceToSlopeStart*directionX;
					}
					
					if(!collisionData.climbingSlope || slopeAngle > maxClimbAngle)
					{
						velocity.x = (hit.distance - skinWidth) * directionX;
						rayLength = hit.distance;
						
						// This fixes the box on the slope problem
						if(collisionData.climbingSlope){
							velocity.y = Mathf.Tan (collisionData.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
						}
						
						collisionData.left = (directionX ==-1);
						collisionData.right = (directionX == 1);
					}
					

			}
		}
	}
	
	void ClimbSlope(ref Vector3 velocity, float slopeAngle){

			float moveDistance = Mathf.Abs (velocity.x);
			float climbVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
			
			if (velocity.y <= climbVelocityY) {
				velocity.y = climbVelocityY;
				velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (velocity.x);
				collisionData.climbingSlope = true;
				collisionData.below = true;
				collisionData.slopeAngle = slopeAngle;
			}

	}
	
	void DescendSlope(ref Vector3 velocity)
	{
		
		float directionX = Mathf.Sign (velocity.x);
		
		Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
		RaycastHit2D hit = Physics2D.Raycast (rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);
		
		if (hit) {
		
				float slopeAngle = Vector2.Angle (hit.normal, Vector2.up);
				if (slopeAngle != 0 && slopeAngle <= maxDescendAngle) {
					// We're moving down the slope
					if (Mathf.Sign (hit.normal.x) == directionX) {
						if (hit.distance - skinWidth <= Mathf.Tan (slopeAngle * Mathf.Deg2Rad) * Mathf.Abs (velocity.x)) {
							float moveDistance = Mathf.Abs (velocity.x);
							float descendVelocityY = Mathf.Sin (slopeAngle * Mathf.Deg2Rad) * moveDistance;
							velocity.x = Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign (velocity.x);
							velocity.y -= descendVelocityY;
							
							collisionData.slopeAngle = slopeAngle;
							collisionData.rawSlopeAngle = hit.transform.gameObject.transform.rotation.z;
							collisionData.descendingSope = true;
							collisionData.below = true;
						}
					}
				}

		}
		
	}
	
	
	
	public struct CollisionInfo{
		public bool above, below;
		public bool left, right;
		
		public bool descendingSope;
		public bool climbingSlope;
		public float slopeAngle, slopeAngleOld;
		public float rawSlopeAngle;

		public Vector3 velocityOld;
		
		public void Reset()
		{
			descendingSope = false;
			climbingSlope = false;
			above = below = false;
			left = right = false;
			slopeAngleOld = slopeAngle;
			slopeAngle = 0;
			rawSlopeAngle = 0;
		}
		
	}
}
