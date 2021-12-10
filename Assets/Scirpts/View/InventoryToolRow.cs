using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryToolRow {
	private GameObject tool_row;
	private Text tool_row_text;
	private RectTransform tool_row_rect;

	public InventoryToolRow(Transform parentTransform,List<Tool> current_tools, int idx){

		tool_row = GameObject.Instantiate (Resources.Load ("Prefab/ToolRow")) as GameObject;
	
		tool_row_text = tool_row.GetComponentInChildren<Text>();
		tool_row_rect = tool_row.GetComponent<RectTransform>();
        Tool the_first_tool = current_tools[0];
       // if (the_first_tool.IsOneTime) {
			tool_row_text.text = the_first_tool.Name  + "*" + current_tools.Count; 
		//} else {
		//	tool_row_text.text = current_tool.Name  + " " + (Mathf.RoundToInt(current_tool.Remaining * 100))  + "%"; 
		//}

		float height_constant = tool_row_rect.rect.height;
		tool_row_rect.anchorMin = new Vector2(0,1);
		tool_row_rect.anchorMax = new Vector2(1,1);
		tool_row_rect.localScale = new Vector3(1f,1f,1f);
		tool_row_rect.sizeDelta = new Vector2( tool_row_rect.rect.width, height_constant);
		tool_row_rect.offsetMin = new Vector2(22.5f, -(idx+1)*(5f + height_constant));
		tool_row_rect.offsetMax = new Vector2(-22.5f, -5 - idx*(5f + height_constant));
		
		tool_row.transform.SetParent(parentTransform,false);
		tool_row.GetComponent<Button>().onClick.AddListener(delegate {
			//inistantiate the detail Dialog based on the Tool !
			//Debug.LogError("Clicked !");
			GameObject tool_detail_top = GameObject.Instantiate (Resources.Load ("Prefab/ToolDetailDialog")) as GameObject;
			tool_detail_top.transform.SetParent(GameObject.Find ("Canvas").transform);
			tool_detail_top.transform.SetAsLastSibling(); // move the dialog to the front of the View
			RectTransform tool_parent_rect =  tool_detail_top.GetComponent<RectTransform>();
			tool_parent_rect.localScale = new Vector3(1f,1f,1f);
			tool_parent_rect.offsetMax = new Vector2(0f,0f);
			tool_parent_rect.offsetMin = new Vector2(0f,0f);
			

			Transform tool_detail_view = tool_detail_top.transform.FindChild("ToolDetail");
			//set title
			tool_detail_view.FindChild("Header").GetComponentInChildren<Text>().text = the_first_tool.Name;
			tool_detail_view.FindChild("Description").GetComponent<Text>().text = the_first_tool.Description;


			if(the_first_tool.IsOneTime){
				tool_detail_view.FindChild("Use").GetComponentInChildren<Text>().text = Lang.Current["use_up"];
			}else{
				if(the_first_tool.IsEquipment){
					tool_detail_view.FindChild("Use").GetComponentInChildren<Text>().text = Lang.Current["set_up"];
				}else{
					tool_detail_view.FindChild("Use").GetComponentInChildren<Text>().text = Lang.Current["take"];
				}
			}
			tool_detail_view.GetComponent<ToolDetailDialogController>().theToolType = the_first_tool.Type; // pass the item to the controller
			
			RectTransform tool_rect = tool_detail_view.GetComponent<RectTransform>();
			
			tool_rect.anchoredPosition3D = new Vector3(0, 0, 0);
			tool_rect.localScale = new Vector3(1f,1f,1f);
		});

        tool_row.GetComponent<Button>().onClick.AddListener(
           delegate {
               GameObject.Find("ClickWeak").GetComponent<AudioSource>().Play();
           });


    }

	public void Remove(){
		GameObject.Destroy (tool_row);
	}

	public void UpdateToolRow(List<Tool> current_tools , int idx){
		if (tool_row_text != null) {

			tool_row_text.text = current_tools[0].Name  + "*" + current_tools.Count;
             
			if(current_tools.Contains(Game.Current.Hero.EquippedTool) ||
                   current_tools.Contains(Game.Current.Hero.Equipment))
            {
				tool_row_text.color = Color.yellow;
			}else{
				tool_row_text.color = Color.white;
			}
		}
		//move the the right position
		float height_constant = tool_row_rect.rect.height;
		tool_row_rect.offsetMin = new Vector2(22.5f, -(idx+1)*(5f + height_constant));
		tool_row_rect.offsetMax = new Vector2(-22.5f, -5 - idx*(5f + height_constant));

		//may need to update image
	}
 

}
