using System;
using System.Threading.Tasks;
using Nethereum.Unity.Rpc;
using Nethereum.Web3;
using UnityEngine;

public class GetLatestBlock : MonoBehaviour
{
    public string Url = "https://sepolia.infura.io/v3/bb56b729ee2d4167b64c660f7c99529f";
    void Start()
    {
        GetBlockNumberRequestTask();
    }

    private void GetBlockNumberRequestTask()
    {
        CheckBlockNumberPeriodically(Url).ConfigureAwait(false);
    }
    
    private async Task CheckBlockNumberPeriodically(string url)
    {
        var web3 = new Web3(new UnityWebRequestRpcTaskClient(new Uri(url)));
        var blockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
        Debug.LogError($"Chain blockNumber : {blockNumber}");
    }
}
