using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.HighLevel.GameEngine2D;
using Sce.PlayStation.HighLevel.GameEngine2D.Base;


namespace SpriteCollision
{
	public class CollisionScene :Scene
	{
		// Sprite declaration
		SpriteUV player;
		SpriteUV platform;
		SpriteUV platform2;
		SpriteUV platform3;

		List<Rectangle> platformsLists; 
		List<Rectangle> topEdges;
		List<Rectangle> leftEdges;
		List<Rectangle> bottomEdges;
		List<Rectangle> rightEdges;
		
		Rectangle playerBoundingBox;
		Rectangle top1, top2 , top3;
		Rectangle left1, left2, left3;
		Rectangle right1, right2, right3;
		Rectangle bottom1, bottom2, bottom3;

		
		bool gravity;
		bool jumped;
		bool canPlayerMoveLeft;
		bool canPlayerMoveRight;
		bool isOnPlatform = false;
		int jumpHeight;
		float startingY;
		private const int PLATFORM_OFFSET = 2;
		private const int BB_OFFSET = 10;
		private const int Left_OFFSET = 5;
		// SPEED CONSTANT
		const float MOVESPEED = 200;
		DrawHelpers dh;
		public CollisionScene ()
		{
			dh = new DrawHelpers(Director.Instance.GL, 200);
			platformsLists = new List<Rectangle>();
			topEdges = new List<Rectangle>();
			leftEdges = new List<Rectangle>();
			bottomEdges = new List<Rectangle>();
			rightEdges = new List<Rectangle>();
			gravity = true;
			canPlayerMoveLeft = true;
			canPlayerMoveRight = true;
			
			jumpHeight = 120;
			this.Camera.SetViewFromViewport();
			Scheduler.Instance.ScheduleUpdateForTarget(this, 0, false);
			// Load the sprites
			platform = new SpriteUV(new TextureInfo("/Application/Textures/Shape1.png"));
			platform2 = new SpriteUV(new TextureInfo("/Application/Textures/Shape1.png"));
			platform3 = new SpriteUV(new TextureInfo("/Application/Textures/Shape1.png"));
			player = new SpriteUV(new TextureInfo("/Application/Textures/Shape2.png"));
			///////////////////////////////////////////////////////////////////////////
			
		
			// Set the position and size of the sprites
			platform.Position = new Sce.PlayStation.Core.Vector2(0, 10);
			platform.Quad.S = platform.TextureInfo.TextureSizef;
			
			platform2.Position = new Sce.PlayStation.Core.Vector2(120, 120);
			platform2.Quad.S = platform2.TextureInfo.TextureSizef;
			
			platform3.Position = new Sce.PlayStation.Core.Vector2(120, 390);
			platform3.Quad.S = platform2.TextureInfo.TextureSizef;
			
			player.Position = new Sce.PlayStation.Core.Vector2(120, 190);
			player.Quad.S = player.TextureInfo.TextureSizef;
			//////////////////////////////////////////////////////////////////////////
			
			// Each update we re-create the rectangle around the sprite
			Rectangle box1 = new Rectangle(platform.Position.X  , platform.Position.Y, platform.TextureInfo.Texture.Width, platform.TextureInfo.Texture.Height);
			top1 = new Rectangle(platform.Position.X + BB_OFFSET, platform.Position.Y +  platform.TextureInfo.Texture.Height, platform.TextureInfo.Texture.Width - (BB_OFFSET * 2),
			                     PLATFORM_OFFSET);
			left1 = new Rectangle(platform.Position.X, platform.Position.Y - Left_OFFSET, platform.TextureInfo.Texture.Width , platform.TextureInfo.Texture.Height - (Left_OFFSET * 2)); 
			bottom1 = new Rectangle(platform.Position.X, platform.Position.Y, platform.TextureInfo.Texture.Width - (BB_OFFSET * 2), 1);
			right1 = new Rectangle(platform.Position.X + platform.TextureInfo.Texture.Width, platform.Position.Y - Left_OFFSET, Left_OFFSET, platform.TextureInfo.Texture.Height - (Left_OFFSET * 2)); 
			
			Rectangle box2 = new Rectangle(platform2.Position.X, platform2.Position.Y, platform2.TextureInfo.Texture.Width, platform2.TextureInfo.Texture.Height);
			top2 = new Rectangle(platform2.Position.X + BB_OFFSET, platform2.Position.Y +  platform2.TextureInfo.Texture.Height, platform2.TextureInfo.Texture.Width - (BB_OFFSET * 2)
			                     , PLATFORM_OFFSET);
			left2 = new Rectangle(platform2.Position.X, platform2.Position.Y  - Left_OFFSET, platform2.TextureInfo.Texture.Width , platform2.TextureInfo.Texture.Height - (Left_OFFSET * 2));
			bottom2 = new Rectangle(platform2.Position.X, platform2.Position.Y, platform2.TextureInfo.Texture.Width - (BB_OFFSET * 2), 1);
			right2 = new Rectangle(platform2.Position.X + platform2.TextureInfo.Texture.Width, platform2.Position.Y - Left_OFFSET, Left_OFFSET, platform2.TextureInfo.Texture.Height - (Left_OFFSET * 2)); 
			
			
			Rectangle box3 = new Rectangle(platform3.Position.X, platform3.Position.Y, platform3.TextureInfo.Texture.Width, platform3.TextureInfo.Texture.Height);
			top3 = new Rectangle(platform3.Position.X + BB_OFFSET, platform3.Position.Y +  platform3.TextureInfo.Texture.Height, platform3.TextureInfo.Texture.Width - (BB_OFFSET * 2)
			                     , PLATFORM_OFFSET);
			left3 = new Rectangle(platform3.Position.X , platform3.Position.Y - Left_OFFSET, platform3.TextureInfo.Texture.Width, platform3.TextureInfo.Texture.Height - (Left_OFFSET * 2));
			bottom3 = new Rectangle(platform3.Position.X, platform3.Position.Y, platform3.TextureInfo.Texture.Width - (BB_OFFSET * 2), 1);
			right3 = new Rectangle(platform3.Position.X + platform3.TextureInfo.Texture.Width, platform3.Position.Y - Left_OFFSET, Left_OFFSET, platform3.TextureInfo.Texture.Height - (Left_OFFSET * 2)); 
			
			playerBoundingBox = new Rectangle(player.Position.X, player.Position.Y, player.TextureInfo.Texture.Width, player.TextureInfo.Texture.Height);
			// Add the shapes to the Scene (see base class for CollisionScene - hence the use of this keyword)
			this.AddChild(platform);
			this.AddChild(platform2);
			this.AddChild(platform3);
			this.AddChild(player);
			platformsLists.Add(box1);
			platformsLists.Add(playerBoundingBox);
			platformsLists.Add(box2);
			platformsLists.Add(box3);
			topEdges.Add (top1);
			topEdges.Add (top2);
			topEdges.Add (top3);
			leftEdges.Add(left1);
			leftEdges.Add(left2);
			leftEdges.Add(left3);
			bottomEdges.Add(bottom1);
			bottomEdges.Add(bottom2);
			bottomEdges.Add(bottom3);
			rightEdges.Add(right1);
			rightEdges.Add(right2);
			rightEdges.Add(right3);
			
		}
		
		
		public override void Update (float dt)
		{
			// Get any user input and move the boxes
			if(Input2.GamePad0.Left.Down && canPlayerMoveLeft)
			{
				player.Position += new Vector2((-MOVESPEED * dt), 0);
			}
			if(Input2.GamePad0.Right.Down && canPlayerMoveRight)
			{
				player.Position += new Vector2((MOVESPEED * dt), 0);
			}
			if(Input2.GamePad0.Up.Down)
			{
				
				if(gravity == false)
				{
					startingY = player.Position.Y;
					Console.WriteLine(startingY);
					jumped = true;
				}
				
			}
			if(Input2.GamePad0.Down.Down)
			{
				player.Position += new Vector2(0, (-MOVESPEED * dt));
				jumped = false;
			}
			
			if (gravity == true)
			{
				player.Position += new Vector2(0, (-50 * dt));
			} 
			
			if(jumped == true)
			{	
				Console.WriteLine(player.Position.Y - startingY);
				if(player.Position.Y - startingY < jumpHeight) //startingY is only when the button is pressed so it gets the point
					player.Position += new Vector2(0, (MOVESPEED * dt));
				else
				{
					gravity = true;
					jumped = false;
				}
			}
			
			if(jumped == false && gravity == true)
			{
				player.Position += new Vector2(0, (-50 * dt));
			
			}
		
			// Update the boxes of the player and the platforms to represent any movement.
			platformsLists[0] = new Rectangle(platform.Position.X, platform.Position.Y, platform.TextureInfo.Texture.Width, platform.TextureInfo.Texture.Height);
			platformsLists[1] = new Rectangle(platform2.Position.X, platform2.Position.Y, platform2.TextureInfo.Texture.Width, platform2.TextureInfo.Texture.Height); 
			platformsLists[2] = new Rectangle(platform3.Position.X, platform3.Position.Y, platform3.TextureInfo.Texture.Width, platform3.TextureInfo.Texture.Height);
			playerBoundingBox = new Rectangle(player.Position.X, player.Position.Y, player.TextureInfo.Texture.Width, player.TextureInfo.Texture.Height);
			
			// TOP EDGES //
			topEdges[0] = new Rectangle(platform.Position.X + BB_OFFSET, platform.Position.Y +  platform.TextureInfo.Texture.Height, 
			                            platform.TextureInfo.Texture.Width - (BB_OFFSET * 2), PLATFORM_OFFSET);
			topEdges[1] = new Rectangle(platform2.Position.X + BB_OFFSET, platform2.Position.Y +  platform2.TextureInfo.Texture.Height, 
			                            platform2.TextureInfo.Texture.Width - (BB_OFFSET * 2), PLATFORM_OFFSET);
			topEdges[2] = new Rectangle(platform3.Position.X + BB_OFFSET, platform3.Position.Y +  platform3.TextureInfo.Texture.Height, 
			                            platform3.TextureInfo.Texture.Width - (BB_OFFSET * 2), PLATFORM_OFFSET);
			
			// Left Edges //
			leftEdges[0] = new Rectangle(platform.Position.X, platform.Position.Y - Left_OFFSET, platform.TextureInfo.Texture.Width, platform.TextureInfo.Texture.Height - (Left_OFFSET * 2));
			leftEdges[1] =  new Rectangle(platform2.Position.X, platform2.Position.Y - Left_OFFSET, platform2.TextureInfo.Texture.Width, platform2.TextureInfo.Texture.Height - (Left_OFFSET * 2));
			leftEdges[2] = new Rectangle(platform3.Position.X, platform3.Position.Y - Left_OFFSET, platform3.TextureInfo.Texture.Width, platform3.TextureInfo.Texture.Height - (Left_OFFSET * 2));
			
			//Bottom Edges/
			bottomEdges[0] = new Rectangle(platform.Position.X, platform.Position.Y, platform.TextureInfo.Texture.Width - (BB_OFFSET * 2), 1);
			bottomEdges[1] = new Rectangle(platform2.Position.X, platform2.Position.Y, platform2.TextureInfo.Texture.Width - (BB_OFFSET * 2), 1);
			bottomEdges[2] = new Rectangle(platform3.Position.X, platform3.Position.Y, platform3.TextureInfo.Texture.Width - (BB_OFFSET * 2), 1);
			
			//Right Edges
			rightEdges[0] = new Rectangle(platform.Position.X + platform.TextureInfo.Texture.Width, platform.Position.Y - Left_OFFSET, Left_OFFSET, platform.TextureInfo.Texture.Height - (Left_OFFSET * 2)); 
			rightEdges[1] = new Rectangle(platform2.Position.X + platform2.TextureInfo.Texture.Width, platform2.Position.Y - Left_OFFSET, Left_OFFSET, platform2.TextureInfo.Texture.Height - (Left_OFFSET * 2)); 
			rightEdges[2] = new Rectangle(platform3.Position.X + platform3.TextureInfo.Texture.Width, platform3.Position.Y - Left_OFFSET, Left_OFFSET, platform3.TextureInfo.Texture.Height - (Left_OFFSET * 2));
			
			DoTopEdgesIntersect(playerBoundingBox, topEdges);
			DoLeftEdgesIntersect(playerBoundingBox, leftEdges);
			DoBottomEdgesIntersect(playerBoundingBox, bottomEdges);
			DoRightEdgesIntersect(playerBoundingBox, rightEdges);
			//DoBoxesIntersect(playerBoundingBox ,boxList);
		
		}
		
