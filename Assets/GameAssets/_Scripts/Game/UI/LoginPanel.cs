using System;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.Web3;
using Org.BouncyCastle.Math;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using WalletConnect.Web3Modal;
using WalletConnectSharp.Core;
using WalletConnectSharp.Sign;
using WalletConnectSharp.Sign.Models;
using WalletConnectUnity.Core;

public class LoginPanel : MonoBehaviour
{
    public Button Login;
    public Button LogOut;

    private void Awake()
    {
        Login.gameObject.SetActive(false);
        LogOut.gameObject.SetActive(false);
        Login.onClick.AddListener(OnLoginClick);
        LogOut.onClick.AddListener(OnLogOutClick);
    }
    
    
    public async void Start()
    {
        OnInit();
    }

    private async void OnInit()
    {
        var sepolia = new Chain(
            chainNamespace: "eip155", 
            chainReference: "11155111", // Sepolia's chain ID
            name: "Ethereum Sepolia Testnet",
            viemName: "sepolia",
            nativeCurrency: new Currency
            (
                "Ethereum",
                "ETH",
                18
            ),
            blockExplorer: new BlockExplorer
                ("Etherscan", "https://sepolia.etherscan.io"),
            rpcUrl: "https://sepolia.infura.io/v3/bb56b729ee2d4167b64c660f7c99529f", // Replace with your Sepolia RPC URL
            isTestnet: true,
            imageUrl: "https://cryptologos.cc/logos/ethereum-eth-logo.png"
        );
        
        await Web3Modal.InitializeAsync(new Web3ModalConfig
        {
            supportedChains = new[]
            {
                sepolia
            }
        });
        
        Web3Modal.ChainChanged += (_, chainArgs) =>
        {
            Debug.Log($"ChainId Changed: {chainArgs.Chain.ChainId}");
        };
        
        //Invoked after successful connection of an account
        Web3Modal.AccountChanged += (_, eventArgs) =>
        {
            Account newAccount = eventArgs.Account;
            Debug.Log($"AccountChanged. Address: {newAccount.Address}");
        };
        
        // Invoked after wallet connected
        Web3Modal.AccountConnected += (_, eventArgs) =>
        {
            AccountConnected(eventArgs);
        };

        //  Invoked after successful disconnection of an account
        Web3Modal.AccountDisconnected  += (_, _) =>
        {
            Debug.Log($"AccountDisconnected");
            Login.gameObject.SetActive(true);
            LogOut.gameObject.SetActive(false);
        };

        
        var sessionResumed = await Web3Modal.ConnectorController.TryResumeSessionAsync();
        Login.gameObject.SetActive(!sessionResumed);
        LogOut.gameObject.SetActive(sessionResumed);
    }
    
    private async void AccountConnected(Connector.AccountConnectedEventArgs eventArgs)
    {
        Account activeAccount = await eventArgs.GetAccount();
        Debug.Log($"AccountConnected: {activeAccount.Address}");
        Login.gameObject.SetActive(false);
        LogOut.gameObject.SetActive(true);


        var session = WalletConnectUnity.Core.WalletConnect.Instance.ActiveSession;
        Debug.LogError(session);
        // OnPersonalSignButton();

        TestContract();
    }
    
    public async void OnPersonalSignButton()
    {
        Debug.Log("[Web3Modal Sample] OnPersonalSignButton");
        try
        {
            var account = await Web3Modal.GetAccountAsync();
            const string message = "Hello from Unity!";
            var signature = await Web3Modal.Evm.SignMessageAsync(message);
            var isValid = await Web3Modal.Evm.VerifyMessageSignatureAsync(account.Address, message, signature);

            Debug.LogError($"Signature valid: {isValid}");
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }
    }

    private async void TestContract()
    {
        Debug.LogError("TestContract");
        // ERC20 合约地址
        var contractAddress = "0xd72984FbE2836D82b2A917c862eAC20ACE41FB08";
        // 创建 ERC20 合约实例
        var fromAddress = "0xe98a897299cea70f9b86d7800640E4b0568b1015";
        // 转账的接收地址
        var toAddress = "0x19AaB55F22AF0FE7D6B509AeE400B7C26908CaE8";
        string abi = "[{\"inputs\":[{\"internalType\":\"uint256\",\"name\":\"initialSupply\",\"type\":\"uint256\"}],\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"allowance\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"needed\",\"type\":\"uint256\"}],\"name\":\"ERC20InsufficientAllowance\",\"type\":\"error\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"sender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"balance\",\"type\":\"uint256\"},{\"internalType\":\"uint256\",\"name\":\"needed\",\"type\":\"uint256\"}],\"name\":\"ERC20InsufficientBalance\",\"type\":\"error\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"approver\",\"type\":\"address\"}],\"name\":\"ERC20InvalidApprover\",\"type\":\"error\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"receiver\",\"type\":\"address\"}],\"name\":\"ERC20InvalidReceiver\",\"type\":\"error\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"sender\",\"type\":\"address\"}],\"name\":\"ERC20InvalidSender\",\"type\":\"error\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"}],\"name\":\"ERC20InvalidSpender\",\"type\":\"error\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"Approval\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"Transfer\",\"type\":\"event\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"owner\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"}],\"name\":\"allowance\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"spender\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"approve\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"}],\"name\":\"balanceOf\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"decimals\",\"outputs\":[{\"internalType\":\"uint8\",\"name\":\"\",\"type\":\"uint8\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"account\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"mint\",\"outputs\":[],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"name\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"symbol\",\"outputs\":[{\"internalType\":\"string\",\"name\":\"\",\"type\":\"string\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"name\":\"totalSupply\",\"outputs\":[{\"internalType\":\"uint256\",\"name\":\"\",\"type\":\"uint256\"}],\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"transfer\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"inputs\":[{\"internalType\":\"address\",\"name\":\"from\",\"type\":\"address\"},{\"internalType\":\"address\",\"name\":\"to\",\"type\":\"address\"},{\"internalType\":\"uint256\",\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"transferFrom\",\"outputs\":[{\"internalType\":\"bool\",\"name\":\"\",\"type\":\"bool\"}],\"stateMutability\":\"nonpayable\",\"type\":\"function\"}]";
        
        Debug.LogError($"Addr1 BalanceOf  : {await Web3Modal.Evm.ReadContractAsync<BigInteger>(contractAddress, abi, "balanceOf", new object[]{fromAddress})}");
        Debug.LogError($"Addr2 BalanceOf  : {await Web3Modal.Evm.ReadContractAsync<BigInteger>(contractAddress, abi, "balanceOf", new object[]{toAddress})}");
        var result = await Web3Modal.Evm.WriteContractAsync(contractAddress, abi, "transfer", new object[]{toAddress,100*Math.Pow(10,18)});
        Debug.LogError($"tx :{result}");
        Debug.LogError($"Addr1 BalanceOf  : {await Web3Modal.Evm.ReadContractAsync<BigInteger>(contractAddress, abi, "balanceOf", new object[]{fromAddress})}");
        Debug.LogError($"Addr2 BalanceOf  : {await Web3Modal.Evm.ReadContractAsync<BigInteger>(contractAddress, abi, "balanceOf", new object[]{toAddress})}");
    }

    private void OnLoginClick()
    {
        Web3Modal.OpenModal(ViewType.Connect);
    }
    
    private async void OnLogOutClick()
    {
        LogOut.gameObject.SetActive(false);
        await Web3Modal.DisconnectAsync();
    }
}
