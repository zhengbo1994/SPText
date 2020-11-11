Asc.tree = Asc.tree || {};
/*
	Asc.tree.cateTree 分类树
	id		元素id(带#)
	data	一级数据
	getDate		点击响应函数(参数tree对象)
*/

Asc.tree.cateTree = function(id,getDate){
	this.el = $(id);
	this.getDate = getDate;
	this.data={};
	this.end = false;
	this.Init(getDate);
	this.bind(this.container,'click',this.clickLis,this);
}

/*
	Init 初始化
*/
Asc.tree.cateTree.prototype.Init = function(){
	this.el.addClass('asc-tree');
	this.el.append('<div class="asc-tree-c cc"></div><div class="asc-tree-nav cc"><ul><li class="txt">你当前选择的是：</li></ul></div>');
	this.container = $(this.el.children('.asc-tree-c')[0]);
	this.getDate.call(this,this,0);
}

/*
	CreateHtml 生成html字符串
	data		数据
	color		搜索着色
*/
Asc.tree.cateTree.prototype.CreateHtml = function(data,color){
	var list = data.list;
	var liststr='';
	var c='';
	for(var i=0,len=list.length;i<len;i++){
		var n = list[i];
		color = color || 0;
		c = n.son?' class="parent"':c;
		if(color){
			c = n.light?' class="parent searchlist"':c;
		}
		liststr+='<li'+c+'><span itemid="'+n.rid+'">'+n.txt+'</span></li>';
	}
	if(data.level==1){
		liststr = '<div class="asc-tree-list"><ul level="'+data.level+'">'+liststr+'</ul></div>';
	}else{
		if(this.el.find('.asc-tree-list').length<data.level){
			liststr = '<div class="asc-tree-list"><ul class="asc-tree-son" level="'+data.level+'">'+liststr+'</ul><div class="asc-tree-search"><input type="text" class="input_search" value="请输入名称或拼音首字母查找"/></div></div>'
		}
	}
	return liststr;
	
}
/*
	updateHtml 生成html
	data		数据
*/
Asc.tree.cateTree.prototype.updateHtml = function(data){
	this.data['level'+data.level] = data;
	var html = this.CreateHtml(data);
	var list = this.el.find('.asc-tree-list');
	if(data.level>1){
		if(data.level<list.length){
			list.slice(data.level).remove();
			$(list[data.level-1]).find('ul:first').html(html);
		}else if(data.level==list.length){
			$(list[data.level-1]).find('ul:first').html(html);
		}else{
			html = $(html);
			this.bind(html.find('input:first'),'focus',this.inputFocus,this);
			this.bind(html.find('input:first'),'blur',this.inputBlur,this);
			//this.bind(html.find('input:first'),'keyup',this.inputKeyDown,this);搜索算法有问题
			this.container.append(html);
		}
	}else{
		this.container.append(html);
	}
	this.updatePath();
	
}
/*
	updateHtml 生成html
*/
Asc.tree.cateTree.prototype.error = function(){
	alert('请求失败');
}
/*
	inputFocus input事件
	e	事件对象
*/
Asc.tree.cateTree.prototype.inputFocus = function(e){
	el = $(e.target);
	if(el.val()=='请输入名称或拼音首字母查找'){
		el.val('');
	}
}
/*
	inputBlur input事件
*/
Asc.tree.cateTree.prototype.inputBlur = function(e){
	el = $(e.target);
	if(el.val()==''){
		el.val('请输入名称或拼音首字母查找');
	}
}
/*
	inputBlur input事件
*/
Asc.tree.cateTree.prototype.inputKeyDown = function(e){
	var el = $(e.target);
	var list = el.parents('.asc-tree-list').children('ul');
	var level = list.attr('level');
	var data = this.data['level'+level];
	var datalist = this.data['level'+level]['list'];
	var val = el.val();
	if(val!=''){
		datalist.sort(function(a,b){
			if(a.py.indexOf(val[0])==-1){
				a['light']=false;
				return 1;
			}else{
				a['light']=true;
			}
			return -1;
		})
		list.html(this.CreateHtml(data,1));
	}
	
}
/*
	clickLis 监听列表点击事件
*/
Asc.tree.cateTree.prototype.clickLis = function(e){
		if(e.target.nodeName.toLowerCase()=='span'){
			var eel = $(e.target);
			var ul = eel.parents('ul');
			var level = ul.attr('level');
			var pathstr='';
			var pel = eel.parent();
			this.resetSelect(ul);
			pel.addClass('selected');
			if(pel.attr('class').indexOf('parent')!=-1){
				this.getDate.call(this,this,eel.attr('itemid'));
			}else{
				if(this.el.find('.asc-tree-list').length>level){
					this.el.find('.asc-tree-list').slice(level).remove();
				}
				this.updatePath();
				this.end = true;
			}
		}
}
/*
	updatePath 更新路径
*/
Asc.tree.cateTree.prototype.updatePath=function(){
	var list = this.el.find('.selected');
	var path = [];
	list.each(function(i,n){
		var pitem = {};
		pitem.id = $(n).children('span').attr('itemid');
		pitem.text = $(n).children('span').text();
		pitem.level = $(n).parents('ul').attr('level');
		path.push(pitem);
	})
	this.path = path;
	var html = '<li class="txt">你当前选择的是：</li>';
	for(var i=0,len=path.length;i<len;i++){
		if(path[i].level==1){
			html+='<li class="root"><a href="javascript:void(0);">'+path[i].text+'</a></li>';
		}else{
			html+='<li class="son"><a href="javascript:void(0);">'+path[i].text+'</a></li>';
		}
	}
	this.el.find('.asc-tree-nav ul:first').html(html);
}

/*
	resetSelect 重置选定样式
*/
Asc.tree.cateTree.prototype.resetSelect=function(el){
	var list = el.find('li');
	list.each(function(i,n){
		$(n).removeClass('selected');
	})
}
/*
	添加遮罩
*/
Asc.tree.cateTree.prototype.addMask = function(){
	var html ='<div class="asc-tree-loading"></div>';
	this.loading = $(html);
	this.el.append(this.loading);
}

/*
	removeMask 删除加载层
*/
Asc.tree.cateTree.prototype.removeMask = function(){
	this.loading.remove();
}


/*
	bind 事件监听
	el 元素(jquery对象)
	ename 事件名
	fn 监听函数
	scope 作用域
	arr 参数列表
*/
Asc.tree.cateTree.prototype.bind = function(el,ename,fn,scope,arr){
	arr = arr || [];
	var len = arr.length;
	el.bind(ename,function(e){
		if(arr.length==len){
			arr.unshift(e);
		}else{
			arr.shift();
			arr.unshift(e);
		}
		fn.apply(scope,arr)
	})
}