		private void DoTopEdgesIntersect(Rectangle a, List<Rectangle> boxList)
		{
			foreach(Rectangle b in boxList)
			{
				
				if (a.X < b.X + b.Width && 
				    a.X + a.Width > b.X && 
				    a.Y < b.Y + b.Height && 
				    a.Height + a.Y > b.Y) 
				{
					isOnPlatform = true;
	    			Console.WriteLine("Collision Detected - TOP");
					gravity = false;
					canPlayerMoveLeft = true;
					canPlayerMoveRight = true;
					return;
				}
				else
				{
					isOnPlatform = false;
					gravity = true;
					canPlayerMoveLeft = true;
					canPlayerMoveRight = true;
					//Console.WriteLine("No Collision");	
				}
			}
		}
		
		private void DoLeftEdgesIntersect(Rectangle a, List<Rectangle> boxList)
		{
			foreach(Rectangle b in boxList)
			{
				
				if (a.X < b.X + b.Width && 
				    a.X + a.Width > b.X && 
				    a.Y < b.Y + b.Height && 
				    a.Height + a.Y > b.Y) 
				{
	    			Console.WriteLine("Collision Detected - LEFT");
					gravity = false;
					canPlayerMoveLeft = true;
					canPlayerMoveRight = false;
					return;
				}
				else
				{
					if(!isOnPlatform)
					{
						gravity = true;
					}
					canPlayerMoveLeft = true;
					canPlayerMoveRight = true;
					//Console.WriteLine("No Collision");	
				}
			}
		}
		
