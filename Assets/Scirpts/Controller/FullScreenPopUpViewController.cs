using UnityEngine;
using System.Collections;

public class FullScreenPopUpViewController : MonoBehaviour {
    private  CanvasGroup ChangePanelGroup;
    private CanvasGroup StoryPanelGroup;

    void Start()
    {
        ChangePanelGroup =  GameObject.Find("PanelChangeable").GetComponent<CanvasGroup>();
        StoryPanelGroup = GameObject.Find("StoryPannel").GetComponent<CanvasGroup>();
        ChangePanelGroup.alpha = 0;
        StoryPanelGroup.alpha = 0;

    }

	public void CloseSelf(){
        ChangePanelGroup.alpha = 1;
        StoryPanelGroup.alpha = 1;
        Destroy (transform.parent.gameObject);
	}

    public void CloseCurrentSelf()
    {
        ChangePanelGroup.alpha = 1;
        StoryPanelGroup.alpha = 1;
        Destroy(gameObject);
    }

}
