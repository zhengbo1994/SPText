Asc.windows = Asc.windows || {};
/*
	windows 窗口类
	待:移动 resize事件,自动产生id
*/
Asc.windows.base = function(configs){
	var defaults = {
		width:480,
		height:150,
		buttonAlign:'right'
	};
	$.extend(this,defaults,configs);
	this.init();
}
Asc.windows.base.id = function(prefix){
	prefix = prefix || 'aid-';
	var idSeed  =0;
	return function(){
		idSeed++;
		return prefix+idSeed;
	}
}
/*
	初始化
*/
Asc.windows.base.prototype.init = function(){
	this.closeEvent = [];
	this.closeEventScop = [];
	this.createWin();
	this.createMask();
	if(this.ie6()){
		this.hideSelect();
	}
	$('body').append(this.mask);
	$('body').append(this.win);
	this.mask.show();
	this.win.show();
}
/*
	生成id
*/
Asc.windows.base.prototype.getId = Asc.windows.base.id();
/*
	创建窗口
*/
Asc.windows.base.prototype.createWin = function(){
	var align = {left:'tal',center:'tac',right:'tar'};
	this.win = $('<div class="asc-win" id="'+this.getId()+'"><div class="asc-win-i"></div></div>');
	this.win.hide();
	this.wini = this.win.find('div:first');
	this.topw = $('<div class="asc-win-t"><span class="fl title">'+this.title+'</span><a class="del_img fr close" href="javascript:;"></a></div>');
	this.container  = $('<div class="asc-win-c"><div class="asc-win-cc"></div></div>');
	this.containeri = this.container.find('div:first');

	//样式
	if(this.top){
		if(this.ie6()){
			this.top=this.top-($(window).height()/2)+$('html').scrollTop();
		}else{
			this.top = this.top-$(window).height()/2;
			this.top = this.top>0 ? 0 : this.top;
		}
	}else{
		if(this.ie6()){
			this.top = -((this.height+12)/2)+$('html').scrollTop();
		}else{
			this.top = -((this.height+12)/2);
		}
	}
	
	this.win.css({height:this.height+'px',width:this.width+'px','margin-top':this.top+'px','margin-left':-((this.width+2)/2)+'px'});
	var ch = this.buttons?(this.height-28-32):(this.height-28);
	this.container.css('height',ch+'px');

	//内容
	if(this.el){
		this.content = $('#'+this.el).html();
		$('#'+this.el).empty();
	}else if(this.body){
		this.content = this.body;
	}else if(this.iframe){
		var w = this.width-10;
		this.content = '<iframe frameborder="0"  hspace="0" src="'+this.iframe+'" name="iframe'+(new Date()).getTime()+'" id="iframe'+(new Date()).getTime()+'id" scrolling="yes" style="overflow:visible;height:'+(ch-10)+'px;width:'+w+'px;"></iframe>';
	}else if(this.load){
		this.content = $('');
		this.container.addClass('loading');
		this.containeri.load(this.load.url,this.load.data,function(){
			this.load.callback.createDelegate(this);
			this.container.removeClass('loading');
		}.createDelegate(this));
	}

	//组装
	
	
	if(this.ie6()&&this.iframe){
		setTimeout(function(){
			this.containeri.append(this.content);
		}.createDelegate(this),0);
	}else{
		this.containeri.append(this.content);
	}
	this.wini.append(this.topw);
	this.wini.append(this.container);
	if(this.buttons){
		this.bottom = $('<div class="asc-win-b '+align[this.buttonAlign]+'"></div>');
		this.createButton();
		this.wini.append(this.bottom);
	}

	//绑定事件
	this.bindEvents();
}
/*
	创建遮罩
*/
Asc.windows.base.prototype.createMask = function(){
	this.mask = $('<div class="asc-overlay"></div>');
	this.mask.hide();
	if(this.ie6()){
		this.mask.height($(window).height());
		this.mask.width($(window).width());
	}
}
/*
	创建按钮
*/
Asc.windows.base.prototype.createButton = function(){
	for(var i=0,len=this.buttons.length;i<len;i++){
		var item = this.buttons[i];
		var button = $('<span class="'+item.cls+'"><span><button type="submit">'+item.text+'</button></span></span>');
		Asc.bind(button,'click',item.fn,this);
		this.bottom.append(button);
	}
}
/*
	关闭事件绑定
*/
Asc.windows.base.prototype.bindClose = function() {
	var closelist = this.containeri.find('.close');
	for(var i=0,len=closelist.length;i<len;i++){
		var item = closelist[i];
		Asc.bind($(item),'click',function(){this.closeWindow()},this);
	}
}
/*
	添加关闭事件
*/
Asc.windows.base.prototype.addCloseEvent = function(fn,scope) {
	this.closeEvent.push(fn);
	this.closeEventScop.push(scope);
}
/*
	绑定事件
*/
Asc.windows.base.prototype.bindEvents = function() {
	var btn_close = this.topw.find('.close:first');
	Asc.bind(btn_close,'click',function(){this.closeWindow()},this);
}
/*
	关闭
*/
Asc.windows.base.prototype.closeWindow = function() {
	if(this.closeEvent.length){
		for(var i=0,len=this.closeEvent.length;i<len;i++){
			this.closeEvent[i].call(this.closeEventScop[i]);
		}
	}
	this.el?$('#'+this.el).append(this.containeri.html()):false;
	this.win.unbind();
	this.mask.unbind();

	this.win.remove();
	this.mask.remove();
	if(this.ie6()){
		this.showSelect();
	}
}
/*
	ie6判断
*/
Asc.windows.base.prototype.ie6 = function() {
	return (typeof document.body.style.maxHeight === "undefined") ? true:false;
}
/*
	隐藏select
*/
Asc.windows.base.prototype.hideSelect = function() {
	$('select').each(function(i,n){
		$(n).css('visibility','hidden');
	})
}
/*
	显示select
*/
Asc.windows.base.prototype.showSelect = function() {
	$('select').each(function(i,n){
		$(n).css('visibility','visible');
	})
}
/*
	show 信息展示
	alert 提示
	prompt 写入框
	comfirm 确认框
	icon类配置
*/
/*
	alert提示类
*/
Asc.windows.alert = function(title,tipcontent,fn){
	fn = fn || function(){};
	var configs = {
		'title':title,
		body:'<div class="asc-win-tip cc"><div class="asc-win-icon"></div><div class="asc-win-text">'+tipcontent+'</div></div>',
		width:500,
		height:150,
		buttons:[
			{
				text:'确定',
				cls:'bt2',
				fn:function(){
					this.closeWindow();
					fn.apply(this);
				}
			}	
		]
	}
	var alert = new Asc.windows.base(configs);
	return alert;
}
/*
	确认类
*/
Asc.windows.confirm = function(title,tipcontent,fn){
	fn = fn || function(){};
	var configs = {
		'title':title,
		body:'<div class="asc-win-tip cc"><div class="asc-win-icon"></div><div class="asc-win-text">'+tipcontent+'</div></div>',
		width:500,
		height:150,
		buttons:[
			{
				text:'确定',
				cls:'bt2',
				fn:function(){
					//this.closeWindow();
					fn.apply(this,['yes']);
				}
			},{
				text:'取消',
				cls:'bt2',
				fn:function(){
					//this.closeWindow();
					fn.apply(this,['no']);
				}
			}	
		]
	}
	var confirm = new Asc.windows.base(configs);
	return confirm;	
}
/*
	输入框
*/
Asc.windows.prompt = function(title,tipcontent,fn){
	fn = fn || function(){};
	var configs = {
		'title':title,
		body:'<div class="asc-win-tip cc"><p class="asc-win-label">'+tipcontent+'</p><input type="text" class="input_wb" /></div>',
		width:500,
		height:150,
		buttons:[
			{
				text:'确定',
				cls:'bt2',
				fn:function(){
					//this.closeWindow();
					var i = this.win.find('input:first');
					var val= i.val();
					fn.apply(this,['yes',val]);
				}
			},{
				text:'取消',
				cls:'bt2',
				fn:function(){
					//this.closeWindow();
					fn.apply(this,['no']);
				}
			}	
		]
	}
	var prompt = new Asc.windows.base(configs);
	prompt.win.find('input:first').focus();
	return prompt;	
}