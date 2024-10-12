using System.Threading.Tasks;
using Nethereum.Web3;
using UnityEngine;
using Account = Nethereum.Web3.Accounts.Account;

public class WebThreeManager : Singleton<WebThreeManager>
{
    // private string Url = "https://sepolia.infura.io/v3/bb56b729ee2d4167b64c660f7c99529f";//测试网API
    private string Url = "http://127.0.0.1:8545";//测试网API
    
    // private const string CONTRACT_DEPLOY_ADDRESS = "0xe98a897299cea70f9b86d7800640E4b0568b1015";
    // private const string ERC20_CONTRACT_ADDRESS = "0xEb002390EAd3e88b77372D392A4dCa3ec3E58305";
    // private const string ERC1155_CONTRACT_ADDRESS = "0x4eCfD6643c46569032bCf720774940f0c95B034C";
    // private const string SHOP_CONTRACT_ADDRESS = "0x504f06dcc84E5d0f5e95a3239f933A7E9a69c265";
    
    //local
    private const string CONTRACT_DEPLOY_ADDRESS = "0xf39Fd6e51aad88F6F4ce6aB8827279cffFb92266";
    private const string ERC20_CONTRACT_ADDRESS = "0x5FbDB2315678afecb367f032d93F642f64180aa3";
    private const string ERC1155_CONTRACT_ADDRESS = "0xe7f1725E7734CE288F8367e1Bb143E90bb3F0512";
    private const string SHOP_CONTRACT_ADDRESS = "0x9fE46736679d2D9a65F0992F2272dE9f3c7fa6e0";
    
    private Web3 mWeb3;
    private Account mAccount;

    public GameERC20 Erc20;
    public GameERC1155 Erc1155;
    public GameShop Shop;
    public void Setup(Account account)
    {
        mAccount = account;
        mWeb3 = new Web3(mAccount, Url);
        
        Erc20 = new GameERC20(mWeb3,mAccount,ERC20_CONTRACT_ADDRESS,CONTRACT_DEPLOY_ADDRESS);
        Erc1155 = new GameERC1155(mWeb3,mAccount,ERC1155_CONTRACT_ADDRESS,CONTRACT_DEPLOY_ADDRESS);
        Shop = new GameShop(mWeb3, mAccount,SHOP_CONTRACT_ADDRESS,CONTRACT_DEPLOY_ADDRESS);
    }
   
    
    private async Task CheckBlockNumberPeriodically()
    {
        var blockNumber = await mWeb3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
        Debug.LogError($"Chain blockNumber : {blockNumber}");
    }
}
