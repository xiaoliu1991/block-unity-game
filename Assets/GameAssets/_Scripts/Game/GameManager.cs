using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Nethereum.Unity.Rpc;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using UnityEngine;

public enum GameState
{
    MainMenu,
    Moving,
    Gathering,
    Shopping,
    Trading,
    GameOver
}

public class GameManager : MonoBehaviour
{
    public GameState CurrentGameState { get; private set; }

    public static GameManager Instance { get; private set; }
    public decimal StoneCount { get; private set; }
    public Dictionary<int, BigInteger> NFTBalances { get; private set; } = new();

    public bool Ready { get; private set; }

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

    private void Start()
    {
        CurrentGameState = GameState.MainMenu;
        UIManager.Instance.UpdateUI(GameState.MainMenu);
        CameraManager.Instance.UpdateCameraMode(GameState.MainMenu);
    }

    public async void StartGame()
    {
        try
        {
            await WebThreeManager.Instance.Erc1155.ApproveAllWithDeveloperByShop();
            await SyncResourceBalancesWithChain();
            Ready = true;
            SetGameState(GameState.Moving);
        }
        catch (Exception e)
        {
            Ready = false;
            Debug.LogError(e);
        }
    }

    internal void SetGameState(GameState gameState)
    {
        if (!Ready)return;
        CurrentGameState = gameState;
        UIManager.Instance.UpdateUI(gameState);
        CameraManager.Instance.UpdateCameraMode(gameState);
    }


    internal async void AddToken(Action call)
    {
        StoneCount++;
        await WebThreeManager.Instance.Erc20.ClaimToken(1);
        call?.Invoke();
    }


    internal async Task SyncResourceBalancesWithChain()
    {
        StoneCount = await WebThreeManager.Instance.Erc20.GetToken();
        var goods = await WebThreeManager.Instance.Shop.GetGoods();
        foreach (var goodsId in goods)
        {
            NFTBalances[(int)goodsId] = await WebThreeManager.Instance.Erc1155.GetCount((int)goodsId);
        }
    }

}
