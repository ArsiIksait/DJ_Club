# -*- coding: utf-8 -*-

import asyncio
import socket
import blivedm
import time

class Socket():
    def sendMsg(msg):
        """
        版权声明：本文为CSDN博主「凤兮凤兮非无凰」的原创文章，遵循CC 4.0 BY-SA版权协议，转载请附上原文出处链接及本声明。
        原文链接：https://blog.csdn.net/abc_12366/article/details/87062950
        """
        Address = ('127.0.0.1', 25565) # Socket服务器地址,根据自己情况修改
        s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        try:
            s.connect(Address)  # 尝试连接服务端
            s.sendall(msg.encode()) # 尝试向服务端发送消息
        except Exception:
            print(time.strftime("[%Y-%m-%d %H:%M:%S]", time.localtime()) + ' [ERROR] 无法连接到Socket服务器,请检查服务器是否启动')
        s.close()

class MyBLiveClient(blivedm.BLiveClient):

    async def _on_receive_danmaku(self, danmaku: blivedm.DanmakuMessage):
        print(time.strftime("[%Y-%m-%d %H:%M:%S]", time.localtime()) + f' [{danmaku.uid}] [{danmaku.uname}] [{danmaku.msg}]')
        Socket.sendMsg(f'msg\0{danmaku.uid}\0{danmaku.uname}\0{danmaku.msg}')

    async def _on_receive_gift(self, gift: blivedm.GiftMessage):
        print(time.strftime("[%Y-%m-%d %H:%M:%S]", time.localtime()) + f' [{gift.uid}] [{gift.uname}] 赠送 [{gift.gift_name}x{gift.num}] ({gift.coin_type}币x{gift.total_coin})')
        Socket.sendMsg(f'gift\0{gift.uid}\0{gift.uname}\0{gift.gift_name}\0{gift.num}\0{gift.price}\0{gift.coin_type}\0{int(round(time.time()))}') 
# msg 123 ArsiIksait 点歌 76
# gift 123 ArsiIksait 小电视 1 10000000 gold 4523654323

async def main():
    # 参数1是直播间ID
    # 如果SSL验证失败就把ssl设为False
    client = MyBLiveClient(7683400, ssl=True)
    future = client.start()
    try:
        # 5秒后停止，测试用
        # await asyncio.sleep(5)
        # future = client.stop()
        # 或者
        # future.cancel()

        await future
    finally:
        await client.close()


if __name__ == '__main__':
    asyncio.get_event_loop().run_until_complete(main())
