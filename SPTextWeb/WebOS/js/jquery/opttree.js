Asc.tree = Asc.tree || {};

/*
	操作树
*/
Asc.tree.optTree = function(configs){
	var defaults = {
		cache:false,
		tiptext:'你已选择',
		editFn:function(){},
		delFn:function(){},
		addFn:function(){}
	}
	Asc.apply(this,configs,defaults);
	//数据
	this.pathData = {};
	this.data = {};
	this.search = {};
	this.cLevel=0;
	this.init();
}

Asc.apply(Asc.tree.optTree.prototype,{
	init:function(){ //初始化
		this.el = $('#'+this.el);
		this.el.addClass('asc-opt-tree');
		this.el.addClass('cc');
		this.top = $(
			'<div class="aot-top cc">'
				+'<div class="aot-topath"><span class="aot-cate">'+this.tiptext+'：</span></div>'
				+'<div class="aot-topsearch"><span class="bt2 add"><span>'
				+'<button type="button">添加</button>'
				+'</span></span><span class="mr5" style="margin-left:5px;">'
				+'<input class="input_wa" type="text" />'
				+'</span><span class="bt2 search"><span>'
				+'<button type="button">搜 索</button>'
				+'</span></span></div>'
			+'</div>'
		);
		this.path = this.top.find('.aot-topath:first');
		this.btn = this.top.find('.search:first');
		this.addBtn = this.top.find('.add:first');
		this.input = this.top.find('input:first');
		this.content = $('<div class="aot-content cc"></div>');

		this.el.append(this.top);
		this.el.append(this.content);
		this.updateList(this.url,'level='+(this.cLevel+1));
		//事件
		Asc.bind(this.content,'mouseover',this.showOperation,this);
		Asc.bind(this.content,'mouseout',this.hideOperation,this);
		Asc.bind(this.content,'click',this.updateContent,this);
		Asc.bind(this.path,'click',this.updateContent,this);
		Asc.bind(this.btn,'click',this.searchAction,this);
		Asc.bind(this.addBtn,'click',this.addFn,this);
	}
	,showOperation:function(e){//显示操作项
		var et = $(e.target);
		var t; 
		if(t=Asc.getTarget(et,'aot-list')){
			t.find('.opt:first').show();
		}
	}
	,hideOperation:function(e){//隐藏操作项
		var et = $(e.target);
		var t; 
		if(t=Asc.getTarget(et,'aot-list')){
			t.find('.opt:first').hide();
		}
	}
	,searchAction:function(){//搜索
		var val = this.input.val();
		if(val!=''){
			this.getData(this.url,'type=search&keyword='+val,function(data){
				this.content.html(this.createSearchList(data));
			},this);

			this.input.val('');
		}
	}
	,createSearchList:function(data){//创建搜索结果列表
		var str='';

	}
	,updateContent:function(e){//更新列表
		var et = $(e.target);
		var t;
		if(t=Asc.getTarget(et,'name')){
			var list=this.content.find('.aot-list');
			var p =t.parent();
			var index = list.index(p);
			this.pathData[this.cLevel]=this.data[this.cLevel][index];
			this.updatePath();
			if(this.pathData[this.cLevel]['son']){
				//if(this.data[this.cLevel+1]){
					//this.content.html(this.createList(this.data[this.cLevel+1]));
					//this.cLevel +=1;
				//}else{
					var item = this.data[this.cLevel][index];
					this.updateList(this.url,'level='+(this.cLevel+1)+'&iid='+item.iid);
				//}
			}else{
				list.removeClass('selected');
				p.addClass('selected');
			}
			
		}
		if(t=Asc.getTarget(et,'aot-pathitem')){
			var list = this.top.find('.aot-pathitem');
			var index = list.index(t);
			this.pathData = this.removeObj(index,this.pathData);
			if(this.data[index+1]){
				this.content.html(this.createList(this.data[index+1]));
				this.cLevel = index+1;
			}
			this.updatePath();
		}
		if(t=Asc.getTarget(et,'edit')){
			e.stopPropagation();
			var list = this.content.find('.aot-list');
			var index = list.index(t.parents('.aot-list'));
			this.editFn.call(this,index,this.data[this.cLevel][index]);

		}if(t=Asc.getTarget(et,'del')){
			e.stopPropagation();
			var list = this.content.find('.aot-list');
			var index = list.index(t.parents('.aot-list'));
			this.delFn.call(this,this.data[this.cLevel][index]);
		}
	}
	,delItem:function(data){//删除单项
		var index = this.data[this.cLevel].indexOf(data);
		if(index!=-1){
			this.data[this.cLevel].remove(data);
			this.content.find('.aot-list:eq('+index+')').remove();
		}

	}
	,updateItem:function(index,data){ //更新列表
		this.data[this.cLevel][index]=data;
		this.content.find('.aot-list:eq('+index+')').find('.name:first').text(data.text);
	}
	,addItem:function(data){//添加单项
		this.data[this.cLevel].push(data);
		var sty = ['aot-list','aot-list parent'];
		this.content.append('<div class="'+sty[data.son]+'"><span class="name">'+data.text+'</span><span class="opt"><a href="javascript:void(0);" class="edit">[编辑]</a><a href="javascript:void(0);" class="del">[删除]</a></span></div>');
	}
	,removeObj:function(keep,obj){//删除对象元素
		if(keep==0){
			return {};
		}else{
			var index=0;
			for(x in obj){
				if(index>=keep){
					delete obj[x];
				}
				index++;
			}
			return obj;
		}
	}
	,updatePath:function(){//更新导航
		var str='<span class="aot-cate">'+this.tiptext+'：</span>';
		var x;
		for(x in this.pathData){
			var item = this.pathData[x];
			str+='<span class="aot-pathitem"><a href="javascript:void(0);">'+item.text+'</a></span>';
		}
		this.path.html(str);
	}
	,updateList:function(url,post){//更新列表
		this.addLoading();
		this.getData(url,post,function(data){
			var str = this.createList(data.data);
			this.content.html(str);
			this.data[data.level]=data.data;
			this.cLevel=parseInt(data.level);
		},this);
	}
	,getData:function(url,post,successFn,scope){//请求数据
		scope.addLoading();
			$.ajax({
			type: "POST",
			url: url,
			data:post,
			cache:scope.cache,
			dataType:'json',
			success: function(data){
				successFn.call(scope,data);
				scope.removeLoading();
			},
			error:function(x,t,e){
				alert(t);
			}
		}); 
	}
	,addLoading:function(){//加载遮罩
		this.el.addClass('aot-loading');
	}
	,removeLoading:function(){//删除加载遮罩
		this.el.removeClass('aot-loading');
	}
	,createList:function(data){//创建列表html
		//{text:'安徽',pid:0,iid:'1',son:1}
		var str='';
		var sty = ['aot-list','aot-list parent'];
		for(var i=0,len=data.length;i<len;i++){
			var item = data[i];
			str+='<div class="'+sty[item.son]+'"><span class="name">'+item.text+'</span><span class="opt"><a href="javascript:void(0);" class="edit">[编辑]</a><a href="javascript:void(0);" class="del">[删除]</a></span></div>';
		}
		return str;
	}
})

//扩展
Asc.tree.optTreeList = function(configs){
	var defaults = {}
	Ext.apply(this,configs,defaults);
	//初始化
	Asc.tree.optTreeList.super.constructor.call(this);
}
Asc.extend(Asc.tree.optTreeList,{
	init:function(){
	
	}
	,createList:function(){
	
	}
});