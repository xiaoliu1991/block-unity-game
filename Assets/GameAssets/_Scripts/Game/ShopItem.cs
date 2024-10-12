using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    public int tokenId = 1;
    public Button button;
    public TMP_Text nameText;
    public TMP_Text stockText;
    public TMP_Text costText;

    public async void Initialize()
    {
        button.onClick.RemoveAllListeners();

        nameText.text = "GOLD";
        var stock = await WebThreeManager.Instance.Erc1155.GetStock(tokenId);
        stockText.text = stock.ToString();
        var price = await WebThreeManager.Instance.Shop.GetPrice(tokenId);
        costText.text = $"Cost ResToken : {price.ToString()}";

        button.onClick.AddListener(BuyItem);
    }

    private async void BuyItem()
    {
        try
        {
            UIManager.Instance.SetShopLog("Claiming item...");
            await WebThreeManager.Instance.Shop.buy(tokenId, 1);
            var stock = await WebThreeManager.Instance.Erc1155.GetStock(tokenId);
            stockText.text = stock.ToString();
            await GameManager.Instance.SyncResourceBalancesWithChain();
            UIManager.Instance.SetShopLog("Item claimed!");
        }
        catch (System.Exception e)
        {
            Debug.LogException(e);
            UIManager.Instance.SetShopLog("Error claiming item: " + e.Message);
        }
    }
}
