using System.Threading.Tasks;
using Nethereum.Web3;
using UnityEngine;
using Account = Nethereum.Web3.Accounts.Account;

public class WebThreeManager : Singleton<WebThreeManager>
{
    // private string Url = "https://sepolia.infura.io/v3/bb56b729ee2d4167b64c660f7c99529f";//测试网API
    private string Url = "http://127.0.0.1:8545";//测试网API
    
    private Web3 mWeb3;
    private Account mAccount;

    public GameERC20 Erc20;
    public GameERC1155 Erc1155;
    public GameShop Shop;
    public void Setup(Account account)
    {
        mAccount = account;
        mWeb3 = new Web3(mAccount, Url);
        
        Erc20 = new GameERC20(mWeb3,mAccount);
        Erc1155 = new GameERC1155(mWeb3,mAccount);
        Shop = new GameShop(mWeb3, mAccount);
    }
   
    
    private async Task CheckBlockNumberPeriodically()
    {
        var blockNumber = await mWeb3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
        Debug.LogError($"Chain blockNumber : {blockNumber}");
    }
}
