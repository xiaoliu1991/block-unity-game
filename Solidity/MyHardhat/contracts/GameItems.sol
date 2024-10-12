// SPDX-License-Identifier: MIT
pragma solidity ^0.8.26;

import {ERC1155} from "@openzeppelin/contracts/token/ERC1155/ERC1155.sol";
import "@openzeppelin/contracts/access/Ownable.sol";
import "hardhat/console.sol";

contract GameItemsToken is ERC1155,Ownable(msg.sender) {
    // 定义代币ID
    uint256 public constant GOLD = 1;

    // 构造函数，设置初始 URI
    constructor() ERC1155("https://game.example/api/item/{id}.json") {
        // 初始化 mint 代币
        _mint(msg.sender, GOLD, 10000, "GOLD"); // 铸造1万个 GOLD 代币给部署者
    }

    // 批量铸造功能，只有合约所有者可以调用
    function mintBatch(address to, uint256[] memory ids, uint256[] memory amounts, bytes memory data) public onlyOwner {
        _mintBatch(to, ids, amounts, data);
    }

    // 铸造单个代币
    function mintSingleToken(address to, uint256 id, uint256 amount, bytes memory data) public onlyOwner {
        console.log("mintSingleToken",to);
        _mint(to, id, amount, data);
    }

    function setApprovalForAll(address operator, bool approved) public override {
        address sender = _msgSender();
        console.log("setApprovalForAll",sender,operator,approved);
        _setApprovalForAll(sender, operator, approved);
    }

    function safeTransferFrom(address from, address to, uint256 id, uint256 value, bytes memory data) public override {
        address sender = _msgSender();
        bool b = isApprovedForAll(from, sender);
        console.log("safeTransferFrom0",sender,from,b);
        if (from != sender && !b) {
            revert ERC1155MissingApprovalForAll(sender, from);
        }
        console.log("safeTransferFrom2",from,to,id);

        uint256 fromBalance = balanceOf(from,id);
        console.log("safeTransferFrom3",from,fromBalance);
        _safeTransferFrom(from, to, id, value, data);
    }
}