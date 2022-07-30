"""
[课程内容]: Python采集唯品会商品数据/实现可视化数据展示/商品评论采集/评论词云分析

[授课老师]: 青灯教育-自游   20:05正式开始讲课

[环境使用]:
    Python 3.8
    Pycharm

[模块使用]:
    requests  ---> pip install requests
    csv 内置模块 不需要安装

---------------------------------------------------------------------------------------------------
win + R 输入cmd 输入安装命令 pip install 模块名 (如果你觉得安装速度比较慢, 你可以切换国内镜像源)
先听一下歌 等一下后面进来的同学,20:05正式开始讲课 [有什么喜欢听得歌曲 也可以在公屏发一下]
相对应的安装包/安装教程/激活码/使用教程/学习资料/工具插件 可以加落落老师微信
---------------------------------------------------------------------------------------------------
听课建议:
    1. 对于本节课讲解的内容, 有什么不明白的地方 可以直接在公屏上面提问, 具体哪行代码不清楚 具体那个操作不明白
    2. 不要跟着敲代码, 先听懂思路, 课后找落落老师领取录播, 然后再写代码
    3. 早退没有资料 <课后发送签到>
---------------------------------------------------------------------------------------------------
模块安装问题:
    - 如果安装python第三方模块:
        1. win + R 输入 cmd 点击确定, 输入安装命令 pip install 模块名 (pip install requests) 回车
        2. 在pycharm中点击Terminal(终端) 输入安装命令
    - 安装失败原因:
        - 失败一: pip 不是内部命令
            解决方法: 设置环境变量

        - 失败二: 出现大量报红 (read time out)
            解决方法: 因为是网络链接超时,  需要切换镜像源
                清华：https://pypi.tuna.tsinghua.edu.cn/simple
                阿里云：https://mirrors.aliyun.com/pypi/simple/
                中国科技大学 https://pypi.mirrors.ustc.edu.cn/simple/
                华中理工大学：https://pypi.hustunique.com/
                山东理工大学：https://pypi.sdutlinux.org/
                豆瓣：https://pypi.douban.com/simple/
                例如：pip3 install -i https://pypi.doubanio.com/simple/ 模块名

        - 失败三: cmd里面显示已经安装过了, 或者安装成功了, 但是在pycharm里面还是无法导入
            解决方法: 可能安装了多个python版本 (anaconda 或者 python 安装一个即可) 卸载一个就好
                    或者你pycharm里面python解释器没有设置好
---------------------------------------------------------------------------------------------------
如何配置pycharm里面的python解释器?
    1. 选择file(文件) >>> setting(设置) >>> Project(项目) >>> python interpreter(python解释器)
    2. 点击齿轮, 选择add
    3. 添加python安装路径
---------------------------------------------------------------------------------------------------
pycharm如何安装插件?
    1. 选择file(文件) >>> setting(设置) >>> Plugins(插件)
    2. 点击 Marketplace  输入想要安装的插件名字 比如:翻译插件 输入 translation / 汉化插件 输入 Chinese
    3. 选择相应的插件点击 install(安装) 即可
    4. 安装成功之后 是会弹出 重启pycharm的选项 点击确定, 重启即可生效
---------------------------------------------------------------------------------------------------
Python的应用学习方向有哪些?
    网站开发:
        如目前优秀的全栈的 django、框架flask ，都继承了python简单、明确的风格，开发效率高、易维护，与自动化运维结合性好。
        python已经成为自动化运维平台领域的事实标准；
        python开发的网站:
            豆瓣 https://www.douban.com/
            Youtube ,  Dropbox , 豆瓣...等等
    爬虫程序
        在爬虫领域，Python几乎是霸主地位，将网络一切数据作为资源，通过自动化程序进行有针对性的数据采集以及处理。
        从事该领域应学习爬虫策略、高性能异步IO、分布式爬虫等，并针对Scrapy框架源码进行深入剖析，从而理解其原理并实现自定义爬虫框架。
    数据分析
        Python语言相对于其它解释性语言最大的特点是其庞大而活跃的科学计算生态，在数据分析、交互、可视化方面有相当完善和优秀的库.
    自动化脚本
        执行许多重复的任务，例如阅读 pdf、播放音乐、查看天气、打开书签、清理文件夹等等，使用自动化脚本
        就无需手动一次又一次地完成这些任务，非常方便。
    人工智能
        各种人工智能算法都基于Python编写，尤其PyTorch之后，Python作为AI时代头牌语言的位置基本确定。
    游戏开发/辅助 自动化测试 运维
    ....
---------------------------------------------------------------------------------------------------
Python找工作就业方向以及薪资待遇情况怎么样?
    Python找工作方向主要是
        开发工程师 <网站开发/全栈开发>
            - 北京平均薪资23K
            - 应届生15K
            - 1-3年 16.9K
            - 3-5年22.9K
            - 数据来源: https://www.jobui.com/salary/beijing-pythonkaifagongchengshi/
            招聘数据来源:(Boss直聘)https://www.zhipin.com/job_detail/?query=python%E5%BC%80%E5%8F%91&city=101010100&industry=&position=
        爬虫工程师
            - 北京平均薪资22.5K
            - 应届生16.7K
            - 1-3年 18.1K
            - 3-5年 24K
            - 数据来源: https://www.jobui.com/salary/beijing-pachongkaifagongchengshi/
            招聘数据来源:(Boss直聘) https://www.zhipin.com/job_detail/?query=%E7%88%AC%E8%99%AB&city=101010100&industry=&position=
        数据分析师
            - 北京平均薪资25.3K
            - 应届生13.5K
            - 1-3年 19.4K
            - 3-5年26.2K
            - 数据来源: https://www.jobui.com/salary/beijing-shujufenxishi/
            招聘数据来源:(Boss直聘) https://www.zhipin.com/job_detail/?query=%E6%95%B0%E6%8D%AE%E5%88%86%E6%9E%90&city=101010100&industry=&position=
按照需求课程学完Python就业找工作薪资一般情况是在 8-15K左右
---------------------------------------------------------------------------------------------------
Python想要兼职接外包应该学习什么?
    外包是什么? 是指别人花钱请你帮他写程序, 根据甲方的需求定制化开发程序软件, 从而获得一定报酬.
    目前关于爬虫+数据分析外包相对而言会多一些:
        比如: 学生的毕设 / 课设 / 作业 ; 个人商家需要的一些数据采集 ; 某人公司需要的某些数据等等
            采集疫情数据做可视化分析 / 采集房源数据做可视化分析 / 采集招聘网站做可视化分析 / 采集电商平台做可视化分析 等等
            这些是普遍存在的外包需求
    根据外包的需求以及难易程序 外包的收费情况也是不一样的, 按照分布计算
        简单的外包: 100-300左右            耗时: 30-60分钟左右
        中等的外包: 500+ 左右              耗时: 1-2个小时左右
        难度稍大外包: 价格 1000+ 不封顶等   耗时: 3-5天不等

    爬虫和数据分析外包 一般情况写外包的周期相对而言会比较多短

    网站开发的外包难度以及耗时相当而言会大一些
        比如开发后台数据管理系统 / 某公司企业官网 / 或者学生毕设等等
    但是价格相对而言都是比较高的, 网站开发价格普通是上千

    可以去哪里接外包?
        - 淘宝 / 闲鱼
        - 外包接单群 <QQ外包群或者微信外包群> 有抽成一般抽3成
        - 外包接单平台: <需要押金适合工作室或者外包公司>
            1、解放号
            https://www.jfh.com/serviceProvider.html
            2、猿急送
            https://www.yuanjisong.com/job
            3、程序员客栈
            https://www.proginn.com/?loginbox=show
            4、码市
            https://codemart.com
            5、人人开发
            https://rrkf.com/
            6、猪八戒
            https://changsha.zbj.com/
            7、一品威客
            https://www.epwk.com/task/
            8、开源众包
            https://zb.oschina.net/projects/list.html
            9、智城外包网
            https://www.taskcity.com/
            10、实现网
            https://shixian.com/cases
            11、电鸭社区
            https://eleduck.com/categories/6/tags/0-18
            12、Remoteok
            https://remoteok.io/
            13、Toptal
            https://www.toptal.com/
            14、AngelList
            https://angel.co/
            15、英选
            https://www.yingxuan.io/
---------------------------------------------------------------------------------------------------
零基础同学 0
有基础同学 1

爬虫基本思路流程: <前戏很重要...通用...>

一. 数据来源分析:
    1. 确定需求, 我们采集的数据什么
    2. 通过开发者工具进行抓包分析, 分析数据来源....
        开发者工具 不会用 0  会用 6
        I. F12 或者 鼠标右键点击检查选择network 刷新网页
        II. 通过关键字搜索找寻数据包... --->
            50条数据内容
            50条数据内容
            20条数据内容

        一页数据 120 商品, 通过抓包分析, 找到三个数据包, 分别对应 前 50 中 50 后 20
            24 * 5 = 120

        通过对于商品数据包请求参数, 可以发现, 主要是商品ID改变 --->
        如果我想要获取本页所有数据内容, 获取找到所有商品ID, 并且把这ID分为三组 前50为一组, 中间50为一组 最后20为一组

        III. 通过ID 搜索找寻数据包

获取商品数据 ---> 获取商品ID

二. 代码实现步骤:
    1. 发送请求, 对于商品ID数据包发送请求
    2. 获取数据, 获取服务器返回响应数据 response
    3. 解析数据, 提取我们想要商品ID

    4. 发送请求, 把商品ID 分为三组 分别传入商品数据包发送请求
    5. 获取数据, 获取服务器返回响应数据 response
    6. 解析数据, 提取我们想要商品数据信息内容

    7. 保存数据, 保存数据保存表格里面


公开课讲解 案例演示, 告诉你python可以做什么, 实现什么案例 <先听思路, 在自己看录播抄代码>

系统课程才是从零基础入门系统知识点教学....

广告之后, 精彩继续..
招生...

核心编程 高级开发 爬虫实战 数据分析 全栈开发<前端+后端> 人工智能 自动化办公

直接带你从零基础入门到项目实战, 直接面向就业找工作....
    python方向工作岗位
        爬虫工程师
        数据分析师
        开发工程师

应届毕业生 8-15K左右   1-3年 15-25K左右  3-5年 20-35K左右

有想要学python ---> 6  湖南信息学院...
    1. 担心自己没有经验
    2. 担心自己学历不够

学历不值钱, 但是不代表他不重要 学历敲门砖.....

真的你是小学初中高中学历 ---> 提升学一下学历, 自考统考都可以, 让自己学历好看一些


16年实习, 从事在线教育行业, 在python部门, 16年从事python教育培训 工作时间 下午13点到24点
    转正 3500+ ,  然后和讲师走后门, 让他教我python ---> 下班之后学, 学到凌晨四五点...

    有问题找同事 ---> 有人帮你解答是很重要的.... 你的问题比VIP学员还多...

17年从事python相关...爬虫学了大半个月....

跟老师我来学习, 两周可以带你学会, 70-80%网站的数据采集....

有人带你学习, 和自学, 差别是真的很大的....

掌握课程内容 80%左右 就可以找工作

新班开班 明天开班 从零基础入门开始上课

    加婧琪老师微信: python1018
    预定300学费, 可以获取 1000 学费减免 ---> 7880
    学费是在腾讯课堂支付,有第三方平台保障
        8880 - 300 - 1000 = 7580
    申请12/18期分期免息学习,
        可以通过支付宝花呗 / 京东白条 / 信用卡 / 腾讯课堂官方教育分期 <海尔金融>
        学习无忧
        0利息0手续费
        7580 / 18 = 421/月

    每个月可以支付多少学费, 来投资自己学习...

今天报名会提供两个外包 200外包
    8月29支付第一期学费,  421 - 200 <外包> = 221
    9月29支付第二期学费,  421 - 200 <外包> = 221

    10月29号支付第三期学费, 421 自己已经学了三个月, 爬虫相关外包, 都可以去接了...
            1-2个外包, 你的学费就有了 --->  外包渠道提供给你, 只要你有技术都可以接
            接外包有问题 思路不懂 也可以找老师解答辅导

冻结学习时间 :
    你报名之后 暂时没有办法听直播课程, 课程录播资料都有 + 解答辅导也是有的
    只是暂时没办法听直播课程...


没有工作 != 没有收入来源
    因为你没有技术, 你可以学python接外包赚钱


"""
# 导入数据请求模块  --> 第三方模块 需要 在cmd里面进行pip install requests  不会2
import requests
# 导入格式化输出模块 --> 内置模块 不需要安装
from pprint import pprint
# 导入csv模块 --> 内置模块 不需要安装
import csv

