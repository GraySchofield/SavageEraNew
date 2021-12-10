using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopInventoryRowView  {

    private GameObject resource_row;
    private Text resource_row_text;
    private RectTransform resource_row_rect;

    public ShopInventoryRowView(GameObject parentObject, StackableItem res, int index)
    {
        resource_row = GameObject.Instantiate(Resources.Load("Prefab/ShopResourceRow")) as GameObject;
        resource_row_text = resource_row.GetComponent<Text>();
        resource_row_rect = resource_row.GetComponent<RectTransform>();
        resource_row_text.text = res.Name + " " + res.Count;

        float height_constant = resource_row_rect.rect.height;
        resource_row_rect.anchorMin = new Vector2(0, 1);
        resource_row_rect.anchorMax = new Vector2(1, 1);
        resource_row_rect.localScale = new Vector3(1f, 1f, 1f);
        resource_row_rect.sizeDelta = new Vector2(resource_row_rect.rect.width, height_constant);
        resource_row_rect.offsetMin = new Vector2(45f, -(index + 1) * (5f + height_constant));
        resource_row_rect.offsetMax = new Vector2(-45f, -5 - index * (5f + height_constant));
        resource_row.transform.SetParent(parentObject.transform, false);
    }


   
    public void Remove()
    {
        GameObject.Destroy(resource_row);
    }
}
