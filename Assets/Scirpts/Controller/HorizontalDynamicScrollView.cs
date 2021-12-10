using UnityEngine;
using System.Collections;

public class HorizontalDynamicScrollView : MonoBehaviour {
    private int current_col_count = 0;
    private int preivous_col_count = 0;
    private RectTransform this_rf;
    private float child_width = 0f;
    public int child_per_col = 1;

    public float padding = 10f;
    public float additional_dialog_padding = 0f;

    // Use this for initialization
    void Start()
    {
        this_rf = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            current_col_count = Mathf.CeilToInt(transform.childCount / child_per_col);
            if (current_col_count != preivous_col_count)
            {
                //dynamically change the size 
                if (current_col_count > 0)
                {
                    //when at least have one child
                    if (child_width == 0)
                    {
                        child_width = transform.GetChild(0).GetComponent<RectTransform>().rect.width;
                    }

                    //	this_rf.sizeDelta = new Vector2(this_rf.rect.x, child_height * current_children_count);
                    this_rf.offsetMin = new Vector2(0f, 0f);
                    this_rf.offsetMax = new Vector2((child_width + padding) * (current_col_count + 1) + additional_dialog_padding, 0f);

                }
            }


            preivous_col_count = current_col_count;
        }
    }
}
