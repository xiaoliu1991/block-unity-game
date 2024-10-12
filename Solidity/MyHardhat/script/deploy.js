const hre = require("hardhat");

async function main() {

    const deployer = await hre.ethers.getSigners();
    console.log("Deploying contracts with the account:", deployer[0].address);

    const provider = hre.ethers.provider;
    const blockNum = await provider.getBlockNumber();
    console.log(`当前区块高度：${blockNum}`);

    const GameResToken = await hre.ethers.getContractFactory("GameResToken");
    const _GameResToken = await GameResToken.deploy(0);
    console.log(`GameResToken合约地址：${_GameResToken.target}`);

    const GameItemsToken = await hre.ethers.getContractFactory("GameItemsToken");
    const _GameItemsToken = await GameItemsToken.deploy();
    console.log(`GameItemsToken合约地址：${_GameItemsToken.target}`);

    const GameShop = await hre.ethers.getContractFactory("GameShop");
    const _GameShop = await GameShop.deploy(_GameResToken.target,_GameItemsToken.target);
    console.log(`GameShop合约地址：${_GameShop.target}`);
}


main().catch((error)=>{
    console.log(error);
    process.exitCode = 1;
});