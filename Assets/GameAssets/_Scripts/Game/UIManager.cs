using System;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Web3.Accounts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }
    
    [SerializeField]
    private GameObject _mainMenuPanel;

    [SerializeField]
    private GameObject _movingPanel;

    [SerializeField]
    private GameObject _gatheringPanel;

    [SerializeField]
    private GameObject _shoppingPanel;

    [SerializeField]
    private GameObject _tradingPanel;

    [SerializeField]
    private GameObject _gameOverPanel;

    [SerializeField]
    private TMP_Text _stoneText;

    [SerializeField]
    private TMP_Text _totalItemsOwnedText;

    [SerializeField]
    private TMP_Text _shopLog;

    [SerializeField]
    private Button _playButton;

    [SerializeField]
    private TMP_Text _marketplaceLog;

    [SerializeField]
    private TMP_InputField _account;

    [SerializeField]
    private TMP_InputField _password;


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
        _playButton.onClick.AddListener(OnPlayClick);
    }

    private void OnPlayClick()
    {
        
        WebThreeManager.Instance.Setup(new Account("0x59c6995e998f97a5a0044966f0945389dc9e86dae88c7a8412f4603b6b78690d"));
        GameManager.Instance.StartGame();
        
        return;
        try
        {
            if (string.IsNullOrEmpty(_account.text) || string.IsNullOrEmpty(_password.text))
            {
                return;
            }
            _account.interactable = false;
            _password.interactable = false;
            _playButton.interactable = false;
            _playButton.GetComponentInChildren<TMP_Text>().text = "Loading";
            WebThreeManager.Instance.Setup(AccountCreator.CreateAccount(_account.text,_password.text));
            GameManager.Instance.StartGame();
        }
        catch (Exception e)
        {
            _account.interactable = true;
            _password.interactable = true;
            _playButton.interactable = true;
            _playButton.GetComponentInChildren<TMP_Text>().text = "Play";
        }
    }

    private async void LateUpdate()
    {
        _stoneText.text = "Res Token: " + GameManager.Instance.StoneCount;

        if (GameManager.Instance.CurrentGameState == GameState.Shopping)
        {
            BigInteger totalItemsOwned = 0;
            var balances = GameManager.Instance.NFTBalances;
            foreach (var nft in balances)
                totalItemsOwned += nft.Value;
            _totalItemsOwnedText.text = "Total Items Owned: " + totalItemsOwned;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            await WebThreeManager.Instance.Erc1155.ApproveAllByShop();
        }
    }

    public void UpdateUI(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.MainMenu:
                _mainMenuPanel.SetActive(true);
                _movingPanel.SetActive(false);
                _gatheringPanel.SetActive(false);
                _shoppingPanel.SetActive(false);
                _tradingPanel.SetActive(false);
                _gameOverPanel.SetActive(false);
                break;
            case GameState.Moving:
                _mainMenuPanel.SetActive(false);
                _movingPanel.SetActive(true);
                _gatheringPanel.SetActive(false);
                _shoppingPanel.SetActive(false);
                _tradingPanel.SetActive(false);
                _gameOverPanel.SetActive(false);
                break;
            case GameState.Gathering:
                _gatheringPanel.SetActive(true);
                break;
            case GameState.Shopping:
                _shoppingPanel.SetActive(true);
                var shopItems = FindObjectsOfType<ShopItem>();
                foreach (var item in shopItems)
                    item.Initialize();
                break;
            case GameState.Trading:
                _tradingPanel.SetActive(true);
                var marketplaceItems = FindObjectsOfType<MarketplaceItem>();
                foreach (var item in marketplaceItems)
                    item.Initialize();
                break;
            case GameState.GameOver:
                _mainMenuPanel.SetActive(false);
                _movingPanel.SetActive(false);
                _gatheringPanel.SetActive(false);
                _shoppingPanel.SetActive(false);
                _tradingPanel.SetActive(false);
                _gameOverPanel.SetActive(true);
                break;
        }
    }
    internal void SetShopLog(string v)
    {
        _shopLog.text = v;
    }

    internal void SetMarketplaceLog(string v)
    {
        _marketplaceLog.text = v;
    }
}
