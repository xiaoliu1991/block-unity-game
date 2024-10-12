using System.Numerics;
using System.Threading.Tasks;
using Nethereum.Contracts.Standards.ERC1155;
using Nethereum.Contracts.Standards.ERC1155.ContractDefinition;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using UnityEngine;

public class GameERC1155
{
    // private const string CONTRACT_ADDRESS = "0x4eCfD6643c46569032bCf720774940f0c95B034C";
    private const string CONTRACT_ADDRESS = "0xe7f1725E7734CE288F8367e1Bb143E90bb3F0512";//local
    
    // private const string MANAGER_ADDRESS = "0xe98a897299cea70f9b86d7800640E4b0568b1015";
    private const string MANAGER_ADDRESS = "0xf39Fd6e51aad88F6F4ce6aB8827279cffFb92266";//local
    
    private Web3 mWeb3;
    private Account mAccount;

    public GameERC1155(Web3 web3, Account account)
    {
        mWeb3 = web3;
        mAccount = account;
    }

    public async Task<int> GetStock(int id)
    {
        var balanceHandler = mWeb3.Eth.GetContractQueryHandler<BalanceOfFunction>();
        var balanceOf = await balanceHandler.QueryAsync<int>(CONTRACT_ADDRESS, new BalanceOfFunction(){Account = MANAGER_ADDRESS,Id = id});
        return balanceOf;
    }
    
    
    public async Task<int> GetCount(int id)
    {
        var balanceHandler = mWeb3.Eth.GetContractQueryHandler<BalanceOfFunction>();
        var balanceOf = await balanceHandler.QueryAsync<int>(CONTRACT_ADDRESS, new BalanceOfFunction(){Account = mAccount.Address,Id = id});
        return balanceOf;
    }
    
    public async Task ApproveAllWithDeveloperByShop()
    {
        var account = new Account("0xac0974bec39a17e36ba4a6b4d238ff944bacb478cbed5efcae784d7bf4f2ff80");
        var web3 = new Web3();
        var storeAddress = WebThreeManager.Instance.Shop.GetAddress();
        var fun = web3.Eth.GetContractTransactionHandler<SetApprovalForAllFunction>();
        var receipt = await fun.SendRequestAndWaitForReceiptAsync(CONTRACT_ADDRESS, new SetApprovalForAllFunction()
        {
            FromAddress = account.Address,
            Operator = storeAddress,
            Approved = true
        });
        Debug.LogError($"ApproveAll tx :{receipt.TransactionHash}");
    }
    
    public async Task ApproveAllByShop()
    {
        var storeAddress = WebThreeManager.Instance.Shop.GetAddress();
        var fun = mWeb3.Eth.GetContractTransactionHandler<SetApprovalForAllFunction>();
        var receipt = await fun.SendRequestAndWaitForReceiptAsync(CONTRACT_ADDRESS, new SetApprovalForAllFunction()
        {
            FromAddress = mAccount.Address,
            Operator = storeAddress,
            Approved = true
        });
        Debug.LogError($"ApproveAll tx :{receipt.TransactionHash}");
    }
}