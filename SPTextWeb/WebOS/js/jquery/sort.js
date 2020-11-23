Asc.sort = Asc.sort || {};
/*
	Asc.sort.moveitem	对象
	cid		组件id(带#)
	width	宽度
	height	高度
	leftr	移动时是否移除左边列表
	获取参数
		getParams(dir),dir=1或2(1左框,2右框)
*/
Asc.sort.sortItem = function(configs){
	var defaults = {
		delFn:function(){}
	}
	Asc.apply(this,configs,defaults);
	this.el  = $('#'+this.el);
	this.step = this.el.find('.aom-stepname:first');
	this.list = this.el.find('.aom-listname:first');
	this.btnTop = this.el.find('.btn_top:first');
	this.btnUp = this.el.find('.btn_up:first');
	this.btnDown = this.el.find('.btn_down:first');
	this.btnBottom = this.el.find('.btn_bottom:first');
	//this.btnAdd = this.el.find('.bt2:first');

	this.init();
}
Asc.apply(Asc.sort.sortItem.prototype,{
	init:function(){

		//Asc.bind(this.btnAdd,'click',this.addItem,this);
		Asc.bind(this.btnTop,'click',this.moveItem,this,['top']);
		Asc.bind(this.btnUp,'click',this.moveItem,this,['up']);
		Asc.bind(this.btnDown,'click',this.moveItem,this,['down']);
		Asc.bind(this.btnBottom,'click',this.moveItem,this,['bottom']);
		Asc.bind(this.list,'click',this.listFn,this);
	},
	addItem:function(){
		//pass
	},
	moveItem:function(e,dir){
		switch (dir){
			case 'top':
				this.toTop();
				break;
			case 'up':
				this.toUp();
				break;
			case 'down':
				this.toDown();
				break;
			case 'bottom':
				this.toBottom();
				break;
			default:
				break;
		}
	},
	toTop:function(){
		var s = this.getSelected();
		var items = this.getItems();
		if(s){
			var index = items.indexOf(s[0]);
			if(index!=0){
				this.list.prepend(s);
			}
		}
	},
	toUp:function(){
		var s = this.getSelected();
		var items = this.getItems();
		if(s){
			var index = items.indexOf(s[0]);
			if(index!=0){
				s.prev().before(s);
			}
		}
	},
	toDown:function(){
		var s = this.getSelected();
		var items = this.getItems();
		if(s){
			var index = items.indexOf(s[0]);
			if(index!=items.length-1){
				s.next().after(s);
			}
		}
	},
	toBottom:function(){
		var s = this.getSelected();
		var items = this.getItems();
		if(s){
			var index = items.indexOf(s[0]);
			if(index!=items.length-1){
				this.list.append(s);
			}
		}
	},
	getItems:function(){
		var arr = [];
		this.list.find('.aom-listi').each(function(i,n){
			arr.push(n);
		})
		return arr;
	},
	getSort:function(){
		var items = this.getItems();
		var arr = [];
		for(var i=0,len=items.length;i<len;i++){
			arr.push($(items[i]).attr('val'));
		}
		return arr;
	},
	getSelected:function(){
		var s = this.list.find('.selected:first');
		if(s.length){
			return s;
		}
		return false;
	},
	getTartget:function(et,cls,depth){
		depth = depth || 5;
		if(et.hasClass(cls)){
			return et;
		}else{
			for(var i=0;i<depth;i++){
				et = et.parent();
				if(et.hasClass(cls)){
					return et;
				}
			}
		}
		return null;
	},
	listFn:function(e){
		var et = $(e.target);
		if(et.attr('class').toLowerCase().indexOf('del')!=-1){
			e.stopPropagation();
			var key = et.parents('.aom-listi').attr('val');
			this.delFn.call(this,key);
		}else{
			var p = this.getTartget(et,'aom-listi');
			if(p){
				p.addClass('selected').siblings().removeClass('selected');
			}
		}
	}
})