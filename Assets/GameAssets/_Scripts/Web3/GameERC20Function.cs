using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;


[Function("balanceOf","uint256")]
public class MyBalanceOfFunction : FunctionMessage
{
    [Parameter("address", "account", 1)]
    public string Owner { get; set; }
}


[Function("approve", "bool")]
public class MyApproveFunction : FunctionMessage
{
    [Parameter("address", "spender", 1)]
    public virtual string Spender { get; set; }

    [Parameter("uint256", "value", 2)]
    public virtual BigInteger Value { get; set; }
}

[Function("mint")]
public class MyMintFunction : FunctionMessage
{
    [Parameter("address", "account", 1)]
    public virtual string Owner { get; set; }

    [Parameter("uint256", "value", 2)]
    public virtual BigInteger Value { get; set; }
}