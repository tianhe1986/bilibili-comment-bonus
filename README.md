# bilibili-comment-bonus
该小工具是参考了[这篇文章](https://zhuanlan.zhihu.com/p/136594048)写的。

此文章中说的是转发抽奖，而我这里处理的是动态评论抽奖。

为什么用C#写，当然是因为，可以直接生成exe拿给非技术人员。
对于真正需要用它的小白，减少些负担，别叫他们装这个环境那个环境吧，太难了，给他们发个能直接双击打开用的程序多好。

使用方式应该不用特别说明了吧，就两个输入框，一个填动态对应的ID，一个填总中奖人数。

# 处理流程
流程本身是很简单的：
1. 通过接口获取所有评论对应的用户id，去重后放入一个数组中。
2. 从第1步的数组中随机选择n个（n为总中奖人数），展示结果。

但真正处理起来会碰到些问题。

例如，随便选了条动态，我也不知道为啥选的是萌妹纸，对应URL是`https://t.bilibili.com/419720499278140689?tab=2`

用chrome分析请求，能够发现，获取评论的接口对应URL是`https://api.bilibili.com/x/v2/reply?callback=jQuery331011330966944510901_1596623798388&jsonp=jsonp&pn=1&type=11&oid=85364891&sort=2&_=1596623798390`

这个URL直接用新窗口打开是不成的，需要更改下参数，我这边研究了下，用这样的URL就可以了`https://api.bilibili.com/x/v2/reply?jsonp=jsonp&pn=1&type=11&oid=85364891&sort=2`

这里面几个关键的参数如下：
* pn: 表示当前获取的是第几页的评论。
* oid: 实际用来获取评论的id，这个接下来会讲解它造成的问题。
* type: 跟oid配套的参数，接下来会一并讲。
* sort: 排序方式。跟页面中的两项对应，2表示按热度排序，0表示按时间排序。

可以看到，这个oid跟动态的ID是不同的，那么，得有个办法获得它，对吧？

同样的，分析请求，发现了这个接口`https://api.vc.bilibili.com/dynamic_svr/v1/dynamic_svr/get_dynamic_detail?dynamic_id=419720499278140689`

在返回的json数据中，能够看到data.card.desc.rid对应的值就是85364891，这很美好。

但是我在看一些其他动态的时候发现，对应的oid使用的不是这个新的rid，而就是动态id，而type也不一样。

目前，我发现了以下两种情况：
* data.card.desc.type = 2， 则oid应传data.card.desc.rid，type应传11。
* 其他， oid应传动态id，type应传17。

当然可能会有更多的情况，需要继续扩充。但是我这边的同事目前使用足够了，所以我这边先这样了。

如果有需要，请自行修改。

# 关于pull request
请提到develop分支吧