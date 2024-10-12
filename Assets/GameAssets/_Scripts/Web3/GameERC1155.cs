using System.Numerics;
using System.Threading.Tasks;
using Nethereum.Contracts.Standards.ERC1155;
using Nethereum.Contracts.Standards.ERC1155.ContractDefinition;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using UnityEngine;

public class GameERC1155
{
    private Web3 mWeb3;
    private Account mAccount;
    private string mContractAddress;
    private string mContractDeployAddress;

    public GameERC1155(Web3 web3, Account account,string contractAddress,string deployAddress)
    {
        mWeb3 = web3;
        mAccount = account;
        mContractAddress = contractAddress;
        mContractDeployAddress = deployAddress;
    }

    public async Task<int> GetStock(int id)
    {
        var balanceHandler = mWeb3.Eth.GetContractQueryHandler<BalanceOfFunction>();
        var balanceOf = await balanceHandler.QueryAsync<int>(mContractAddress, new BalanceOfFunction(){Account = mContractDeployAddress,Id = id});
        return balanceOf;
    }
    
    
    public async Task<int> GetCount(int id)
    {
        var balanceHandler = mWeb3.Eth.GetContractQueryHandler<BalanceOfFunction>();
        var balanceOf = await balanceHandler.QueryAsync<int>(mContractAddress, new BalanceOfFunction(){Account = mAccount.Address,Id = id});
        return balanceOf;
    }
    
    public async Task ApproveAllWithDeveloperByShop()
    {
        var account = new Account("0xac0974bec39a17e36ba4a6b4d238ff944bacb478cbed5efcae784d7bf4f2ff80");
        var web3 = new Web3();
        var storeAddress = WebThreeManager.Instance.Shop.GetAddress();
        var fun = web3.Eth.GetContractTransactionHandler<SetApprovalForAllFunction>();
        var receipt = await fun.SendRequestAndWaitForReceiptAsync(mContractAddress, new SetApprovalForAllFunction()
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
        var receipt = await fun.SendRequestAndWaitForReceiptAsync(mContractAddress, new SetApprovalForAllFunction()
        {
            FromAddress = mAccount.Address,
            Operator = storeAddress,
            Approved = true
        });
        Debug.LogError($"ApproveAll tx :{receipt.TransactionHash}");
    }
}