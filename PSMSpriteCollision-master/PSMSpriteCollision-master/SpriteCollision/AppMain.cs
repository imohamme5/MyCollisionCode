using System;
using System.Collections.Generic;

using Sce.PlayStation.Core;
using Sce.PlayStation.Core.Environment;
using Sce.PlayStation.Core.Graphics;
using Sce.PlayStation.Core.Input;
using Sce.PlayStation.HighLevel.GameEngine2D;

namespace SpriteCollision
{
	public class AppMain
	{
		
		public static void Main (string[] args)
		{
			
			Initialize ();

			while (true) {
				SystemEvents.CheckEvents ();
				
				Director.Instance.Update();
				Director.Instance.GL.Context.Clear();
				Director.Instance.Render();
				Director.Instance.GL.Context.SwapBuffers();
				Director.Instance.PostSwap();
				
			}
		}

		public static void Initialize ()
		{
			// Set up the graphics system
			Director.Initialize();
			CollisionScene sceneToRun = new CollisionScene();
			
			Director.Instance.RunWithScene(sceneToRun);
		}

		
	}
}
