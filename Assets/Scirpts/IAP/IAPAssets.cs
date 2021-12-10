using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Soomla.Store;

/// <summary>
/// This class defines our game's economy, which includes virtual goods, virtual currencies
/// and currency packs, virtual categories
/// </summary>
public class IAPAssets : IStoreAssets{

	/// <summary>
	/// see parent.
	/// </summary>
	public int GetVersion() {
		return 0;
	}

	/// <summary>
	/// see parent.
	/// </summary>
	public VirtualCurrency[] GetCurrencies() {
		return new VirtualCurrency[]{SURVIVAL_CURRENCY};
	}

	/// <summary>
	/// see parent.
	/// </summary>
	public VirtualCurrencyPack[] GetCurrencyPacks() {
		return new VirtualCurrencyPack[] { FIVECOIN_PACK, THIRTYCOIN_PACK, HUNDREDCOIN_PACK };
	}

	/// <summary>
	/// see parent.
	/// </summary>
	public VirtualGood[] GetGoods() {
		return new VirtualGood[] {};
	}
		
	/// <summary>
	/// see parent.
	/// </summary>
	public VirtualCategory[] GetCategories() {
		return new VirtualCategory[]{};
	}

	/** Static Final Members **/

	public const string SURVIVAL_CURRENCY_ITEM_ID = "currency_survival";

	public const string FIVECOIN_PACK_PRODUCT_ID = "com.thegsstorm.wildhunter.coins.5";

	public const string THIRTYCOIN_PACK_PRODUCT_ID = "com.thegsstorm.wildhunter.coins.30";

	public const string HUNDREDCOIN_PACK_PRODUCT_ID = "com.thegsstorm.wildhunter.coins.100";

	/** Virtual Currencies **/

	public static VirtualCurrency SURVIVAL_CURRENCY = new VirtualCurrency(
		"Coins",										// name
		"",												// description
		SURVIVAL_CURRENCY_ITEM_ID						// item id
	);


	/** Virtual Currency Packs **/

	public static VirtualCurrencyPack FIVECOIN_PACK = new VirtualCurrencyPack(
		"5 Coins",                                   // name
		"",                                           // description
		"coins_5",                                   // item id
		5,											  // number of currencies in the pack
		SURVIVAL_CURRENCY_ITEM_ID,                    // the currency associated with this pack
		new PurchaseWithMarket(FIVECOIN_PACK_PRODUCT_ID, 0.99)
	);

	public static VirtualCurrencyPack THIRTYCOIN_PACK = new VirtualCurrencyPack(
		"30 Coins",                                   // name
		"",                                           // description
		"coins_30",                                   // item id
		30,                                           // number of currencies in the pack
		SURVIVAL_CURRENCY_ITEM_ID,                    // the currency associated with this pack
		new PurchaseWithMarket(THIRTYCOIN_PACK_PRODUCT_ID, 3.99)
	);

	public static VirtualCurrencyPack HUNDREDCOIN_PACK = new VirtualCurrencyPack(
		"100 Coins",                                  // name
		"",                 	// description
		"coins_100",                                  // item id
		100,                                            // number of currencies in the pack
		SURVIVAL_CURRENCY_ITEM_ID,                        // the currency associated with this pack
		new PurchaseWithMarket(HUNDREDCOIN_PACK_PRODUCT_ID, 9.99)
	);
}
	
