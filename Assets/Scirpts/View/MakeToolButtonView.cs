using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;



public class MakeToolButtonView: ButtonView  {
	
	public MakeToolButtonView(GameObject parentObject, MakeToolAction action, int index): base("Prefab/MakeButton", -1, 1, 15, 15){
		Tool tool = action.GetTool ();

		Button btn = go.transform.FindChild("Button").GetComponent<Button> ();
        Button info = go.GetComponent<Button>();
		go.transform.SetParent (parentObject.transform);

		MoveTo (index);
		
		btn.GetComponentInChildren<Text>().text = tool.Name;
		//go.GetComponentInChildren<Text> ().text = des;
		go.transform.FindChild("WorkRequires").GetComponent<Text> ().text = action.GetRequireString();
		//go.GetComponent<Animator> ().SetTrigger ("spawn");

		action.OnSuccess (delegate {
            if(Game.Current.GameTime < Config.SecondsPerDay && PlayerPrefs.GetInt("Step") == 2)
            {
                PlayerPrefs.SetInt("Step", 3);
                PlayerPrefs.Save();
            }
			GameObject.Find ("Click").GetComponent<AudioSource> ().Play ();
		});

        action.OnFail(delegate {
            GameObject.Find("Fail").GetComponent<AudioSource>().Play();
        });

        btn.onClick.AddListener (delegate {
			action.Run();
		});

        info.onClick.AddListener(delegate
        {
            GameObject detail_dialog = GameObject.Instantiate(Resources.Load("Prefab/PopUpInfoDialog")) as GameObject;
            detail_dialog.transform.SetParent(GameObject.Find("Canvas").transform, false);
            detail_dialog.transform.SetAsLastSibling();

            RectTransform dialog_rect_transform = detail_dialog.GetComponent<RectTransform>();
            dialog_rect_transform.offsetMax = new Vector2(0f, 0f);
            dialog_rect_transform.offsetMin = new Vector2(0f, 0f);
            Transform detail = detail_dialog.transform.FindChild("Detail");
            detail.FindChild("Header").GetComponentInChildren<Text>().text = action.Name;
            detail.FindChild("Description").GetComponent<Text>().text = action.GetProduce().Description;
        });

        info.onClick.AddListener(
            delegate {
                GameObject.Find("ClickWeak").GetComponent<AudioSource>().Play();
            });


    }
}
