using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MapListController : MonoBehaviour {
	public Transform mapRowPrefab;
	public Transform mapRowEquippedPrefab;
	public Transform mapDetailPrefab;

	public void RenderMapList(){
		List<Map> maps = Game.Current.Hero.AllMaps;
		foreach (Transform childTransform in transform) Destroy(childTransform.gameObject);
		for (int i = 0; i < maps.Count; i ++) {
			//add the items to the view 
			Map current_map = maps[i];
			Transform clone;
			if(current_map == Game.Current.Hero.CurrentEquippedMap){
				clone = Instantiate(mapRowEquippedPrefab) as Transform;
			}else{
				clone = Instantiate(mapRowPrefab) as Transform;
			}
			Text name = clone.GetComponentInChildren<Text>();
			RectTransform rf = clone.GetComponent<RectTransform>();
			if(name == null){
				Debug.LogError("UI null!");
			}
			name.text = current_map.Name ;
			clone.SetParent(transform);
			float height_constant = rf.rect.height;
			rf.anchorMin = new Vector2(0,1);
			rf.anchorMax = new Vector2(1,1);
			rf.localScale = new Vector3(1f,1f,1f);
			rf.offsetMin = new Vector2(22.5f, -(i+1)*(8f + height_constant));
			rf.offsetMax = new Vector2(-22.5f, -5 - i*(8f + height_constant));
			clone.GetComponent<Button>().onClick.AddListener(delegate {
				//show or hide gears detail
				clone.SetAsLastSibling(); //need to move front
				if(clone.FindChild("GeneralListDetail(Clone)") != null){
					//close 
					Destroy(clone.FindChild("GeneralListDetail(Clone)").gameObject);
				}else{
					//open
					Transform map_detail = Instantiate(mapDetailPrefab) as Transform;
					map_detail.SetParent(clone);
					map_detail.FindChild("Title").GetComponent<Text>().text = current_map.Name;
					map_detail.FindChild("InformationPanel").FindChild("Content").GetComponent<Text>().text = current_map.Description;
					RectTransform detail_rf  = map_detail.GetComponent<RectTransform>();
					detail_rf.localScale = new Vector3(1f,1f,1f);
                    detail_rf.offsetMin = new Vector2(5f, -detail_rf.rect.height);
					detail_rf.offsetMax = new Vector2(-5f, 0f);
					
				}
			});
			clone.FindChild("Add").GetComponent<Button>().onClick.AddListener(delegate {
				//add Weapon to player gear	
				Game.Current.Hero.CurrentEquippedMap =  current_map;
                transform.parent.parent.GetComponent<FullScreenPopUpViewController>().CloseCurrentSelf();
                GameObject.Find("Click").GetComponent<AudioSource>().Play();

            });
		} 
	}

}
