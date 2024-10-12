using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZTestMono : MonoBehaviour
{
    // Start is called before the first frame update
    async void Start()
    {
        new GameERC20Test().CallTransfer();
    }
}
