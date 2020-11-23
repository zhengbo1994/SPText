Asc.form = Asc.form || {};
/*
	combox下拉菜单
*/
Asc.form.combox = function(config){
	defaults = {
		name:'combox',
		width:154,
		listWidth:154,
		emptyText:'请选择……',
		postParams:false,
		clickRequest:false,
		initKey:false,
		data:{key:'key',text:'text'},
		dataType:'local'
	}
	$.extend(this,defaults,config);
	this.Init();
}

/*
	初始化
*/
Asc.form.combox.prototype.Init = function(){
	this.requested = false;
	this.el = $(this.el);
	this.input = $('<input type="hidden" name="'+this.name+'"/>');
	this.text = $('<div class="afc-text">'+this.emptyText+'</div><b class="afc-down"></b>');
	this.el.css('width',this.width+'px');
	this.el.addClass('asc-form-combox');
	this.showText = $(this.text[0]);
	this.icon = $(this.text[1]);
	this.list = $('<ul class="afc-list"></ul>');
	this.list.css('width',this.listWidth+'px');
	this.list.addClass('afc-loading');
	this.list.hide();

	this.el.append(this.text);
	this.el.append(this.input);
	this.el.append(this.list);

	this.el.hover(function(){
		this.el.css('border-color','#23a6dd');
		this.icon.addClass('afc-downhover');
	}.createDelegate(this),function(){
		this.el.css('border-color','#e1e1e1');
		this.icon.removeClass('afc-downhover');
	}.createDelegate(this))
	
	Asc.bind(this.el,'click',this.showList,this,[]);
	Asc.bind(this.list,'click',this.selectItem,this);
	Asc.bind($(document),'click',function(e){
		if(this.list.css('display')=='block'){
			this.list.hide();
		}
	},this);
	if(this.dataType=='local'){
		this.list.removeClass('afc-loading');
		this.list.append(this.createList(this.data));
		this.initCompText(this.data);

	}else{
		this.sendRequest(true);
	}
	
}
/*
	显示列表
*/
Asc.form.combox.prototype.showList = function(e){
	e.stopPropagation();
	if(this.clickRequest){
		this.sendRequest();
	}else{
		this.list.show();
	}
}
/*
	响应选择动作
*/
Asc.form.combox.prototype.selectItem = function(e){
	e.stopPropagation();
	if(e.target.nodeName.toLowerCase()=='li'){
		this.input.val($(e.target).attr('keyattr'));
		$(e.target).addClass('selected').siblings().removeClass('selected');
		this.showText.text($(e.target).text());
		this.showText.attr('keyattr',$(e.target).attr('keyattr'));
		this.list.hide();
	}
}
/*
	创建列表
*/
Asc.form.combox.prototype.createList = function(data){
	//{key:'12',text:'安徽省'}
	var str='';
	var sty = '';
	for(var i=0,len=data.length;i<len;i++){
		if(i==len-1){
			sty = ' class="last"';
		}
		str+='<li keyattr="'+data[i].key+'" '+sty+'>'+data[i].text+'</li>';
	}
	return str;
}
/*
	发送请求
*/
Asc.form.combox.prototype.sendRequest = function(init){
	init = init || false;
	if(init){
		this.list.hide();
	}else{
		this.list.show();
	}
	$.ajax({
	   type: "POST",
	   url: this.url,
	   data: this.postParams,
	   dataType:'json',
	   success: function(data){
		 this.requested =true;
		 this.list.html('');
		 this.list.removeClass('afc-loading');
		 this.list.append(this.createList(data.data));
		 if(init){
			 this.initCompText(data.data);
		 }
		 var val = this.showText.attr('keyattr');
		 if(val){
			 this.list.find('li:[keyattr="'+val+'"]').addClass('selected');
		 }
	   }.createDelegate(this),
	  error:function(x,t,e){
		 alert(t);
	  }
	});
}
/*
	初始化文本
*/
Asc.form.combox.prototype.initCompText = function(data){
	for(var i=0,len=data.length;i<len;i++){
		if(data[i].key==this.initKey){
			this.showText.text(data[i].text);
			this.showText.attr('keyattr',data[i].key);
			this.input.val(this.initKey);
		}
	}
}