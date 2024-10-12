// SPDX-License-Identifier: MIT
pragma solidity ^0.8.26;

import "@openzeppelin/contracts/token/ERC1155/IERC1155.sol";
import "@openzeppelin/contracts/token/ERC20/IERC20.sol";
import "@openzeppelin/contracts/access/Ownable.sol";
import "hardhat/console.sol";

contract GameShop is Ownable {
    mapping(uint256 id => uint256 price) private _goodsPrice;
    uint256[] private _goodsId;
    IERC20 private _erc20Token;      // ERC20 代币合约
    IERC1155 private _erc1155Token;  // ERC1155 代币合约

    event Purchase(address indexed buyer, uint256 amount, uint256 totalPrice);

    constructor(address _erc20TokenAddress, address _erc1155TokenAddress) Ownable(msg.sender) {
        _erc20Token = IERC20(_erc20TokenAddress);
        _erc1155Token = IERC1155(_erc1155TokenAddress);
        setPrice(1, 5000000000000000000);
    }


    function getPrice(uint256 id) public view returns(uint){
        return _goodsPrice[id];
    }

    function setPrice(uint256 id,uint256 price) public onlyOwner() returns(bool){
        if(_goodsPrice[id] <= 0){
            _goodsId.push(id);
        }

        _goodsPrice[id] = price;
        return true;
    }

    function getGoods() public view returns (uint256[] memory){
        return _goodsId;
    }

    // 用户购买
    function buy(uint256 id,uint256 amount) external returns(bool){
        require(amount > 0, "Must purchase at least 1 token");

        uint256 price = _goodsPrice[id];
        require(price > 0, "Price is 0");

        // 计算所需支付的总价格
        uint256 totalPrice = price * amount;
        // 检查用户的ERC20余额是否足够
        require(_erc20Token.balanceOf(msg.sender) >= totalPrice, "Insufficient ERC20 balance");

        // 检查用户是否批准了足够的 ERC20 代币给商店合约
        require(_erc20Token.allowance(msg.sender, address(this)) >= totalPrice, "Insufficient allowance");
        // 从用户账户中转移ERC20代币到商店部署者
        _erc20Token.transferFrom(msg.sender, owner(), totalPrice);
        // 商店转移ERC1155代币给购买者
        _erc1155Token.safeTransferFrom(owner(), msg.sender, id, amount, "");
        emit Purchase(msg.sender, amount, totalPrice);

        return true;
    }
}