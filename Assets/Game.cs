using UnityEngine;
using System.Collections;

// UNITY IDE INITIAL WORK:
// To set this up in the Unity editor, we
// 1) created a folder in Assets called Resources
// 2) put two images in that folder
// 3) Set the 'Pixels Per Unit' to 100 (it defaulted to that anyways)
// 4) Made a GameObject
// 5) Attached a new C# script to it and called it 'Game'
// 6) Hit 'File->Save Scene' as MainScene

// Scenes: The scene is where you are creating and arranging GameObjects, the scene is the world
// you can do everything on one scene, or use multiple scenes, its up to you

// CONCEPTUAL: Extending the class MonoBehavior means 'I should be attached to a game object'
public class Game : MonoBehaviour {

	// These variables are explained where they're used
	private float config_orthographicSize = 5;
	private float aspect = (float) Screen.width / (float) Screen.height;
	Sprite whiteSquare;
	Sprite blackSquare;
	Tile[,] tileMap = new Tile[5, 5];
	Vector2 topLeftTileLocation = new Vector2(2,8);
	bool spriteColorToggle = true;

	// Use this for initialization of stuff
	void Start () {
		//------------------------
		// LOOK AT ME!!! ---------
		// -----------------------

		// Begin the process of setting the stage

		// Make a camera
		createAndConfigureCamera ();

		// Make and arrange tiles on the screen
		createAndArrangeTiles ();
	}

	void createAndConfigureCamera() {

		// FUNCTIONAL: Create a Gameobject
		// CONCEPTUAL: A GameObject is just a bucket that can hold components. 
		// Components do all kinds of interesting things in unity,
		// But the GameObject is just a bucket.
		var cameraGameObject = new GameObject("MainCamera");
		
		// FUNCTIONAL: Attach the 'Camera' Component
		// CONCEPTUAL: The camera is the 'window' we look through to see the 'world'
		var cameraComponent = cameraGameObject.AddComponent<Camera>();
		
		// FUNCTIONAL: Set the camera to orthographic projection mode
		// CONCEPTUAL: An orthographic camera is not affected by an object's distance from camera
		// Objects in orthographic mode appear exactly the same size regardless of how close or far they are from the camera
		cameraComponent.orthographic = true;
		
		// FUNCTIONAL: Put bottom left of the camera at (0,0) world coordinates 
		// and move it -1 on the z-axis 
		// CONCEPTUAL: (GameObjects will be on z=0, so by pulling the camera back everything else will always be in front of the camera)
		// aspect is calculated relative to the actual screen being used at runtime.
		cameraComponent.transform.position = new Vector3 (config_orthographicSize * aspect, config_orthographicSize, -1);
		
		// FUNCTIONAL: 'orthographicSize' is the distance fron the center of the camera to the top or bottom of the visible area
		// It is half the total height of the camera's viewing size
		// Horizontal viewing size varies depending on the viewport's aspect ratio and will adjust automatically
		// CONCEPTUAL: By manipulating this number, you increase or decrease the size of the 'window' you use when looking at the world
		// Increasing orthographicSize will make objects appear smaller on the eventual 'viewport' that the game appears in
		cameraComponent.orthographicSize = config_orthographicSize;

		// FUNCTIONAL: setting the background color for the camera
		// CONCEPTUAL: the background color is the color for 'nothingness' and will only be seen 
		// if your sprites don't completely cover the camera's viewing area
		cameraComponent.backgroundColor = Color.gray;
	}

