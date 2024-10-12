using System;
using Nethereum.Web3;
using UnityEngine;
using WalletConnect.Web3Modal;

public class LoginManager : MonoBehaviour
{
    public static LoginManager Instance { get; private set; }

    public event Action<Account> OnAccountConnected;
    public event Action AccountDisconnected;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    
    public async void Init(Action call)
    {
        await Web3Modal.InitializeAsync();
        OnInit();
        call?.Invoke();
    }
    
    
    private void OnInit()
    {
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
            AccountDisconnected?.Invoke();
        };
    }
    
    private async void AccountConnected(Connector.AccountConnectedEventArgs eventArgs)
    {
        Account activeAccount = await eventArgs.GetAccount();
        Debug.Log($"AccountConnected: {activeAccount.Address}");
        OnAccountConnected?.Invoke(activeAccount);
    }
    
    
    public void OnLogin()
    {
        Web3Modal.OpenModal(ViewType.Connect);
    }
    
    public async void OnLogOut()
    {
        await Web3Modal.DisconnectAsync();
    }
}
