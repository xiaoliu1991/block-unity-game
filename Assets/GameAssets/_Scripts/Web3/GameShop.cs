using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using UnityEngine;

public class GameShop
{
    private Web3 mWeb3;
    private Account mAccount;
    private string mContractAddress;
    private string mContractDeployAddress;
    
    public GameShop(Web3 web3, Account account,string contractAddress,string deployAddress)
    {
        mWeb3 = web3;
        mAccount = account;
        mContractAddress = contractAddress;
        mContractDeployAddress = deployAddress;
    }

    public string GetAddress()
    {
        return mContractAddress;
    }
    
    public async Task<List<BigInteger>> GetGoods()
    {
        var fun = mWeb3.Eth.GetContractQueryHandler<ShopGetGoodsFunction>();
        var result = await fun.QueryAsync<List<BigInteger>>(mContractAddress, new ShopGetGoodsFunction());
        Debug.LogError($"GetGoods :{string.Join(",",result)}");
        return result;
    }
    
    public async Task<int> GetPrice(int id)
    {
        var fun = mWeb3.Eth.GetContractQueryHandler<ShopGetPriceFunction>();
        var result = await fun.QueryAsync<BigInteger>(mContractAddress, new ShopGetPriceFunction()
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
        var result = await fun.SendRequestAndWaitForReceiptAsync(mContractAddress, new ShopSetPriceFunction()
        {
            FromAddress = mContractDeployAddress,
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
        var receipt = await fun.SendRequestAndWaitForReceiptAsync(mContractAddress, new ShopBuyFunction()
        {
            FromAddress = mAccount.Address,
            Id = id,
            Amount = num
        });
        Debug.LogError($"buy tx :{receipt.TransactionHash}");
    }

}