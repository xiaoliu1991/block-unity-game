# block-unity-game

## 业务介绍
-玩家获取资源到商店进行资源交换

## 项目部署

### 本地部署和环境配置

#### 项目环境准备
1. nodejs

#### 项目配置
1. 进入到./Solidity/MyHardhat 依赖安装 `yarn`
2. 启动本地链 `npx hardhat node`
3. 部署合约（本地） 【这步骤生产环境中用管理员账号进行部署】
    
    `npx hardhat run ./script/deploy.js --network localhost`
    
    部署后控制台打印合约地址，初次部署是这个地址：
    
    -GameResToken合约地址：0x5FbDB2315678afecb367f032d93F642f64180aa3
    -GameItemsToken合约地址：0xe7f1725E7734CE288F8367e1Bb143E90bb3F0512
    -GameShop合约地址：0x9fE46736679d2D9a65F0992F2272dE9f3c7fa6e0
    
    把这个地址写到`WebThreeManager`下的中(./screenshot/1.jpg)


开始游戏(./screenshot/2.jpg)
捡苹果(./screenshot/3.jpg)
打开商店(./screenshot/4.jpg)
兑换金币(./screenshot/5.jpg)