# 创建文件
f = open('七夕礼物_多页.csv', mode='a', newline='', encoding='utf-8')
# 固定写法  f文件对象 fieldnames字段名 表头
csv_writer = csv.DictWriter(f, fieldnames=[
    '商品',
    '品牌',
    '原价',
    '售价',
    '折扣',
    '属性',
    '详情页',
])
# 写入表头
csv_writer.writeheader()


def get_shop_info(shop_id):
    """
    def 关键字 get_shop_info自定义函数名 shop_id 形式参数
    专门用来获取商品数据

    发送请求, 获取数据
    :param shop_id:商品ID
    :return:
    """
    # 确定url地址
    link = 'https://mapi.vip.com/vips-mobile/rest/shopping/pc/product/module/list/v2'
    params = {
        'app_name': 'shop_pc',
        'app_version': '4.0',
        'warehouse': 'VIP_HZ',
        'fdc_area_id': '104103101',
        'client': 'pc',
        'mobile_platform': '1',
        'province_id': '104103',
        'api_key': '70f71280d5d547b2a7bb370a529aeea1',
        'user_id': '',
        'mars_cid': '1655447722495_62c422a2b0d263186b2d64412108655f',
        'wap_consumer': 'a',
        'productIds': shop_id,
        'scene': 'search',
        'standby_id': 'nature',
        'extParams': '{"stdSizeVids":"","preheatTipsVer":"3","couponVer":"v2","exclusivePrice":"1","iconSpec":"2x","ic2label":1,"superHot":1,"bigBrand":"1"}',
        'context': '',
        '_': '1659096852827',
    }
    json_data = requests.get(url=link, params=params, headers=headers).json()
    # 解析数据
    for i in json_data['data']['products']:
        shop_attr = [j['name'] + ':' + j['value'] for j in i['attrs']]
        href = f'https://www.vipglobal.hk/detail-{i["brandId"]}-{i["productId"]}.html'
        dit = {
            '商品': i['title'],
            '品牌': i['brandShowName'],
            '原价': i['price']['marketPrice'],
            '售价': i['price']['salePrice'],
            '折扣': i['price']['mixPriceLabel'],
            '属性': shop_attr,
            '详情页': href,
        }
        csv_writer.writerow(dit)
        print(dit)


