
using System;
using System.Numerics;
using System.Threading.Tasks;
using Nethereum.Contracts.Standards.ERC20.ContractDefinition;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using UnityEngine;
using TransferFunction = Nethereum.Contracts.Standards.ERC20.ContractDefinition.TransferFunction;

public class GameERC20Test
{
    public async Task CallTransfer()
    {
        // Sepolia 测试网的节点 URL
        string rpcUrl = "https://sepolia.infura.io/v3/bb56b729ee2d4167b64c660f7c99529f";
        // ERC20 合约地址
        var contractAddress = "0xd72984FbE2836D82b2A917c862eAC20ACE41FB08";
        // 创建 ERC20 合约实例
        var fromAddress = "0xe98a897299cea70f9b86d7800640E4b0568b1015";
        // 转账的接收地址
        var toAddress = "0x19AaB55F22AF0FE7D6B509AeE400B7C26908CaE8";
        
        // rpcUrl = "http://127.0.0.1:8545";
        // contractAddress = "0xe7f1725E7734CE288F8367e1Bb143E90bb3F0512";
        // fromAddress = "0xf39Fd6e51aad88F6F4ce6aB8827279cffFb92266";
        // toAddress = "0x70997970C51812dc3A010C7d01b50e0d17dc79C8";
        
        // var account = new Account("562c5b7417eca5b6b95032c7faa758b39746aaf23018225ffc250d25142089eb");//c727e327892f89a057c0fc57f68a2c1f09c3da983d2d04b5adb7792cb82b0d82
        var account = AccountCreator.CreateAccount("xiaoliu", "123456");
        toAddress = fromAddress;
        fromAddress = account.Address;
        
        // 创建 Web3 实例
        var web3 = new Web3(account,  rpcUrl);
        try
        {
            // var totalSupply = web3.Eth.GetContractQueryHandler<TotalSupplyFunction>();
            // var balanceHandler = web3.Eth.GetContractQueryHandler<MyBalanceOfFunction>();
            // Debug.LogError($"TotalSupply  : {Web3.Convert.FromWei(await totalSupply.QueryAsync<BigInteger>(contractAddress, new TotalSupplyFunction()))}");
            // Debug.LogError($"Addr1 BalanceOf  : {Web3.Convert.FromWei(await balanceHandler.QueryAsync<BigInteger>(contractAddress, new MyBalanceOfFunction(){Owner = account.Address}))}");
            //
            // var fun = web3.Eth.GetContractTransactionHandler<MyMintFunction>();
            // var receipt = await fun.SendRequestAndWaitForReceiptAsync(contractAddress, new MyMintFunction()
            // {
            //     Owner = account.Address,
            //     Value = 1
            // });
            // Debug.LogError($"ClaimGems tx :{receipt.TransactionHash}");
            //
            // Debug.LogError($"TotalSupply  : {Web3.Convert.FromWei(await totalSupply.QueryAsync<BigInteger>(contractAddress, new TotalSupplyFunction()))}");
            // Debug.LogError($"Addr1 BalanceOf  : {Web3.Convert.FromWei(await balanceHandler.QueryAsync<BigInteger>(contractAddress, new MyBalanceOfFunction(){Owner = account.Address}))}");

            var totalSupply = web3.Eth.GetContractQueryHandler<TotalSupplyFunction>();
            var balanceHandler = web3.Eth.GetContractQueryHandler<MyBalanceOfFunction>();
            var decimalsFunction = web3.Eth.GetContractQueryHandler<DecimalsFunction>();
            Debug.LogError($"TotalSupply  : {await totalSupply.QueryAsync<BigInteger>(contractAddress, new TotalSupplyFunction())}");
            Debug.LogError($"Addr1 BalanceOf  : {await balanceHandler.QueryAsync<BigInteger>(contractAddress, new MyBalanceOfFunction(){Owner = fromAddress})}");
            Debug.LogError($"Addr2 BalanceOf  : {await balanceHandler.QueryAsync<BigInteger>(contractAddress, new MyBalanceOfFunction(){Owner = toAddress})}");
            var decimals = await decimalsFunction.QueryAsync<BigInteger>(contractAddress, new DecimalsFunction());
            Debug.LogError($"Decimals  : {decimals}");
            
            // 创建交易请求
            var transferHandler = web3.Eth.GetContractTransactionHandler<TransferFunction>();
            var transactionReceipt = await transferHandler.SendRequestAndWaitForReceiptAsync(contractAddress, new TransferFunction(){FromAddress = fromAddress,To = toAddress,Value = Web3.Convert.ToWei(1)});
            Debug.LogError($"tx :{transactionReceipt.TransactionHash}");
            
            Debug.LogError($"Addr1 BalanceOf  : {await balanceHandler.QueryAsync<BigInteger>(contractAddress, new MyBalanceOfFunction(){Owner = fromAddress})}");
            Debug.LogError($"Addr2 BalanceOf  : {await balanceHandler.QueryAsync<BigInteger>(contractAddress, new MyBalanceOfFunction(){Owner = toAddress})}");
            
            
            // var fun = web3.Eth.GetContractTransactionHandler<ApproveFunction>();
            // var receipt = await fun.SendRequestAndWaitForReceiptAsync(contractAddress, new ApproveFunction()
            // {
            //     FromAddress = account.Address,
            //     Spender = fromAddress,
            //     Value = Web3.Convert.ToWei(10)
            // });
            // Debug.LogError($"tx :{receipt.TransactionHash}");
            
            // var fun1 = web3.Eth.GetContractTransactionHandler<TransferFromFunction>();
            // var receipt1 = await fun1.SendRequestAndWaitForReceiptAsync(contractAddress, new TransferFromFunction()
            // {
            //     From = fromAddress,
            //     FromAddress = account.Address,
            //     To = fromAddress,
            //     Value = Web3.Convert.ToWei(2)
            // });
            // Debug.LogError($"tx1 :{receipt1.TransactionHash}");
            
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
    
    
    public void CreateAccount(string password, Action<string, string> callback)
    {
        try
        {
            var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
            var address = ecKey.GetPublicAddress();
            var privateKeyByes = ecKey.GetPrivateKeyAsBytes();
            var privateKey = privateKeyByes.ToHex();
            var addr = Nethereum.Signer.EthECKey.GetPublicAddress(privateKey);
            var keystoreservice = new Nethereum.KeyStore.KeyStoreService();
            var encryptedJson = keystoreservice.EncryptAndGenerateDefaultKeyStoreAsJson(password, privateKeyByes, address);
        
            Debug.LogError($"address:{address}");
            Debug.LogError($"privateKey:{privateKey}");
            Debug.LogError($"address:{addr}");
            Debug.LogError($"encryptedJson:{encryptedJson}");
            callback(address, encryptedJson);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }
}
