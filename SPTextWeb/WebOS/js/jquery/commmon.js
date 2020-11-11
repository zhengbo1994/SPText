//ready
$(document).ready(function(){
})
/*
  asc空间
*/
asc={
	gDate:function(str){
        var date = new Date(str);
        this.lastmodify=date.getFullYear()+"-"+date.getMonth()+"-"+date.getDate();
	},
    gettime:function(){
        return (new Date()).getTime();
    }
};

asc.table={
    single:function(tid,s,oe){
        if(oe=="odd"){
            $(tid+" tr:"+oe).addClass(s);
        }else{
            $(tid+" tr").each(function(i,n){
                if(i!=0&&(i%2)==0) {
                    $(n).addClass(s);
                }
            })
        }
    }
}
asc.time= {
    localtime:function(llang) {
        var date = new Date();
        var tz = -(date.getTimezoneOffset()/60);
        tz=tz>0?"+"+tz:tz;
        var tz =" (GMT"+tz+")";
        var thour = date.getHours();
        var tmin = date.getMinutes();
        tmin= tmin<10?"0"+tmin:tmin;
        var apm = thour>=12?"pm":"am";
        thour = thour<10?"0"+thour:thour;
        var langarr={
            zh:{am : "上午", pm : "下午"},
            en:{am : "AM", pm : "PM"}
        }
        return langarr[llang][apm]+" "+thour+"："+tmin+tz;
    }
}


/*
 tab函数
 a 链接
 c 内容
 s 当前样式
 e 绑定时间
 fn callback函数
*/
asc.tab_nav=function(a,c,s,e,fn) {
 	var list = $(c);
	var links = $(a);
    var lz = list.length;
    var lz1 = links.length;
    var more=(lz1>lz)?true:false;
    var currentt=0;
	$.each(links,function(i,n){ 
         if($(n).hasClass(s)) {
            currentt=i;
          }
	})
	$.each(list,function(i,n){ 
            if(i==currentt) {
                $(n).css("display","block");
            }
            else {
                $(n).css("display","none");
            }
	})
	function closenac() {
		$.each(list,function(i,n){
			$(n).css("display","none");
            $(links[i]).removeClass(s);
		})
		$.each(links,function(i,n){
            $(n).removeClass(s);
		})
	}
	function showall() { 
		$.each(list,function(i,n){
			$(n).css("display","block");
            $(links[i]).removeClass(s);
		})
	}
	$.each(links,function(i,n){ 
		$(n).bind(e,(function() {
           if(i<lz){
			closenac();
			//$(list[i]).fadeIn("slow");
			$(list[i]).css("display","block");
			$(n).addClass(s);
            $(n).blur();
			return false;
            if(fn) {
                (fn)();
            }
          }
           if(more&&i>=lz){
                showall();
                $(n).addClass(s);
                $(n).blur();
                if(fn) {
                    (fn)();
                }
           }
		}))
	});
    //callback
    if(fn) {
        (fn)();
    }
 }
/*
	hover函数
	eid 元素id
	s 样式
*/
 asc.hover=function(eid,s){
    $(eid).each(function(i,n){
        $(n).hover(function(){
              $(this).addClass(s);
        },function(){
              $(this).removeClass(s);
        })
    })
 }

/*
	显示隐藏层
	eid 元素id
	s 0/1
*/
asc.shows = function(eid,s){
	e = $(eid);
	show = e.css('display')=='none'?0:1;
	if(show!=s){
		if(s==1){
			e.css('display','');
		}else{
			e.css('display','none');
		}
	}
}
asc.show = function(eid){
	e = $(eid);
	show = e.css('display')=='block'?true:false;
	if(!show){
		e.show();
	}
}
asc.hide = function(eid){
	e = $(eid);
	show = e.css('display')=='block'?true:false;
	if(show){
		e.hide();
	}
}
/*
添加删除行
*/

asc.addRec = function(tpl,pid){
	last = $("#"+pid+" tr:last-child");
	n = $("#"+tpl).clone(true);
	n.css('display',"");
	n.attr('id','')
	last.before(n);
}

asc.delRec = function(pid,o){
	s = $('#'+pid+' tr');
	if(s.length>2){
		$(o).parents('tr').remove();
	}
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