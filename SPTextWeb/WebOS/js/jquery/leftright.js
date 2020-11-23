Asc.tree = Asc.tree || {};
/*
	Asc.tree.moveitem	对象
	cid		组件id(带#)
	width	宽度
	height	高度
	leftr	移动时是否移除左边列表
	获取参数
		getParams(dir),dir=1或2(1左框,2右框)
*/
Asc.tree.moveitem = function(cid,width,height,leftr){
	width = width || false;
	height = height || false;
	leftr = leftr===false?false:true;
	this.leftlist = $(cid+"_left_list");
	this.leftbtns = $(cid+'_left_tool');
	this.rightlist = $(cid+"_right_list");
	this.rightbtns = $(cid+'_right_tool');
	this.btn_right = $(cid+"_toright");
	this.btn_left = $(cid+"_toleft");
	this.btn_top = $(cid+"_btntop");
	this.btn_bottom = $(cid+"_btnbottom");
	this.btn_up = $(cid+"_btnup");
	this.btn_down = $(cid+"_btndown");
	this.leftarr = this.getListValue(this.leftlist);
	this.rightarr = this.getListValue(this.rightlist);
	this.selected = this.getSelect();
	if(width&&height){
		this.initComp(cid,width,height);
	}
	var self = this;
	this.bind(this.leftbtns,'click',this.routerEvent,this,[this.leftlist]);
	this.bind(this.rightbtns,'click',this.routerEvent,this,[this.rightlist]);
	this.btn_right.bind('click',function(){
		var obj = self.getChecked(self.leftlist);
		if(obj.num){
			for(var i=0;i<obj.num;i++){
				self.rightarr[obj.list[i]]=self.leftarr[obj.list[i]];
				if(leftr){
					delete self.leftarr[obj.list[i]];
				}
			}
			self.update();
		}
	})
	this.btn_left.bind('click',function(){
		var obj = self.getChecked(self.rightlist);
		if(obj.num){
			for(var i=0;i<obj.num;i++){
				self.leftarr[obj.list[i]]=self.rightarr[obj.list[i]];
				delete self.rightarr[obj.list[i]];
			}
			self.update();
			self.selected = false;
		}
	})
	this.leftlist.bind('click',function(e){
		if(e.target.nodeName.toLowerCase()=='input'){
		}else{
			var target = self.getTarget(e,'li')
			if(target){
				if($(target).find('input').attr('checked')){
					$(target).find('input').attr('checked',false);
				}else{
					$(target).find('input').attr('checked',true);
				}
				
			}
		}
	})
	this.rightlist.bind('click',function(e){
		if(e.target.nodeName.toLowerCase()=='input'){
		}else{
			var target = self.getTarget(e,'li')
			if(target){
				self.resetSelect();
				self.selected=$(target).attr('val');
				$(target).addClass('selected');
			}
		}
	})
	this.btn_top.bind('click',function(){
		self.Asc.tree.moveitem('top');
	})
	this.btn_up.bind('click',function(){
		self.Asc.tree.moveitem('up');
	})
	this.btn_down.bind('click',function(){
		self.Asc.tree.moveitem('down');
	})
	this.btn_bottom.bind('click',function(){
		self.Asc.tree.moveitem('bottom');
	})
}
/*
	getParams 获取参数
	dir	左右框	可选值1 2
*/
Asc.tree.moveitem.prototype.getParams = function(dir){
	var arr = [];
	var p = dir==1?this.leftarr:this.rightarr;
	for(i in p){
		arr.push(i);
	}
	return arr;
}
/*
	initComp 初始化组件，提供自定义宽度和高速
	id	元素id
	w	宽度
	h	高度
*/
Asc.tree.moveitem.prototype.initComp=function(id,w,h){
	var boxw = Math.ceil((w-86)/2);
	var boxh = h-12;
	var listh = boxh-28;
	$(id).css({width:w+'px',height:h+'px'});
	$(id+'_left').css({width:boxw+'px',height:boxh+'px'});
	$(id+'_right').css({width:boxw+'px',height:boxh+'px'});
	$(id+'_righttool').css({height:boxh+'px'});
	if($(id+'_left').find('.npages').length){
		this.leftlist.css({height:(listh-35)+'px','overflow-y':'scroll'});
	}else{
		this.leftlist.css({height:listh+'px','overflow-y':'scroll'});
	}
	this.rightlist.css({height:listh+'px','overflow-y':'scroll'});
	
}
/*
	bind 事件监听
	el 元素(jquery对象)
	ename 事件名
	fn 监听函数
	scope 作用域
	arr 参数列表
*/
Asc.tree.moveitem.prototype.bind = function(el,ename,fn,scope,arr){
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
/*
	routerEvent 事件路由
	e 事件对象
	el 元素(jquery对象)
*/
Asc.tree.moveitem.prototype.routerEvent = function(e,el){
	if($(e.target).attr('class').indexOf('btn_selectall')!=-1){
		this.selectAll(el);
	}
	if($(e.target).attr('class').indexOf('btn_selectdel')!=-1){
		this.delSelect(el);
	}
}
/*
	selectAll 全选功能
	e 事件对象
	el 父元素(jquery对象)
*/
Asc.tree.moveitem.prototype.delSelect = function(el){
	var obj = this.getChecked(el);
	if(obj.num){
		var list = el.find('input');
		list.each(function(i,n){
			if($(n).attr('checked')){
				$(n).parent().remove();
			}
		})
	}
	for(var i=0;i<obj.num;i++){
		delete this.rightarr[obj.list[i]];
	}
}
/*
	selectAll 全选功能
	e 事件对象
	el 父元素(jquery对象)
*/
Asc.tree.moveitem.prototype.selectAll = function(el){
	var list = el.find('input');
	var len=0;
	list.each(function(i,n){
		if($(n).attr('checked')){
			len+=1;
		}
	})
	if(len){
		list.each(function(i,n){
			if($(n).attr('checked')){
				$(n).attr('checked',false);
			}
		})
	}else{
		list.each(function(i,n){
			$(n).attr('checked',true);
		})
	}
}
/*
	getChecked 获取被选项目
	el 元素(jquery)
*/
Asc.tree.moveitem.prototype.getChecked = function(el){
	var list = el.find('input');
	var obj = {};
	var num=0;
	var l = [];
	list.each(function(i,n){
		if($(n).attr('checked')){
			num+=1;
			l.push($(n).parent().attr('val'));
		}
	})
	obj.num=num;
	obj.list=l;
	return obj;

}
Asc.tree.moveitem.prototype.getTarget = function(e,elname){
	var find  = false;
	if(e.target.nodeName.toLowerCase()==elname){
		find = e.target;
	}else if(e.target.parentNode.nodeName.toLowerCase()==elname){
		find = e.target.parentNode;
	}
	return find;
}
Asc.tree.moveitem.prototype.moveitem = function(dir){
	var s = this.selected;
	if(s in this.rightarr){
		index = this.getIndex(s,this.rightarr);
		len = this.getLength(this.rightarr);
		if(dir=='up'){
			if(len>1&&index!=0){
				this.rightarr = this.objPre(s,this.rightarr);
				this.rightlist.html(this.createHtml(this.rightarr,2));
			}
		}else if(dir=='down'){
			if(len>1&&index!=len-1){
				this.rightarr = this.objNext(s,this.rightarr);
				this.rightlist.html(this.createHtml(this.rightarr,2));
			}
		}else if(dir=='bottom'){
			if(len>1&&index!=len-1){
				this.rightarr = this.objEnd(s,this.rightarr);
				this.rightlist.html(this.createHtml(this.rightarr,2));
			}
		}else if(dir=='top'){
			if(len>1&&index!=0){
				this.rightarr = this.objFirst(s,this.rightarr);
				this.rightlist.html(this.createHtml(this.rightarr,2));
			}
		}
	}
}
Asc.tree.moveitem.prototype.objEnd=function(key,obj){
	var len = this.getLength(obj);
	var tmp = {};
	tmp[key]=obj[key];
	delete obj[key];
    var tmp1 = {};
	for(var i=0;i<len;i++){
		if(i==len-1){
			tmp1[key]=tmp[key];
		}else{
			var iitemo = this.getItem(i,obj)
			tmp1[iitemo.key]=iitemo.val;
		}
	}
	return tmp1;
}
Asc.tree.moveitem.prototype.objFirst=function(key,obj){
	var tmp = {};
	tmp[key]=obj[key];
	delete obj[key];
	var tmp1 = {};
	tmp1[key]=tmp[key];
	for(var i in obj){
		tmp1[i] = obj[i];
	}
	return tmp1;
}
Asc.tree.moveitem.prototype.objPre=function(key,obj){
	var len = this.getLength(obj);
	var index = this.getIndex(key,obj);
	var pre = index-1;
	var tmps = {};
	tmps[key]=obj[key];
	var iitem = this.getItem(pre,obj);
	var tmp1 = {};
	for(var i=0;i<len;i++){
		if(i==pre){
			tmp1[key]=tmps[key];
		}else if(i==index){
			tmp1[iitem.key]=iitem.val;
		}else{
			var iitemo = this.getItem(i,obj)
			tmp1[iitemo.key]=iitemo.val;
		}
	}
	return tmp1;
}
Asc.tree.moveitem.prototype.objNext=function(key,obj){
	var len = this.getLength(obj);
	var index = this.getIndex(key,obj);
	var pre = index+1;
	var tmps = {};
	tmps[key]=obj[key];
	var iitem = this.getItem(pre,obj)
	
	var tmp1 = {};
	for(var i=0;i<len;i++){
		if(i==pre){
			tmp1[key]=tmps[key];
		}else if(i==index){
			tmp1[iitem.key]=iitem.val;
		}else{
			var iitemo = this.getItem(i,obj)
			tmp1[iitemo.key]=iitemo.val;
		}
	}
	return tmp1;
}

Asc.tree.moveitem.prototype.resetSelect=function(){
	//if(this.selected in this.leftarr){
	//	this.leftlist.find("li[val='"+this.selected+"']").removeClass('selected');
	//}else if(this.selected in this.rightarr){
		this.rightlist.find("li[val='"+this.selected+"']").removeClass('selected');
	//}
}
Asc.tree.moveitem.prototype.getListValue=function(el){
	var list = el.find('li');
	var arr={};
	list.each(function(i,n){
		var e = $(n);
		arr[e.attr('val')]=e.text();
	})
	return arr;
}
Asc.tree.moveitem.prototype.getLength=function(obj){
	var n=0;
	for(var i in obj){
		n++;
	}
	return n;
}
Asc.tree.moveitem.prototype.getSelect=function(){
	var s= false;
	this.leftlist.find('li').each(function(i,n){
		if($(n).attr('class').indexOf('selected')!=-1){
			s=$(n).attr('val')
		}
	})
	this.rightlist.find('li').each(function(i,n){
		if($(n).attr('class').indexOf('selected')!=-1){
			s=$(n).attr('val')
		}
	})
	return s;
}
Asc.tree.moveitem.prototype.update=function(){
		this.leftlist.html(this.createHtml(this.leftarr,1));
		this.rightlist.html(this.createHtml(this.rightarr,2));
		
}
Asc.tree.moveitem.prototype.getItem=function(index,obj){
	var iitem = {key:false,val:false};
	if(this.getLength(obj)){
		var n = 0;
		for(var i in obj){
			if(n==index){
				iitem = {key:i,val:obj[i]};
				break;
			}
			n++
		}
	}
	return iitem;
}
Asc.tree.moveitem.prototype.getIndex=function(item,obj){
	var index=-1;
	var n=0;
	for(var i in obj){
		if(i==item){
			index=n;
			break;
		}
		n++
	}
	return index;
}
Asc.tree.moveitem.prototype.createHtml=function(obj,dir){
	str='';
	if(dir==2){
		for(var key in obj){
			if(key==this.selected){
				str+="<li val='"+key+"' class='selected'><input type='checkbox'/><span class='fl'>"+obj[key]+"</span></li>";
			}else{
				str+="<li val='"+key+"'><input type='checkbox'/><span class='fl'>"+obj[key]+"</span></li>";
			}
		}
	}else{
		for(var key in obj){
			str+="<li val='"+key+"'><input type='checkbox'/><span class='fl'>"+obj[key]+"</span></li>";
		}
	}
	return str;
}