	void createAndArrangeTiles () {

		Tile tile;
		float x;
		float y;

		// Position tiles
		for (int i = 1; i <= 5; i++) {
			for (int j = 1; j <= 5; j++) {

				// Create tile
				tile = createTile ();

				// FUNCTIONAL: Position tile
				// CONCEPTUAL: In unity points are represented as a vector2 (2-dimensional vector)
				// For our purposes we can think of them as points, with some extra functionality if we want it.
				// we add 1 each time to the x value because the Asset is 100x100 pixels at 100 'pixels per unit, 
				// which turns into 1x1 Units (units in unity are what world coordinates are measured in)
				x = topLeftTileLocation.x + (i*1);
				y = topLeftTileLocation.y - (j*1);
				tile.transform.position = new Vector2(x,y);

				// FUNCTIONAL: store the reference to the Tile component in the tileMap multidimensional array
				// CONCEPTUAL: we store the reference to the Tile component, as opposed to the GameObject its tied to,
				// because Tile is where our logic/behaviors/interesting stuff is happening
				// We can query its GameObject parent and fellow components if we decide we want them easily enough
				tileMap[i-1,j-1] = tile;
			}
		}

	}

	Tile createTile() {
		// Controlling the size of things on the screen follows this pipeline:
		// Asset Import property 'pixels per units' -> world units -> GameObject scaling/transforms -> camera's orthographicSize -> aspect ratio of device -> viewport output
		
		// Asset Import property 'Pixels Per Unit' dictates how to translate file resolution to world units
		// Making 'Pixels Per Units' Bigger makes the object smaller on the screen

		// FUNCTIONAL: Load the Sprite Resource into a variable so we can apply it to things
		// CONCEPTUAL: Resources.Load looks inside the 'Assets' directory for a 'Resources' directory, and then for an Asset named
		// 'blueSquare' in the Resources directory
		// It will not automatically look inside children directories, so if you had Assets/Resources/Sprites/blueSquare
		// you would use the following Resources.Load<Sprite>("Sprites/blueSquare");
		Sprite blueSquare = Resources.Load<Sprite>("blueSquare");
		Sprite redSquare = Resources.Load<Sprite>("redSquare");

		// Making another GameObject like above
		GameObject tileGO = new GameObject();

		// FUNCTIONAL: attach a component of type Tile, which corresponds to a Script
		// CONCEPTUAL: Unity has a variety of components, but all the scripts you write are also components
		// Scripts do not necessarily need to be attached to objects, but doing so allows them to fire in response to events,
		// fire in response to physics engine stuff, response to mouse clicks, run code every frame, and other things
		// Scripts not on a GameObject are cut off from these stimuli and will need to be triggered 
		// by scripts that ARE on GameObjects
		Tile tile = tileGO.AddComponent<Tile>();

		// FUNCTIONAL: Adds a BoxCollider2D component to the GameObject
		// CONCEPTUAL: if you want to be able to detect clicks hitting the GameObject you need this. 
		// A collider (there are a few kinds beyond boxcolliders) can be thought of as the 'physical' body 
		// that will be used for interacting with the physics engine,
		// detecting collisions, etc. By default they try to match the size of their mesh (in this case the sprite)
		// The GameObject, SpriteRenderer, and Tile script have no physicality to them
		tileGO.AddComponent<BoxCollider2D> ();

		// FUNCTIONAL: Adds a SpriteRenderer component to the GameObject
		// CONCEPTUAL: If colliders make something 'physical', Renderers make something 'visible'- a SpriteRender is 
		// specialized in rendering sprites, but there are other kinds available
		SpriteRenderer spriteRenderer = tileGO.AddComponent<SpriteRenderer> ();

		// FUNCTIONAL: Simple toggle logic which then sets a sprite into the SpriteRenderer
		// CONCEPTUAL: SpriteRenderer's render sprites. 
		if (spriteColorToggle) {
			spriteRenderer.sprite = blueSquare;
			spriteColorToggle = false;
		} else {
			spriteRenderer.sprite = redSquare;
			spriteColorToggle = true;;

		}

		// CONCEPTUAL: we return tile as opposed to the tileGO, because tile references the Tile script
		// which may have fields and methods that will be nice to have easy access to
		// The GameObject itself and built in unity components that are attached have simple configuration tweaks you can make
		// But all the complexity of your app will be in the scripts you write yourself, so we choose to make them easier to get at
		return tile;
	}	
}