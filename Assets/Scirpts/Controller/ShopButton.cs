using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour {
    public string ProductType; // the type
    public int unitPrice = 1; // the unit price for one piece of item

    [HideInInspector]
    public int currentItemCount = 1; //how much you need to buy


    private Text require_text_view;
    private AudioSource click_weak;
	// Use this for initialization
	void Start () {
        click_weak = GameObject.Find("ClickWeak").GetComponent<AudioSource>();
        require_text_view = transform.FindChild("WorkRequires").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        require_text_view.text = GetTotalPrice() + Lang.Current["coin"] + " " + currentItemCount + Lang.Current["ge"];
    }


    //return the total price needed to buy the desired amount
    public int GetTotalPrice()
    {
        return (currentItemCount * unitPrice);
    }



    public void AddingCount()
    {
        InvokeRepeating("AddCount",0,0.2f);
    }

    public void StopAddingCount()
    {
        CancelInvoke("AddCount");
    }

    public void MinusingCount()
    {
        InvokeRepeating("MinusCount" ,0 ,0.2f);
    }

    public void StopMinusingCount()
    {
        CancelInvoke("MinusCount");
    }


    public void AddCount()
    {
        click_weak.Play();
        this.currentItemCount ++;
    }

    public void MinusCount()
    {
        click_weak.Play();
        if (this.currentItemCount >= 2)
        {
            this.currentItemCount--;
        }
    }

}
