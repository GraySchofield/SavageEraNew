using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonView {
	
	protected GameObject go;
	protected int index;
	protected int num_of_column;
	protected int padding_x;
    protected int padding_y;

	public ButtonView(string prefab, int index, int numOfColumn, int padding_y, int padding_x){
		go = GameObject.Instantiate (Resources.Load (prefab)) as GameObject;
		this.index = index;
		num_of_column = numOfColumn;
		this.padding_x = padding_x;
        this.padding_y = padding_y;
	}
	
	public void Remove(){
		GameObject.Destroy(go);
	}
	
	public void MoveTo(int newIndex){
		if (newIndex == index) {
			// no need to move
			return;
		}

		index = newIndex;

		RectTransform rf = go.GetComponent<RectTransform> ();
		
		rf.localScale = new Vector3 (1f, 1f, 1f);	
		int x = 0 % num_of_column; //0 ,1
		int y = index / num_of_column; // how many rows
		float width = rf.rect.width;
		float height = rf.rect.height;
		
		rf.anchoredPosition = new Vector2 (padding_x + width / 2 + x * (padding_x + width), 
		                                   -padding_y - height/2 - y*(padding_y + height));
		
	}
}