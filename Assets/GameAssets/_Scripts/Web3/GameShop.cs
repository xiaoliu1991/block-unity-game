using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using UnityEngine;

public class GameShop
{
    // private const string CONTRACT_ADDRESS = "0x504f06dcc84E5d0f5e95a3239f933A7E9a69c265";
    private const string CONTRACT_ADDRESS = "0x9fE46736679d2D9a65F0992F2272dE9f3c7fa6e0";//local
    
    // private const string MANAGER_ADDRESS = "0xe98a897299cea70f9b86d7800640E4b0568b1015";
    private const string MANAGER_ADDRESS = "0xf39Fd6e51aad88F6F4ce6aB8827279cffFb92266";//local
    
    private Web3 mWeb3;
    private Account mAccount;

    public GameShop(Web3 web3, Account account)
    {
        mWeb3 = web3;
        mAccount = account;
    }

    public string GetAddress()
    {
        return CONTRACT_ADDRESS;
    }
    
    public async Task<List<BigInteger>> GetGoods()
    {
        var fun = mWeb3.Eth.GetContractQueryHandler<ShopGetGoodsFunction>();
        var result = await fun.QueryAsync<List<BigInteger>>(CONTRACT_ADDRESS, new ShopGetGoodsFunction());
        Debug.LogError($"GetGoods :{string.Join(",",result)}");
        return result;
    }
    
    public async Task<int> GetPrice(int id)
    {
        var fun = mWeb3.Eth.GetContractQueryHandler<ShopGetPriceFunction>();
        var result = await fun.QueryAsync<BigInteger>(CONTRACT_ADDRESS, new ShopGetPriceFunction()
        {
            FromAddress = mAccount.Address,
            Id = id
        });
        Debug.LogError($"GetPrice {id} :{result}");
        return (int)Web3.Convert.FromWei(result);
    }
    
    
    public async Task SetPrice(int id,int price)
    {
        var fun = mWeb3.Eth.GetContractTransactionHandler<ShopSetPriceFunction>();
        var result = await fun.SendRequestAndWaitForReceiptAsync(CONTRACT_ADDRESS, new ShopSetPriceFunction()
        {
            FromAddress = MANAGER_ADDRESS,
            Id = id,
            Price = price
        });
        Debug.LogError($"SetPrice {id} , {price} : {result}");
    }


    public async Task buy(int id,int num)
    {
        Debug.LogError($"bug:{id}，{num}");
        await WebThreeManager.Instance.Erc20.ApproveToShop();
        await WebThreeManager.Instance.Erc1155.ApproveAllByShop();
        
        var fun = mWeb3.Eth.GetContractTransactionHandler<ShopBuyFunction>();
        var receipt = await fun.SendRequestAndWaitForReceiptAsync(CONTRACT_ADDRESS, new ShopBuyFunction()
        {
            FromAddress = mAccount.Address,
            Id = id,
            Amount = num
        });
        Debug.LogError($"buy tx :{receipt.TransactionHash}");
    }

}