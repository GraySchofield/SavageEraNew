using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {
	public float width = 32f;
	public float height = 32f;
	public int sizeX = 50;
	public int sizeY = 50;
	public Color color = Color.white;

	void OnDrawGizmos(){
		Vector3 pos = Camera.current.transform.position;
		Gizmos.color = color;

		for (float y = pos.y - height*sizeY; y < pos.y + height*sizeY; y += this.height) {
			Gizmos.DrawLine(new Vector3(pos.x - width*sizeX, Mathf.Floor(y/height)*height, 0f),
			                new Vector3(pos.x + width*sizeX, Mathf.Floor(y/height)*height, 0f));
		}


		for (float x = pos.x - width*sizeX; x < pos.x + width*sizeX; x += width) {
			Gizmos.DrawLine(new Vector3(Mathf.Floor(x/this.width)*this.width,pos.y - height*sizeY, 0f),
			                new Vector3(Mathf.Floor(x/this.width)*this.width,pos.y + height*sizeY, 0f));
		}
	}
	
}
