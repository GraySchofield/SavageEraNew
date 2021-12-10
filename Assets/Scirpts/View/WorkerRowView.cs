using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class WorkerRowView  {

	private GameObject work_row;
	private Text worker_title;
	private Text worker_count;


	public void UpdateWorkerNumber(int number){
		if (worker_count != null) {
			worker_count.text =  "" + number;
		}
	}



	public WorkerRowView(GameObject parentObject, string woker_name, int worker_number, int index, 
	                     UnityAction addAction,
	                     UnityAction minusAction){
		work_row = GameObject.Instantiate (Resources.Load ("Prefab/WorkerRow")) as GameObject;
		int padding = 10;
		RectTransform rf = work_row.GetComponent<RectTransform> ();
		Button btn_add = work_row.transform.FindChild ("WorkerAdd").GetComponent<Button> ();
		Button btn_minus = work_row.transform.FindChild ("WorkerMinus").GetComponent<Button> ();
		worker_title = work_row.transform.FindChild ("WorkerTitle").GetComponent<Text> ();
		worker_count = work_row.transform.FindChild ("WorkerCount").GetComponent<Text> ();

		work_row.transform.SetParent (parentObject.transform);
		
		rf.localScale = new Vector3 (1f, 1f, 1f);	
		int x = 0; //0 ,1
		int y = index; // how many rows
		float width = rf.rect.width;
		float height = rf.rect.height;
		
		rf.anchoredPosition = new Vector2 (width / 2, 
		                                   -padding - height/2 - y*(padding + height));
		

		worker_title.text = woker_name;
		worker_count.text = "" + worker_number;
		btn_add.onClick.AddListener (addAction); // what to do when add worker
        btn_add.onClick.AddListener(
            delegate {
                GameObject.Find("ClickWeak").GetComponent<AudioSource>().Play();
            });
        btn_minus.onClick.AddListener (minusAction); //what to do when minus worker
        btn_minus.onClick.AddListener(
           delegate {
               GameObject.Find("ClickWeak").GetComponent<AudioSource>().Play();
           });
    }


	//update the worker number
	public void ReDrawNumber(int number){
		Text worker_count = work_row.transform.FindChild ("WorkerCount").GetComponent<Text> ();
		worker_count.text = "" + number;
	}


	public void Remove(){
		GameObject.Destroy(work_row);
	}
	
	public void MoveTo(int index){
		int padding = 10;
		RectTransform rf = work_row.GetComponent<RectTransform> ();
		rf.localScale = new Vector3 (1f, 1f, 1f);	
		int x = 0; //0 ,1
		int y = index; // how many rows
		float width = rf.rect.width;
		float height = rf.rect.height;
		
		rf.anchoredPosition = new Vector2 (width / 2 , 
		                                   -padding - height/2 - y*(padding + height));
		
	}
}