"""
1. 发送请求, 对于商品ID数据包发送请求

python模拟浏览器对于url地址<服务器>发送请求, 获取服务器返回响应数据
    - 选中替换内容, 按ctrl + R 输入 正则命令
        (.*?): (.*)
        '$1': '$2',
    - headers 请求头伪装
        为了防止被服务器识别出来是爬虫程序
        
        当你请求之后,获取数据没有得到数据, 或者得到的数据不是你想要的内容
        意味着你被反爬了...
    
    我都是复制粘贴的...
    
    我要的, 你知道每行代码的意思, 每个参数意思, 为什么这样去写, 别问, 解释不清...
        因为很多知识, 一节课讲不完
"""
for page in range(0, 721, 120):
    # 确定url地址
    url = 'https://mapi.vip.com/vips-mobile/rest/shopping/pc/search/product/rank'
    # 请求参数
    data = {
        # 'callback': 'getMerchandiseIds',
        'app_name': 'shop_pc',
        'app_version': '4.0',
        'warehouse': 'VIP_HZ',
        'fdc_area_id': '104103101',
        'client': 'pc',
        'mobile_platform': '1',
        'province_id': '104103',
        'api_key': '70f71280d5d547b2a7bb370a529aeea1',
        'user_id': '',
        'mars_cid': '1655447722495_62c422a2b0d263186b2d64412108655f',
        'wap_consumer': 'a',
        'standby_id': 'nature',
        'keyword': '口红',
        'lv3CatIds': '',
        'lv2CatIds': '',
        'lv1CatIds': '',
        'brandStoreSns': '',
        'props': '',
        'priceMin': '',
        'priceMax': '',
        'vipService': '',
        'sort': '0',
        'pageOffset': page,
        'channelId': '1',
        'gPlatform': 'PC',
        'batchSize': '120',
        '_': '1659096852824',
    }
    # 伪装headers   1  2
    headers = {
        # 防盗链 告诉服务器请求url地址 是从哪里跳转过来的 ---> 相当于你行程码...
        'referer': 'https://category.vip.com/',
        # user-agent 用户代理, 表示浏览器基本身份标识
        'user-agent': 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/101.0.0.0 Safari/537.36'
    }
    # 发送请求  params查询参数<get请求, 请求链接里面问号后面内容>, data 表单参数 <用于post请求>
    response = requests.get(url=url, params=data, headers=headers)
    # <Response [903]> 响应对象  903 状态码 开发人员给的特定返回状态码 ---> 没有请求成功 200请求成功print(response)
    # 获取数据 response.text文本数据 response.json() json字典数据  response.content 二进制数据
    # 键值对取值  根据冒号左边的内容[键], 提取冒号右边的内容[值]
    # products = response.json()['data']['products']
    # lis = []
    # # for 循环遍历 提取列表元素
    # for index in products:
    #     pid = index['pid']
    #     # 把商品ID添加到lis列表里面
    #     lis.append(pid)
    # 列表推导式
    products = [index['pid'] for index in response.json()['data']['products']]
    # 列表切片 ---> 返回列表   前 50 中 50 后 20  猜....  数据类型转换, 列表转字符串
    string_1 = ','.join(products[0:50])
    string_2 = ','.join(products[50:100])
    string_3 = ','.join(products[100:])
    # string_1 商品ID
    # get_shop_info() 调用获取商品数据函数 shop_id 函数形式参数 shop_id=string_1 关键字传参
    get_shop_info(shop_id=string_1)
    get_shop_info(shop_id=string_2)
    get_shop_info(shop_id=string_3)
