$(document).ready(function(){
//生成菜单
	var data =[
		{
			title:'销售分析',
			list:[
				{id:'homeifr',name:'combox',url:'combox.html'},
				{id:'dsdsdsdsds',name:'详细信息的列表',url:'company.html'}
			]
		}
	]
	
	$('#menulist').html(createMenu(data)); //创建目录列表
	initMenu();
	expendMenu();
	$('#btn_menu_show').bind('click',function(){expendMenu(1)}); //添加展开动作
	$('#btn_menu_hide').bind('click',function(){expendMenu()}); //添加关闭动作
})

//创建目录
function createMenu(data){
	var str='';
	for(var i=0,len=data.length;i<len;i++){
		str+='<dt>'+data[i].title+'</dt>';
		var list = data[i].list;
		str+='<dd><ul>';
		for(var j=0,lenl=list.length;j<lenl;j++){
			str+='<li><a href="javascript:;" onclick="PW.Dialog({id:\''+list[j].id+'\',name:\''+list[j].name+'\',url:\''+list[j].url+'\'});return false;">'+list[j].name+'</a></li>';
		}
		str+='</dd></ul>';
	}
	return str;
}
//展开关闭菜单
function expendMenu(show){
	show = show || 0;
	if(show){
		$('#menulist dd').each(function(i,n){
			$(n).show();
			$(n).prev('dt').removeClass('show');
		})
	}else{
		$('#menulist dd').each(function(i,n){
			$(n).hide();
			$(n).prev('dt').addClass('show');
		})
	}
}
//初始化目录
function initMenu(){
	var menu_level1 = $('#menulist dt');
	menu_level1.each(function(i,n){
		$(n).bind('click',function(){
			var item = $(n).next('dd');
			var sty = item.css('display');
			if(sty=='none'){
				$(n).removeClass('show');
				item.show();
			}else{
				$(n).addClass('show');
				item.hide();
			}
		})
	})
}


function createPath(arr){
	var str='<div>当前位置: ';
	for(var i=0,len=arr.length;i<len;i++){
		if(i==len-1){
			str+='<span class="admenu_down"  depth="2">'+arr[i]+'</span><span></span></div>'
		}else{
			str+='<span class="admenu_down"  depth="'+i+'">'+arr[i]+'</span><span> » </span>'
		}
	}
	return str;	
}


function setCookie(name, value, expires, path, domain, secure) {
	if (!expires) {
		expires = new Date();
		expires.setTime(expires.getTime()+31536000000);
	}
	document.cookie = name + '=' + escape(value) +
		( (expires) ? ';expires=' + expires.toGMTString() : '') +
		'path=' + ( (path) ? path : '/') +
		( (domain)  ? ';domain=' + domain : '') +
		( (secure)  ? ';secure' : '');
}