		private void DoBottomEdgesIntersect(Rectangle a, List<Rectangle> boxList)
		{
			foreach(Rectangle b in boxList)
			{
				
				if (a.X < b.X + b.Width && 
				    a.X + a.Width > b.X && 
				    a.Y < b.Y + b.Height && 
				    a.Height + a.Y > b.Y) 
				{
	    			Console.WriteLine("Collision Detected - Bottom");
					gravity = true;
					canPlayerMoveLeft = true;
					canPlayerMoveRight = false;
					return;
				}
				else
				{
					if(!isOnPlatform)
					{
						gravity = true;
					}
					canPlayerMoveLeft = true;
					canPlayerMoveRight = true;
					//Console.WriteLine("No Collision");	
				}
			}
		}
		
		private void DoRightEdgesIntersect(Rectangle a, List<Rectangle> boxList)
		{
			foreach(Rectangle b in boxList)
			{
				
				if (a.X < b.X + b.Width && 
				    a.X + a.Width > b.X && 
				    a.Y < b.Y + b.Height && 
				    a.Height + a.Y > b.Y) 
				{
	    			Console.WriteLine("Collision Detected - Right");
					gravity = true;
					canPlayerMoveLeft = false;
					canPlayerMoveRight = true;
					return;
				}
				else
				{
					if(!isOnPlatform)
					{
						gravity = true;
					}
					canPlayerMoveLeft = true;
					canPlayerMoveRight = true;
					//Console.WriteLine("No Collision");	
				}
			}
		}
	}
}

