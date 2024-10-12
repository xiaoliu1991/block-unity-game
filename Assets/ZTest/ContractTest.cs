using System;
using System.Threading.Tasks;
using Nethereum.Contracts.Standards.ERC20.ContractDefinition;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using UnityEngine;
using Account = Nethereum.Web3.Accounts.Account;
using TransferFunction = Nethereum.Contracts.Standards.ERC20.ContractDefinition.TransferFunction;

public class ContractTest : MonoBehaviour
{
    void Start()
    {
        CallFun2().ConfigureAwait(false);
    }

    private async Task CallFun2()
    {
        try
        {
            //本地测试
            bool isLocal = false;
            string url = isLocal ? "http://127.0.0.1:8545" :"https://sepolia.infura.io/v3/bb56b729ee2d4167b64c660f7c99529f";
            Web3 web3 = isLocal ? new Web3(url) : new Web3(new Account("562c5b7417eca5b6b95032c7faa758b39746aaf23018225ffc250d25142089eb"),url);
            string contractAddress = isLocal ? "0xa513E6E4b8f2a923D98304ec87F64353C4D5C853" : "0xd72984FbE2836D82b2A917c862eAC20ACE41FB08";
            string addr1 = isLocal ? "0xf39Fd6e51aad88F6F4ce6aB8827279cffFb92266" : "0xe98a897299cea70f9b86d7800640E4b0568b1015";
            string addr2 = isLocal ? "0x70997970C51812dc3A010C7d01b50e0d17dc79C8" : "0x19AaB55F22AF0FE7D6B509AeE400B7C26908CaE8";
            string addr3 = isLocal ? "0x3C44CdDdB6a900fa2b585dd299e03d12FA4293BC" : "0xfD1c11ce1072e32A61eba6E6DA34AD8943eEa6F3";
            var erc20Service = web3.Eth.ERC20.GetContractService(contractAddress);
            var cDecimal = await erc20Service.DecimalsQueryAsync();
            var totalSupply = await erc20Service.TotalSupplyQueryAsync();
            Debug.LogError($"TotalSupply: {totalSupply}");
            Debug.LogError($"Addr1 BalanceOf  : {await erc20Service.BalanceOfQueryAsync(addr1)}");
            Debug.LogError($"Addr2 BalanceOf  : {await erc20Service.BalanceOfQueryAsync(addr2)}");
            Debug.LogError($"Addr3 BalanceOf  : {await erc20Service.BalanceOfQueryAsync(addr3)}");
            // var amount = new BigInteger(50 * Math.Pow(10, cDecimal));
            // Debug.LogError($"Addr1 -> Addr2 : {amount}");
            var tx = await erc20Service.TransferRequestAsync(new TransferFunction()
            {
                FromAddress = addr1,
                To = addr2,
                Value = new HexBigInteger(1),
            });
            Debug.LogError($"Transaction hash: {tx}");
            // Debug.LogError($"Transaction hash: {await service.TransferRequestAsync(addr2, new HexBigInteger(amount))}");
            Debug.LogError($"Addr1 BalanceOf  : {await erc20Service.BalanceOfQueryAsync(addr1)}");
            Debug.LogError($"Addr2 BalanceOf  : {await erc20Service.BalanceOfQueryAsync(addr2)}");
            var approveFunction = new ApproveFunction()
            {
                FromAddress = addr2,
                Spender = addr1,
                Value = 10000
            };
            Debug.LogError($"addr2 -> addr1 Approve {await erc20Service.ApproveRequestAndWaitForReceiptAsync(approveFunction)}");
            // Debug.LogError($"addr2 -> addr1 Allowance：{await erc20Service.AllowanceQueryAsync(addr2,addr1)}");
            // var transferFromFunction = new TransferFromFunction()
            // {
            //     FromAddress = addr1,
            //     From = addr2,
            //     To = addr3,
            //     Value = 10
            // };
            // Debug.LogError($"add2 -> add3 transferFrom: {await erc20Service.TransferFromRequestAndWaitForReceiptAsync(transferFromFunction)}");
            // // Debug.LogError($"add2 -> add3 transferFrom: {await service.TransferFromRequestAndWaitForReceiptAsync(addr2,addr3,new BigInteger(1))}");
            // Debug.LogError($"add2 BalanceOf  : {await erc20Service.BalanceOfQueryAsync(addr2)}");
            // Debug.LogError($"add3 BalanceOf  : {await erc20Service.BalanceOfQueryAsync(addr3)}");

            //测试网
            // Web3 web3 = new Web3("https://sepolia.infura.io/v3/bb56b729ee2d4167b64c660f7c99529f");
            // var service = web3.Eth.ERC20.GetContractService("0x679b616cDc9D4701e5224E16B53A7c63ce77C522");
            // Debug.LogError($"TotalSupply: {await service.TotalSupplyQueryAsync()}");
            // Debug.LogError($"BalanceOf  : {await service.BalanceOfQueryAsync("0xe98a897299cea70f9b86d7800640E4b0568b1015")}");
            // Debug.LogError($"BalanceOf  : {await service.BalanceOfQueryAsync("0x19AaB55F22AF0FE7D6B509AeE400B7C26908CaE8")}");
            // Debug.LogError(await service.TransferRequestAndWaitForReceiptAsync("0x19AaB55F22AF0FE7D6B509AeE400B7C26908CaE8", new BigInteger(Math.Pow(10,18))));
            // Debug.LogError($"BalanceOf  : {await service.BalanceOfQueryAsync("0xe98a897299cea70f9b86d7800640E4b0568b1015")}");
            // Debug.LogError($"BalanceOf  : {await service.BalanceOfQueryAsync("0x19AaB55F22AF0FE7D6B509AeE400B7C26908CaE8")}");

            // string address = "0xe98a897299cea70f9b86d7800640E4b0568b1015";
            // string abi = File.ReadAllText($"{Application.dataPath}/GameAssets/_ABI/GameGems.abi");
            // var contract = web3.Eth.GetContract(abi,"0x679b616cDc9D4701e5224E16B53A7c63ce77C522");
            // Debug.LogError($"totalSupply: {await contract.GetFunction("totalSupply").CallAsync<BigInteger>()}");
            // var transferFunction = contract.GetFunction("transfer");
            // var transactionReceipt = await transferFunction.SendTransactionAsync(address,"0x19AaB55F22AF0FE7D6B509AeE400B7C26908CaE8", BigInteger.Parse("1000000000000000000"));
            // Debug.Log(transactionReceipt);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
       
        // var result = mContract.GetFunction("totalSupply").CallAsync<int>();
        // await result;
        // Debug.LogError($"totalSupply: {result.Result}");
        //
        // result = mContract.GetFunction("balanceOf").CallAsync<int>("0xe98a897299cea70f9b86d7800640E4b0568b1015");
        // await result;
        // Debug.LogError($"balanceOf: {result.Result}");
        //
        // var resultB = mContract.GetFunction("approve").CallAsync<bool>(new object[]
        // {
        //     "0xe98a897299cea70f9b86d7800640E4b0568b1015",1000
        // });
        // await resultB;
        // Debug.LogError($"result: {resultB.Result}");
        //
        // // var resultB = mContract.GetFunction("transfer").CallAsync<bool>("0x19AaB55F22AF0FE7D6B509AeE400B7C26908CaE8",100);
        // // await resultB;
        // // Debug.LogError($"result: {resultB.Result}");
        //
        // result = mContract.GetFunction("allowance").CallAsync<int>(new object[]{"0xe98a897299cea70f9b86d7800640E4b0568b1015","0xe98a897299cea70f9b86d7800640E4b0568b1015"});
        // await result;
        // Debug.LogError($"result: {result.Result}");
    }
    
    // Code Comments for account creation and balance checks are available here:
    // https://gist.github.com/e11io/88f0ae5831f3aa31651f735278b5b463
    public void CreateAccount(string password, Action<string, string> callback)
    {
        var ecKey = Nethereum.Signer.EthECKey.GenerateKey();
        var address = ecKey.GetPublicAddress();
        var privateKey = ecKey.GetPrivateKeyAsBytes();

        var addr = Nethereum.Signer.EthECKey.GetPublicAddress(privateKey.ToString());
        var keystoreservice = new Nethereum.KeyStore.KeyStoreService();
        var encryptedJson = keystoreservice.EncryptAndGenerateDefaultKeyStoreAsJson(password, privateKey, address);
        
        callback(address, encryptedJson);
    }
}
