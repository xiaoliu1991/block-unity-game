using System.Numerics;
using System.Threading.Tasks;
using Nethereum.Contracts.Standards.ERC20.ContractDefinition;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using UnityEngine;

public class GameERC20
{
    // private const string CONTRACT_ADDRESS = "0xEb002390EAd3e88b77372D392A4dCa3ec3E58305";
    private const string CONTRACT_ADDRESS = "0x5FbDB2315678afecb367f032d93F642f64180aa3";//local
    
    // private const string MANAGER_ADDRESS = "0xe98a897299cea70f9b86d7800640E4b0568b1015";
    private const string MANAGER_ADDRESS = "0xf39Fd6e51aad88F6F4ce6aB8827279cffFb92266";//local
    
    private Web3 mWeb3;
    private Account mAccount;

    public GameERC20(Web3 web3, Account account)
    {
        mWeb3 = web3;
        mAccount = account;
    }
    
    public async Task ApproveToShop()
    {
        var fun = mWeb3.Eth.GetContractTransactionHandler<ApproveFunction>();
        var receipt = await fun.SendRequestAndWaitForReceiptAsync(CONTRACT_ADDRESS, new ApproveFunction()
        {
            FromAddress = mAccount.Address,
            Spender = WebThreeManager.Instance.Shop.GetAddress(),
            Value = Web3.Convert.ToWei(10000)
        });
        Debug.LogError($"Approve tx :{receipt.TransactionHash}");
    }

    public async Task ClaimToken(int v)
    {
        var fun = mWeb3.Eth.GetContractTransactionHandler<MyMintFunction>();
        var receipt = await fun.SendRequestAndWaitForReceiptAsync(CONTRACT_ADDRESS, new MyMintFunction()
        {
            Owner = mAccount.Address,
            Value = Web3.Convert.ToWei(v)
        });
        Debug.LogError($"ClaimToken tx :{receipt.TransactionHash}");
    }

    public async Task<decimal> GetToken()
    {
        var balanceHandler = mWeb3.Eth.GetContractQueryHandler<MyBalanceOfFunction>();
        var balanceOf = await balanceHandler.QueryAsync<BigInteger>(CONTRACT_ADDRESS, new MyBalanceOfFunction(){Owner = mAccount.Address});
        return Web3.Convert.FromWei(balanceOf);
    }
}