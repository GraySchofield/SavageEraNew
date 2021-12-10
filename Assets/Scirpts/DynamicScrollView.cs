using UnityEngine;
using System.Collections;

public class DynamicScrollView : MonoBehaviour {
	//any scrollcontent with this scrollview attached 
	//will be able to change it's own size base on the number of children
	private int current_row_count = 0;
	private int preivous_row_count = 0;
	private RectTransform this_rf;
	private float child_height = 0f;
	public int child_per_row = 1;

	public float padding = 10f;
    public float additional_dialog_padding = 0f;

	// Use this for initialization
	void Start () {
		this_rf = GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (gameObject.activeSelf) {
			current_row_count = Mathf.CeilToInt (transform.childCount / child_per_row);
			if (current_row_count != preivous_row_count) {
				//dynamically change the size 
				if (current_row_count > 0) {
					//when at least have one child
					if (child_height == 0) {
						child_height = transform.GetChild (0).GetComponent<RectTransform> ().rect.height;
					}

					//	this_rf.sizeDelta = new Vector2(this_rf.rect.x, child_height * current_children_count);
					this_rf.offsetMin = new Vector2 (0f, -(child_height + padding) * (current_row_count + 1) - additional_dialog_padding);
					this_rf.offsetMax = new Vector2 (0f, 0f);

				}
			}

		
			preivous_row_count = current_row_count;
		}
	}
}
