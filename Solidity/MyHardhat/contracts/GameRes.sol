// SPDX-License-Identifier: MIT
pragma solidity ^0.8.26;

import {ERC20} from "@openzeppelin/contracts/token/ERC20/ERC20.sol";
import "hardhat/console.sol";


contract GameResToken is ERC20 {
    constructor(uint256 initialSupply) ERC20("GameRes", "GR") {
        _mint(msg.sender, initialSupply);
    }

    function mint(address account, uint256 value) public {
       _mint(account,value);
    }
}