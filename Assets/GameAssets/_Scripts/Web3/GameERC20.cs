using System.Numerics;
using System.Threading.Tasks;
using Nethereum.Contracts.Standards.ERC20.ContractDefinition;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using UnityEngine;

public class GameERC20
{
    private Web3 mWeb3;
    private Account mAccount;
    private string mContractAddress;
    private string mContractDeployAddress;

    public GameERC20(Web3 web3, Account account,string contractAddress,string deployAddress)
    {
        mWeb3 = web3;
        mAccount = account;
        mContractAddress = contractAddress;
        mContractDeployAddress = deployAddress;
    }
    
    public async Task ApproveToShop()
    {
        var fun = mWeb3.Eth.GetContractTransactionHandler<ApproveFunction>();
        var receipt = await fun.SendRequestAndWaitForReceiptAsync(mContractAddress, new ApproveFunction()
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
        var receipt = await fun.SendRequestAndWaitForReceiptAsync(mContractAddress, new MyMintFunction()
        {
            Owner = mAccount.Address,
            Value = Web3.Convert.ToWei(v)
        });
        Debug.LogError($"ClaimToken tx :{receipt.TransactionHash}");
    }

    public async Task<decimal> GetToken()
    {
        var balanceHandler = mWeb3.Eth.GetContractQueryHandler<MyBalanceOfFunction>();
        var balanceOf = await balanceHandler.QueryAsync<BigInteger>(mContractAddress, new MyBalanceOfFunction(){Owner = mAccount.Address});
        return Web3.Convert.FromWei(balanceOf);
    }
}