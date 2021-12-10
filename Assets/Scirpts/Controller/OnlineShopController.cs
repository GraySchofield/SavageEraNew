using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

using Soomla.Store;
using Soomla;

public class OnlineShopController : MonoBehaviour {
    private AsyncOperation async;
    public Slider LoadingBar;
    public GameObject loadingScreen;
    public List<GameObject> Toasts;
    private List<string> current_toasts;
    public Text CoinCountText;
    public GameObject InventoryContent;

    public Transform ShopDialogPrefab;
    public Transform Canvas;
    public AudioSource Bought;
    public AudioSource Fail;

	private Reward firstLaunchReward;


    public GameObject BuyingPannel;


    void Start()
    {
        current_toasts = new List<string>();
        InvokeRepeating("ReadToast", 0, 0.1f);

        if (!SoomlaStore.Initialized)
        {
            StoreEvents.OnSoomlaStoreInitialized += onSoomlaStoreInitialized;
            StoreEvents.OnCurrencyBalanceChanged += onCurrencyBalanceChanged;
            StoreEvents.OnUnexpectedStoreError += onUnexpectedStoreError;
            StoreEvents.OnMarketPurchase += onMarketPurchase;

            firstLaunchReward = new VirtualItemReward("first-launch", "Give Money at first launch", IAPAssets.SURVIVAL_CURRENCY_ITEM_ID, 5);
            SoomlaStore.Initialize(new IAPAssets());
        }
    }    

	public void onUnexpectedStoreError(int errorCode) {
		SoomlaUtils.LogError ("IAPEventHandler", "error with code: " + errorCode);
	}

	public void onSoomlaStoreInitialized() {

		// some usage examples for add/ currency
		// some examples
		if (StoreInfo.Currencies.Count>0) {
			try {
				//First launch reward
				if(!firstLaunchReward.Owned)
				{
					firstLaunchReward.Give();
				}

				//How to give currency
				SoomlaUtils.LogDebug("SOOMLA IAPEventHandler", "Currency balance:" + StoreInventory.GetItemBalance(StoreInfo.Currencies[0].ItemId));
			} catch (VirtualItemNotFoundException ex){
				SoomlaUtils.LogError("removeSOOMLA IAPWindow", ex.Message);
			}
		}
	}

	public void onCurrencyBalanceChanged(VirtualCurrency virtualCurrency, int balance, int amountAdded) {
	}

    public void onMarketPurchase(PurchasableVirtualItem pvi, string payload, Dictionary<string, string> extra)
    {
        // pvi is the PurchasableVirtualItem that was just purchased
        // payload is a text that you can give when you initiate the purchase operation and you want to receive back upon completion
        // extra will contain platform specific information about the market purchase.
        //      Android: The "extra" dictionary will contain "orderId" and "purchaseToken".
        //      iOS: The "extra" dictionary will contain "receipt" and "token".
        
        addToast(Lang.Current["purchase_completed"] + Lang.Current[pvi.ItemId]);
    }


    void Update()
    {
		CoinCountText.text = "" + StoreInventory.GetItemBalance(StoreInfo.Currencies[0].ItemId);
    }

    public void OpenBuyingPannel()
    {
        BuyingPannel.SetActive(true);
    }

    public void CloseBuyingPannel()
    {
        BuyingPannel.SetActive(false);
    }
   



	public void Buy5Coins(){
		StoreInventory.BuyItem ("coins_5");
	}


    public void Buy30Coins()
    {
		StoreInventory.BuyItem("coins_30");

    }

    public void Buy100Coins()
    {
		StoreInventory.BuyItem("coins_100");
    }



    public void BuyProduct(ShopButton shop_button)
    {
        Transform clone = Instantiate(ShopDialogPrefab) as Transform;
        clone.SetParent(Canvas);
        clone.SetAsLastSibling();
        clone.localScale = new Vector3(1f, 1f, 1f);
        RectTransform rf = clone.GetComponent<RectTransform>();
        rf.offsetMin = new Vector2(0f, 0f);
        rf.offsetMax = new Vector2(0f, 0f);
        Transform detail = clone.FindChild("Detail");
        Text des = detail.FindChild("Description").GetComponent<Text>();
        Button confirm = detail.FindChild("Confirm").GetComponent<Button>();
        confirm.onClick.AddListener(delegate {
            BuyCountableProduct(shop_button);
            Destroy(clone.gameObject);
        });

        string content = "";
        content += Lang.Current[shop_button.ProductType] + "*"
                + shop_button.currentItemCount + "\n";
        content += Lang.Current["total_cost"] + ":" + shop_button.GetTotalPrice() + 
            Lang.Current["coin"];
        des.text = content;

    }


    public void BuyCountableProduct(ShopButton shop_button)
    {
        if (Shop.Current.CoinCount < shop_button.GetTotalPrice())
        {
            addToast(Lang.Current["not_enough_money"]);
            Fail.Play();
        }
        else
        {
			StoreInventory.TakeItem (StoreInfo.Currencies[0].ItemId, shop_button.GetTotalPrice());	
            Shop.Current.AddProduct(shop_button.ProductType, shop_button.currentItemCount);
            addToast(Lang.Current["gain"] + Lang.Current[shop_button.ProductType] + "*"
                + shop_button.currentItemCount );
            Bought.Play();
            shop_button.currentItemCount = 1;

        }

    }


    private void addToast(string content)
    {
        this.current_toasts.Add(content);
    }

    public void goBackHome()
    {
        loadingScreen.SetActive(true);
        StartCoroutine(loadLevelInBG(1));
    }

    IEnumerator loadLevelInBG(int level)
    {
        async = SceneManager.LoadSceneAsync(level, LoadSceneMode.Single);

        while (!async.isDone)
        {
            LoadingBar.value = async.progress;
            yield return null;
        }
    }


    private void ReadToast()
    {
        if (current_toasts.Count > 0)
        {
            for (int i = 0; i < Toasts.Count; i++)
            {
                GameObject toast = Toasts[i];
                if (!toast.activeSelf)
                {
                    string toast_content = current_toasts[0];
                    //  toast.GetComponent<Text>().text = toast_content;
                    toast.SetActive(true);
                    toast.GetComponent<Text>().text = toast_content;
                    current_toasts.RemoveAt(0);
                    return;
                }
            }
        }
    }

    public void RenderInventory()
    {
        List<StackableItem> products = Shop.Current.AllProducts();
        foreach (Transform childTransform in InventoryContent.transform) Destroy(childTransform.gameObject);
        for (int i = 0; i < products.Count; i++)
        {
            //add the items to the view 
            StackableItem pro = products[i];
            //add new row
            new ShopInventoryRowView(InventoryContent, pro, i);
          //  ShopInventoryRowView Row = new ShopInventoryRowView(InventoryContent, pro, i);
    
        }
    }


}
