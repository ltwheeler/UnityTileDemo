using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	
	void OnMouseDown() {
		Sprite blueSquare = Resources.Load<Sprite>("blueSquare");
		Sprite redSquare = Resources.Load<Sprite>("redSquare");

		SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer> ();
		if (spriteRenderer.sprite == redSquare) {
			spriteRenderer.sprite = blueSquare;
		} else {
			spriteRenderer.sprite = redSquare;
		}
	}
}
