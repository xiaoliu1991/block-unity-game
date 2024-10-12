// SPDX-License-Identifier: MIT
pragma solidity 0.8.26;

import "hardhat/console.sol";

contract Hello{
    uint counter;

    constructor(){
        counter = 1;
    }

    function getCounter() external view returns (uint){
        console.log(msg.sender);
        return counter;
    }

    // function add(uint a,uint b) external pure returns (uint){
    //     console.log("a => ", a);
    //     console.log("b => ", b);
    //     return a + b;
    // }
}