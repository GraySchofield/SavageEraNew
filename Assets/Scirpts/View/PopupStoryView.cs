using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PopupStoryView {
	private GameObject story_view;

	public PopupStoryView(string story_description, List<Option> options){
		story_view = GameObject.Instantiate (Resources.Load ("Prefab/StoryDetailDialog")) as GameObject;
	
		story_view.transform.SetParent(GameObject.Find ("Canvas").transform);
		story_view.transform.SetAsLastSibling(); // move the dialog to the front of the View
		RectTransform rect =  story_view.GetComponent<RectTransform>();
		rect.localScale = new Vector3(1f,1f,1f);
		rect.offsetMax = new Vector2(0f,0f);
		rect.offsetMin = new Vector2(0f,0f);
		
		
		Transform story_detail_view = story_view.transform.FindChild("StoryDetail");
		Transform options_panel = story_detail_view.FindChild("OptionsPanel");
		//set title
		story_detail_view.FindChild("Header").GetComponentInChildren<Text>().text = Lang.Current["story"];
		story_detail_view.FindChild("Description").GetComponent<Text>().text = story_description;

		for (int i = 0; i < options.Count; i++) {
			//instantiate button and add to option panel
			GameObject option_button = GameObject.Instantiate (Resources.Load ("Prefab/OptionButton")) as GameObject;
			option_button.transform.SetParent(options_panel);
			RectTransform rf = option_button.GetComponent<RectTransform>();
            float padding = (options_panel.GetComponent<RectTransform>().rect.width - options.Count * rf.rect.width) / (options.Count + 1);
			rf.localScale = new Vector3(1f,1f,1f);
			rf.anchoredPosition3D = new Vector3((padding + rf.rect.width) * (i+1) - rf.rect.width/2,
			                                    -40f - rf.rect.height/2,0f);

			option_button.GetComponentInChildren<Text>().text = options[i].Name;

			Button btn = option_button.GetComponent<Button>();
			Option current_option = options[i];
			btn.onClick.AddListener(delegate {
				current_option.Run();
				Remove();
			});
		}
	}


	public void Remove(){
		GameObject.Destroy (story_view);
	}

}
