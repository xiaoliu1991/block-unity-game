const { expect } = require("chai");
const { ethers } = require("hardhat");

describe("GameGemsToken",async function(){
    it("Test GameGemsToken",async function () {
        //0xf39Fd6e51aad88F6F4ce6aB8827279cffFb92266
        //0x70997970C51812dc3A010C7d01b50e0d17dc79C8
        //0x3C44CdDdB6a900fa2b585dd299e03d12FA4293BC
        //0x90F79bf6EB2c4f870365E785982E1f101E93b906
        const [owner,addr1,addr2,addr3] = await ethers.getSigners();
        
        const GameGemsToken = await ethers.getContractFactory("GameGemsToken");
        const _GameGemsToken = await GameGemsToken.deploy(10000);

        const res = await _GameGemsToken.totalSupply();
        expect(res).to.equal(10000);

        expect(await _GameGemsToken.balanceOf(owner)).to.equal(10000);
        expect(await _GameGemsToken.balanceOf(addr1)).to.equal(0);
        //合约直接转账给addr1
        await _GameGemsToken.transfer(addr1,100);
        expect(await _GameGemsToken.balanceOf(addr1)).to.equal(100);
        //运行owner使用owner额度转账给addr2
        await _GameGemsToken.approve(owner,100);
        //owner转账给addr1
        await _GameGemsToken.transferFrom(owner,addr1,100);
        expect(await _GameGemsToken.balanceOf(owner)).to.equal(9800);
        expect(await _GameGemsToken.balanceOf(addr1)).to.equal(200);

        //运行owner使用addr1额度转账给addr2
        await _GameGemsToken.connect(addr1).approve(owner,50);
        //addr1转账给addr2
        await _GameGemsToken.transferFrom(addr1,addr2,50);
        expect(await _GameGemsToken.balanceOf(addr1)).to.equal(150);
        expect(await _GameGemsToken.balanceOf(addr2)).to.equal(50);
    });
});