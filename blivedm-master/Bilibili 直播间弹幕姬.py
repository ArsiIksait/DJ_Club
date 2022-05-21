# -*- coding:utf-8 -*-
# 时间:2020/8/3
# 获取B站直播间弹幕作者(A): 猫先生的早茶
# Socket客户端作者(B): 凤兮凤兮非无凰
# 整合二改作者(C): ArsiIksait
# 二改时间: 2022/01/24 18:45
"""
	版权声明：本文为CSDN博主「猫先生的早茶」的原创文章，遵循CC 4.0 BY-SA版权协议，转载请附上原文出处链接及本声明。
	原文链接：https://blog.csdn.net/qq_43017750/article/details/88041247

	原作者(A)注:
	获取bilibili直播间弹幕
	房间号从网页源代码中获取
	打开直播画面后，按ctrl+u 打开网页源代码，按ctrl+f 搜索 room_id
	搜到的"room_id":1016中，1016就是房间号 
	获取不同房间的弹幕:修改代码第26行的roomid的值为对应的房间号
	
	二改作者(C)注:
	房间号也可以在直播间链接中获取,比如我的直播间链接是(https://live.bilibili.com/ >>>7683400<<< ?spm_id_from=333.999.0.0)
	那么链接里的7683400就是我的直播间房间号(room_id)
"""

import socket
import requests;
import time;
import sys;

class Danmu():
	def __init__(self):
		# 弹幕url
		self.url = 'https://api.live.bilibili.com/xlive/web-room/v1/dM/gethistory';
		# 请求头
		self.headers = {
			'Host':'api.live.bilibili.com',
			'User-Agent':'Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:78.0) Gecko/20100101 Firefox/78.0',
		}
		# 定义POST传递的参数
		self.data = {
			'roomid':'5050',
			'csrf_token':'',
			'csrf':'',
			'visit_id':'',
		}
		# 日志写对象
		self.log_file_write = open('danmu.log',mode='a',encoding='utf-8');
		# 读取日志
		log_file_read = open('danmu.log',mode='r',encoding='utf-8');
		self.log = log_file_read.readlines();
		
	def sendMsg(msg):
		"""
		版权声明：本文为CSDN博主「凤兮凤兮非无凰」的原创文章，遵循CC 4.0 BY-SA版权协议，转载请附上原文出处链接及本声明。
		原文链接：https://blog.csdn.net/abc_12366/article/details/87062950
		"""
		Address = ('127.0.0.1', 8088) # Socket服务器地址,根据自己情况修改
		s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
		try:
			s.connect(Address)  # 尝试连接服务端
			s.sendall(msg.encode()) # 尝试向服务端发送消息
		except Exception:
			print(time.strftime("[%Y-%m-%d %H:%M:%S]", time.localtime()) + ' [ERROR] 无法连接到Socket服务器,请检查服务器是否启动')
		s.close()
		
	def get_danmu(self):
		# 获取直播间弹幕
		html = requests.post(url=self.url,headers=self.headers,data=self.data).json();
		# 解析弹幕列表
		for content in html['data']['room']:
			# 获取昵称
			nickname = content['nickname'];
			# 获取发言
			text = content['text'];
			# 获取发言时间
			timeline = content['timeline'];
			# 记录发言
			msg = '['+timeline+']'+' '+'['+nickname+']'+' '+'['+text+']';
			# 判断对应消息是否存在于日志，如果和最后一条相同则打印并保存
			if msg+'\n' not in self.log:
				# 打印消息
				print (msg);
				# 保存日志
				self.log_file_write.write(msg+'\n');
				# 添加到日志列表
				self.log.append(msg+'\n');
				# 发送消息到SocketServer
				Danmu.sendMsg(msg);
			# 清空变量缓存
			nickname = '';
			text = '';
			timeline = '';
			msg = '';

# 创建bDanmu实例
print("Bilibili 直播间弹幕姬已启动 正在检查Socket连接状态...")
Danmu.sendMsg("");
bDanmu = Danmu();
while True:
	# 暂停0.5防止cpu占用过高
	time.sleep(0.5);
	# 获取弹幕
	bDanmu.get_danmu();