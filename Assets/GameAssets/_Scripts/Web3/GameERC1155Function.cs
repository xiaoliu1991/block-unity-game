using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;


[Function("safeTransferFrom")]
public class GameSafeTransferFromFunctionBase : FunctionMessage
{
    [Parameter("address", "from", 1)]
    public virtual string From { get; set; }

    [Parameter("address", "to", 2)]
    public virtual string To { get; set; }

    [Parameter("uint256", "id", 3)]
    public virtual BigInteger Id { get; set; }

    [Parameter("uint256", "value", 4)]
    public virtual BigInteger Amount { get; set; }

    [Parameter("bytes", "data", 5)]
    public virtual byte[] Data { get; set; }
}