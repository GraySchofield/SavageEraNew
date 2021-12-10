using UnityEngine;
using UnityEngine.UI;

public class ToggleSprites : MonoBehaviour
{

	private Image _Background = null;


	public Sprite activeImage;
	public Sprite inactiveImage;

	public bool isCurrentActive;
	public bool isPreviousActive; 

	void Awake()
	{
		_Background = transform.GetComponent<Image>();
	}

	void Update(){
		if (isCurrentActive != isPreviousActive) {
			//state changed in this frame, need to toggle the sprite 
			if(isCurrentActive){
                //changed to active 
                if (activeImage != null)
                {
                    _Background.sprite = activeImage;
                }
                else
                {
                      _Background.color = new Color(1f, 0.843f, 0);
                }
            }
            else{
                //changed to inActive
                if(inactiveImage != null)
                {
                    _Background.sprite = inactiveImage;
                }
                else
                {
                      _Background.color = new Color(1f, 1f, 1f);
                }



            }
			isPreviousActive = isCurrentActive;  //update the previous state
		} 
	}



	



}
