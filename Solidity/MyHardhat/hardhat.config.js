require("@nomicfoundation/hardhat-toolbox");

// task("Accounts","Prints the list of accounts",async(taskArgs,hre)=>{
//   const accounts = await hre.ethers.getSigners();
//   accounts.forEach(account => {
//       console.log(account);
//   });
// });

/** @type import('hardhat/config').HardhatUserConfig */
module.exports = {
  solidity: "0.8.26",
  paths:{
    artifacts:"./artifacts"
  },

  networks:{
    sepolia:{
      url: `https://sepolia.infura.io/v3/bb56b729ee2d4167b64c660f7c99529f`,//访问Sepolia url
      accounts: ["562c5b7417eca5b6b95032c7faa758b39746aaf23018225ffc250d25142089eb"]//metamask 私钥
    },
    // localhost:{
    //   url: "http://127.0.0.1:8545",
    // }
  },
  etherscan: {
    apiKey: {
      sepolia: 'bb56b729ee2d4167b64c660f7c99529f'
    }
  }
  // dependenies:{
  //   '@openzeppelin/contracts':'^4.5.0'
  // }
};
