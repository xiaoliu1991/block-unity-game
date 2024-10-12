using System.Numerics;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;

[Function("getGoods","uint256[]")]
public class ShopGetGoodsFunction : FunctionMessage
{
    
}

[Function("getPrice","uint256")]
public class ShopGetPriceFunction : FunctionMessage
{
    [Parameter("uint256", "id", 1)]
    public BigInteger Id { get; set; }
}


[Function("setPrice", "bool")]
public class ShopSetPriceFunction : FunctionMessage
{
    [Parameter("uint256", "id", 1)]
    public virtual BigInteger Id { get; set; }

    [Parameter("uint256", "price", 2)]
    public virtual BigInteger Price { get; set; }
}

[Function("buy","bool")]
public class ShopBuyFunction : FunctionMessage
{
    [Parameter("uint256", "id", 1)]
    public virtual BigInteger Id { get; set; }

    [Parameter("uint256", "amount", 2)]
    public virtual BigInteger Amount { get; set; }
}