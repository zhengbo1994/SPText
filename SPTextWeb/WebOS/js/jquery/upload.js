Asc.upload = Asc.upload || {};
/*
	------------------
	文件上传类
	bcpxqz@126.com
*/
Asc.upload.uploadHander = function(config){
	var defaults = {
		flash:{
			flash_url:'js/jquery/swfupload/swfupload.swf',
			upload_url:'upload.php',
			//post_params:{'author':'zhangqixin'},
			custom_settings:{
				progressTarget:'procontainer',
				cancelButtonId:'btn-cancel'
			},
			debug:false,
			//file
			file_post_name:'fileinputname',
			file_size_limit:'10MB',
			file_types:'*.xls;*.xlsx;*.xml',
			file_types_description:'Excel/xml文件',
			file_upload_limit:0,
			file_queue_limit:20,
			//button
			button_image_url:'images/btn_addupload.gif',
			button_width:68,
			button_height:21,
			button_cursor:SWFUpload.CURSOR.HAND
		},
		file_types:'png|jpg',
		file_name:'file',
		upload_url:'upload.php',
		method:'POST',
		file_queue_limit:5
	}
	$.extend(this,defaults,config);
	this.init();

}
/*
	生成动态id
*/
Asc.upload.uploadHander.id = function(prefix){
	prefix = prefix || 'aid-';
	var idSeed  =0;
	return function(){
		idSeed++;
		return prefix+idSeed;
	}
}

/*
	init 初始化
*/
Asc.upload.uploadHander.prototype.init = function(){
	this.id = this.getId();
	var fla = this.getFlashVersion();
	if(fla.f==1&&fla.v>=9){
		this.flash['button_placeholder_id']= this.id+'addBtn';
		this.createComp(1);
		$.extend(this.flash,this.createFlashHandler());
		this.createSWFUpload();
		this.upType = 1;
	}else{
		this.createComp(2);
		this.upType = 2;
	}
}
/*
	生成id
*/
Asc.upload.uploadHander.prototype.getId = Asc.upload.uploadHander.id('upload-file-id');
/*
	createComp 生成公共组件
*/
Asc.upload.uploadHander.prototype.createComp = function(type){
	this.startBtn = $('<span class="btn2"><span><button type="submit">开始上传</button></span></span>');
	this.reStartBtn = $('<span class="btn2"><span><button type="submit">继续上传</button></span></span>');
	this.cancelBtn = $('<span class="btn2"><span><button type="submit">取消上传</button></span></span>');
	this.clearAllBtn = $('<div class="comp-swf-btn"><a href="javascript:void(0);">清空列表</a></div>');
	this.el = $('<div class="comp-swf-upload" id="'+this.id+'"></div>');
	this.top = $('<div class="comp-swf-utool"></div>');
	this.fileList = $('<div class="comp-swf-list"></div>');
	this.bottom = $('<div class="comp-swf-ctrl"></div>');
	if(type==1){
		this.addBtn = $('<div class="comp-swf-btn"><span id="'+this.id+'addBtn"></span></div>');
		this.uploadTip = $('<div class="comp-swf-total"><span>上传总进度:</span><span class="comp-swf-proc"><span class="comp-swf-prob" style="width:0%;"></span></span><span class="comp-swf-pron">0%</span><span>，已上传<strong class="comp-swf-prou">0</strong>，共<strong class="comp-swf-prot">0</strong>，共<strong>0</strong>个文件</span></div>');
		this.bind(this.clearAllBtn,'click',this.removeAllFiles,this);
		this.bind(this.startBtn,'click',this.stratUploadFiles,this);
		this.bind(this.reStartBtn,'click',this.reStartBtnFiles,this);
		this.bind(this.cancelBtn,'click',this.cancelUploadFiles,this);

		this.top.append(this.addBtn);
		this.top.append(this.clearAllBtn);
		this.bottom.append(this.startBtn);
		this.bottom.append(this.reStartBtn);
		this.reStartBtn.hide();
		this.bottom.append(this.cancelBtn);
		
		this.el.append(this.top);
		this.el.append(this.fileList);
		this.el.append(this.uploadTip);
		this.el.append(this.bottom);
	}else if(type==2){
		this.addBtn = $('<div class="comp-swf-btn add"><span class="btn2"><input type="file" name="'+this.file_name+'" /><span><button type="submit">添加文件</button></span></span></div>');
		this.uploadTip = $('<div class="comp-swf-total"><span>已上传<strong class="comp-swf-prou">0</strong>个文件，共选择<strong class="comp-swf-prot">0</strong>个文件</span></div>');
		this.errorTip = $('<div class="comp-swf-listip s1"></div>');
		this.files = [];
		this.filesId = [];
		this.uploadFiles = [];
		this.htmlTotalFiles=0;
		this.input = this.addBtn.find('input:first');

		this.bind(this.input,'change',this.onInputChange,this);
		this.bind(this.fileList,'click',this.delteItemFile,this);
		this.bind(this.clearAllBtn,'click',this.delteAllFile,this);
		this.bind(this.startBtn,'click',this.startUploadHtmlFile,this);
		this.bind(this.cancelBtn,'click',this.cancelUploadHtmlFile,this);
		this.bind(this.reStartBtn,'click',this.reStartUploadHtmlFile,this);
		
		this.top.append(this.addBtn);
		this.top.append(this.clearAllBtn);
		this.bottom.append(this.startBtn);
		this.bottom.append(this.reStartBtn);
		this.reStartBtn.hide();
		this.errorTip.hide();
		this.bottom.append(this.cancelBtn);
		this.fileList.append(this.errorTip);
		
		this.el.append(this.top);
		this.el.append(this.fileList);
		this.el.append(this.uploadTip);
		this.el.append(this.bottom);

	}else if(type==3){

	}
	$(this.pid).append(this.el);
}
Asc.upload.uploadHander.prototype.closeUpload = function(){
	if(this.upType == 1){
		this.removeAllFiles();
	}else{
		this.delteAllFile();
	}
}
/*
	------------------------
	普通html上传方式
*/
/*
	添加普通文件
*/
Asc.upload.uploadHander.prototype.onInputChange = function(){
	if(this.addBtn.find('.btn2').length){
		var filename = this.input.val();
		filename = filename.replace(/.*\\/,'');
		if(this.checkExt(filename)){
			if(this.aindexOf(filename,this.files)==-1){
				this.files.push(filename);
				if(this.filesId.length>=this.file_queue_limit){
					this.showTip('文件数超过'+this.file_queue_limit+'选择的文件未添加到列表');
					this.fileList.scrollTop(0);
				}else{
					this.htmlTotalFiles+=1;
					this.addToFilelist(filename);
					this.updateHtmlTips();
				}
				
			}
			else{
				this.showTip(filename+'：文件名重复');
			}
		}else{
			this.showTip(filename+':文件格式不符合规范，可选格式 '+this.file_types.replace('\|','，'));
		}
	}
}
/*删除单个文件*/
Asc.upload.uploadHander.prototype.delteItemFile = function(e){
	if(e.target.nodeName.toLowerCase()=='a'){
		var id = $(e.target).parent().attr('id');
		this.delteFileAction(id);
		if(this.form&&this.form.attr('id').toLowerCase()==id+'f'){
			if(this.iframe&&this.form){
				setTimeout(function(){
					this.iframe.remove();
					this.form.remove();
				}.createDelegate(this),0)
			}
		}
	}
	this.updateHtmlTips();
}
/*删除所有文件*/
Asc.upload.uploadHander.prototype.delteAllFile = function(e,a){
	e = e || 'all';
	a = a || 'all';
	if((typeof e).toLowerCase() == 'string'){
		a=e;
	}
	this.addBtn.children('span:first').removeClass().addClass('btn2');
	this.input.show();
	this.delteFileAction(a);
	this.updateHtmlTips();
	if(this.iframe&&this.form){
		setTimeout(function(){
			this.iframe.remove();
			this.form.remove();
		}.createDelegate(this),0)
	}
}
Asc.upload.uploadHander.prototype.delteFileAction = function(id){
	if(id=='suc'){
		for(var i=0,len=this.filesId.length;i<len;i++){
			$('#'+this.filesId[i]).remove();
			$('#'+this.filesId[i]+'input').remove();
		}
		this.files = [];
		this.filesId = [];
		this.uploadFiles = [];
		this.htmlTotalFiles = 0;
	}else if(id=='all'){
		this.files = [];
		this.filesId = [];
		this.uploadFiles = [];
		this.htmlTotalFiles = 0;
		this.fileList.html('');
		this.fileList.append(this.errorTip);
		this.updateHtmlTips();
	}else{
		var index = this.aindexOf(id,this.filesId);
		this.removeByIndex(index,this.files);
		this.removeByIndex(index,this.filesId);
		this.htmlTotalFiles-=1;
		$('#'+id).remove();
		$('#'+id+'i').remove();
	}
}
/*更新提示*/
Asc.upload.uploadHander.prototype.updateHtmlTips = function(){
	this.uploadTip.find('strong:eq(0)').text(this.uploadFiles.length);
	this.uploadTip.find('strong:eq(1)').text(this.htmlTotalFiles);
}
/*停止上传*/
Asc.upload.uploadHander.prototype.stopUploadFile = function(){
	
}
/*开始上传*/
Asc.upload.uploadHander.prototype.startUploadHtmlFile = function(){
	var len = this.files.length;
	if(len==0&&this.htmlTotalFiles==0){
		this.showTip('请选择文件');
		return false;
	}
	if(len){
		var id = this.filesId[0];
		this.startBtn.hide();
		//修改样式
		for(var i=0;i<len;i++){
			var icon = $('#'+this.filesId[i]).find('b:first');
			var state = $('#'+this.filesId[i]).find('.comp-swf-lstate:first');
			if(i==0){
				icon.removeClass();
				icon.addClass('comp-swf-files');
				icon.addClass('uploading');
				state.text('正在上传');
			}else{
				icon.removeClass();
				icon.addClass('comp-swf-files');
				icon.addClass('uploadwait');
				state.text('等待上传');
			}
		}
		//禁用按钮
		this.addBtn.children('span:first').removeClass().addClass('bt2');
		if(this.input.val()){
			var temp = $('<input type="file" name="'+this.file_name+'" id="'+(new Date()).getTime()+'"/>');
			this.input.before(temp);
			this.input.unbind('click');
			this.input.remove();
			this.input = temp;
			this.bind(this.input,'change',this.onInputChange,this);
		}
		this.input.hide();
		//开始上传
		this.form = this.createForm(id);
		this.iframe = this.createIframe(id);
		this.form.append($('#'+id+'input'));
		this.el.append(this.iframe);
		this.el.append(this.form);
		this.form.submit();
		this.bind(this.iframe,'load',this.iframeLoad,this,[id]);
	}else{
		this.updateHtmlTips();
		this.showTip('上传成功');
		this.reStartBtn.show();
		this.startBtn.hide();
		return false;
	}
}
/*重新上传*/
Asc.upload.uploadHander.prototype.reStartUploadHtmlFile = function(){
	this.delteAllFile('all');
	this.reStartBtn.hide();
	this.startBtn.show();
}
/*取消上传*/
Asc.upload.uploadHander.prototype.cancelUploadHtmlFile = function(){
	this.delteAllFile('suc');
	this.startBtn.show();
	this.reStartBtn.hide();
}
/*
	上传状态
*/
Asc.upload.uploadHander.prototype.iframeLoad = function(e,id){
	var item = $('#'+id);
	var res = '';
	if(this.iframe[0].contentWindow){
		res = $(this.iframe[0].contentWindow.document.body).text();
	}else{
		res = $(this.iframe[0].contentDocument.body).text();
	}
	if(res=='ok'){
		this.uploadFiles.push(id);
		this.updateHtmlTips();
		item.find('.comp-swf-lstate:first').text('上传成功 ');
		var icon = item.find('b:first');
		icon.removeClass();
		icon.addClass('comp-swf-files');
		icon.addClass('uploadok');
		item.find('a:first').hide();
		var offset = this.uploadFiles.length*65;
		this.fileList.scrollTop(offset);
	}else{
		item.find('.comp-swf-lstate:first').text('上传失败'+res);
	}
	setTimeout(function(){
		this.iframe.remove();
		this.form.remove();
		this.startUploadHtmlFile();
	}.createDelegate(this),0)
	//删除数据
	this.filesId.splice(0,1);
	this.files.splice(0,1);
}
/*
	createForm 创建form
*/
Asc.upload.uploadHander.prototype.createForm = function(id){
	var form = $('<form action="'+this.upload_url+'" method="'+this.method+'" enctype="multipart/form-data" id="'+id+'f" target="'+id+'i" style="display:none;"></form>');
	return form;
}
/*
	createIframe 创建Iframe
*/
Asc.upload.uploadHander.prototype.createIframe = function(id){
	var iframe = $('<iframe  style="display:none;" src="javascript:false;" id="'+id+'i" name="'+id+'i"></iframe>');
	return iframe;
}

/*
	------------------
*/
Asc.upload.uploadHander.prototype.addToFilelist = function(filename){
	var id = (new Date()).getTime();	
	this.filesId.push(id);
	this.fileList.append('<div class="comp-swf-l" id="'+id+'"> <a href="javascript:void(0);">删除</a><div class="comp-swf-lname"><b class="comp-swf-files"></b><span>'+filename+'</span></div><div class="comp-swf-lstate">等待上传</div></div>');
	var input = this.input.clone();
	input.attr('id',id+'input');
	input.css('display','none');
	this.fileList.append(input);
	var offset = this.files.length*65;
	this.fileList.scrollTop(offset);
}
Asc.upload.uploadHander.prototype.showTip = function(str){
	this.errorTip.show().text(str);
	this.timer = setTimeout(function(){
		this.errorTip.slideUp();
		this.timer=null;
	}.createDelegate(this),3000);
}
Asc.upload.uploadHander.prototype.checkExt = function(filename){
	var arr = filename.split('.');
	var type  = arr[arr.length-1];
	var reg = new RegExp(this.file_types);
	if(!type.match(reg)){
		return false;
	}
	return true;
}


/*
	-------------------------------
	以下是flash上传的方法
	createSWFUpload 创建swfupload对象
*/
Asc.upload.uploadHander.prototype.createSWFUpload = function(){
	this.swfupload = new SWFUpload(this.flash);
	this.uploadFiles = [];
	this.uploadFilesHtml={};
	this.hasUploadBtyes=0;
	this.curBytes = 0;
}
/*
	createFlashHandler 创建swfupload监听函数
*/
Asc.upload.uploadHander.prototype.createFlashHandler =  function(){
	var obj = {
		file_queued_handler:this.fileQueued.createDelegate(this),
		file_queue_error_handler:this.fileQueuedError.createDelegate(this),
		file_dialog_complete_handler:this.fileDialogComplete.createDelegate(this),
		upload_start_handler:this.uploadStart.createDelegate(this),
		upload_progress_handler:this.uploadProgress.createDelegate(this),
		upload_error_handler:this.uploadError.createDelegate(this),
		upload_success_handler:this.uploadSuccess.createDelegate(this),
		upload_complete_handler:this.uploadComplete.createDelegate(this),
		queue_complete_handler:this.queueComplete.createDelegate(this)
	}
	return obj;
}


/*
	swfupload系列事件
*/
/*
	检查文件是否存在
*/
Asc.upload.uploadHander.prototype.checkExisit = function(file){
	if(this.uploadFiles){
		for(var i=0,len=this.uploadFiles.length;i<len;i++){
			if(file.name==this.uploadFiles[i].name){
				return true;
			}
		}
	}else{
		return false;
	}
}
/*
	函数绑定
*/
Asc.upload.uploadHander.prototype.bind = function(el,ename,fn,scope,arr){
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
	createListItem 创建列表
*/
Asc.upload.uploadHander.prototype.createListItem = function(file){
	var html = $('<div class="comp-swf-l" id="'+file.id+'"> <a href="javascript:void(0);">删除</a><div class="comp-swf-lname"><b class="comp-swf-files"></b><span>'+file.name+'</span></div><div class="comp-swf-ldetails"><span class="comp-swf-proc"><span class="comp-swf-prob" style="width:0%;"></span></span><span class="comp-swf-pron">0%</span></div><div class="comp-swf-lstate"><span>大小：'+this.getSize(file.size)+'</span></div></div>');
	this.uploadFilesHtml[file.id] = html;
	var btn = html.find('a:first');
	this.bind(btn,'click',this.removeFiles.createDelegate(this,[file]),this);
	this.fileList.append(html);
}
/*
	removeFiles 删除列表项
*/
Asc.upload.uploadHander.prototype.removeFiles = function(file){
	this.swfupload.cancelUpload(file.id);
	this.uploadFilesHtml[file.id].remove();
	delete this.uploadFilesHtml[file.id];
	this.aremove(file,this.uploadFiles);
	this.updateTips();
	if(this.uploadFiles.length==0){
		this.swfupload.setButtonDisabled(false);
		this.startBtn.show();
	}
	if(this.getTotalSize(1)){
		var per = Number((this.hasUploadBtyes+this.curBytes)/this.getTotalSize(1)*100);
		this.uploadTip.find('.comp-swf-prob:first').css('width',per.toFixed(0)+'%');
		this.uploadTip.find('.comp-swf-pron:first').text(per.toFixed(0)+'%');
	}else{
		this.uploadTip.find('.comp-swf-prob:first').css('width','0%');
		this.uploadTip.find('.comp-swf-pron:first').text('0%');
	}
	
}
/*
	removeAllFiles 删除所有文件
*/
Asc.upload.uploadHander.prototype.removeAllFiles = function(){
	for(var i=0,len=this.uploadFiles.length;i<len;i++){
		this.swfupload.cancelUpload(this.uploadFiles[i].id);
	}
	this.fileList.html('');
	this.uploadFilesHtml = {};
	this.uploadFiles = [];
	this.updateTips();
	this.swfupload.setButtonDisabled(false);
	this.hasUploadBtyes=0;
	this.uploadTip.find('.comp-swf-prob:first').css('width','0%');
	this.uploadTip.find('.comp-swf-pron:first').text('0%');
	this.uploadTip.find('strong:first').text('0');
	this.reStartBtn.hide();
	this.startBtn.show();
}
/*
	取消上传
*/
Asc.upload.uploadHander.prototype.cancelUploadFiles = function(){
	if(this.uploadFiles.length){
		for(var i=0,len=this.uploadFiles.length;i<len;i++){
			var item = this.swfupload.getFile(this.uploadFiles[i].id);
			if(item){
				this.swfupload.cancelUpload(item.id);
				this.uploadFilesHtml[item.id].remove();
			}
		}
	}
	this.uploadFilesHtml = {};
	this.uploadFiles = [];
	this.updateTips();
//	this.startBtn.show();
//	this.swfupload.setButtonDisabled(false);
	this.hasUploadBtyes=0;
	this.uploadTip.find('.comp-swf-prob:first').css('width','0%');
	this.uploadTip.find('.comp-swf-pron:first').text('0%');
	this.uploadTip.find('strong:first').text('0');
}
/*

*/
Asc.upload.uploadHander.prototype.reStartBtnFiles = function(){
	this.uploadFilesHtml = {};
	this.uploadFiles = [];
	this.updateTips();
	this.swfupload.setButtonDisabled(false);
	this.startBtn.show();
	this.hasUploadBtyes=0;
	this.uploadTip.find('.comp-swf-prob:first').css('width','0%');
	this.uploadTip.find('.comp-swf-pron:first').text('0%');
	this.uploadTip.find('strong:first').text('0');
	this.reStartBtn.hide();
	this.fileList.html('');
}
/*
	stratUploadFiles 开始上传
*/
Asc.upload.uploadHander.prototype.stratUploadFiles = function(){
	if(this.uploadFiles.length){
		this.swfupload.setButtonDisabled(true);
		this.startBtn.hide();
		for(var i in this.uploadFilesHtml){
			var item = this.uploadFilesHtml[i];
			item.find('b:first').removeClass();
			item.find('b:first').addClass('comp-swf-files');
			item.find('b:first').addClass('uploadwait');
			item.find('.comp-swf-lstate:first').html('<span>等待上传中，请稍后……</span>');
		}
		this.swfupload.startUpload();
	}
}
/*
	updateTips 更新提示
*/
Asc.upload.uploadHander.prototype.updateTips = function(){
	this.uploadTip.show();
	this.uploadTip.find('strong:eq(1)').text(this.getTotalSize());
	this.uploadTip.find('strong:eq(2)').text(this.uploadFiles.length);
}
/*
	getTotalSize 获取文件总大小
*/
Asc.upload.uploadHander.prototype.getTotalSize = function(c){
	c = c || 0;
	var size = 0;
	for(var i=0,len=this.uploadFiles.length;i<len;i++){
		size+=this.uploadFiles[i].size;
	}
	if(size&&!c){
		size = this.getSize(size);
	}
	return size;
}

/*
	getUploadLength 获取已上传文件个数
*/
Asc.upload.uploadHander.prototype.getUploadLength = function(btyes){
	var size = 0;
	for(var i=0,len=this.uploadFiles.length;i<len;i++){
		if(this.uploadFiles[i].filestatus==-4){
			size+=1;
		}
	}
	return size;
}

Asc.upload.uploadHander.prototype.aindexOf = function(item,arr){
	for (var i = 0, len = arr.length; i < len; i++){
		if(arr[i] == item){
			return i;
		}
    }
    return -1;
}
Asc.upload.uploadHander.prototype.aremove = function(item,arr){
	var index = arr.indexOf(item);
	if(index != -1){
		arr.splice(index, 1);
	}
	return arr;
}
Asc.upload.uploadHander.prototype.removeByIndex = function(index,arr){
	arr.splice(index, 1);
	return arr;
}
/*
	handler系列
*/
Asc.upload.uploadHander.prototype.fileQueued = function(file){
	if(this.checkExisit(file)){
		this.swfupload.cancelUpload(file.id);
	}else{
		this.createListItem(file);
		this.uploadFiles.push(file);
		this.updateTips();
	}
}
Asc.upload.uploadHander.prototype.fileQueuedError = function(file,error,message){
	switch(error){
		case -100:
			alert('文件数据超过'+this.swfupload.settings.file_queue_limit+'个');
			break;
		case -110:
			alert(file.name+'文件大小超过'+this.swfupload.settings.file_size_limit);
			break;
		case -120:
			alert(file.name+'文件大小为零');
			break;
		case -130:
			alert(file.name+'文件类型不符合');
			break;
		default:
			break;
	}
}
Asc.upload.uploadHander.prototype.fileDialogComplete = function(ns,nq){
	
}
Asc.upload.uploadHander.prototype.uploadStart = function(file){
	
}
Asc.upload.uploadHander.prototype.uploadProgress = function(file,bytes,totalbytes){
	this.updateTips();
	if(this.getTotalSize(1)){
		var per = Number((this.hasUploadBtyes+bytes)/this.getTotalSize(1)*100);
		this.uploadTip.find('.comp-swf-prob:first').css('width',per.toFixed(0)+'%');
		this.uploadTip.find('.comp-swf-pron:first').text(per.toFixed(0)+'%');
		this.uploadTip.find('strong:first').text(this.getSize(this.hasUploadBtyes+bytes));
	}else{
		this.uploadTip.find('.comp-swf-prob:first').css('width','0%');
		this.uploadTip.find('.comp-swf-pron:first').text('0%');
	}	
	var item = this.uploadFilesHtml[file.id];
	if(item){
		var peri =  Number(bytes/totalbytes*100);
		item.find('.comp-swf-prob:first').css('width',peri.toFixed(0)+'%');
		item.find('.comp-swf-pron:first').text(peri.toFixed(0)+'%');
		item.find('b:first').removeClass();
		item.find('b:first').addClass('comp-swf-files');
		item.find('b:first').addClass('uploading');
		item.find('.comp-swf-lstate:first').html('<span>已上传：'+this.getSize(bytes)+'</span><span class="p_lr_10">共：'+this.getSize(totalbytes)+'</span>');
	}
	this.curBytes = bytes;
}
Asc.upload.uploadHander.prototype.uploadError = function(file,error,message){

}
Asc.upload.uploadHander.prototype.uploadSuccess = function(file,response){
	var item = this.uploadFilesHtml[file.id];
	item.find('b:first').removeClass();
	item.find('b:first').addClass('comp-swf-files');
	item.find('b:first').addClass('uploadok');
	item.find('a:first').hide();
	this.hasUploadBtyes+=file.size;
	this.curBytes = 0;
}
Asc.upload.uploadHander.prototype.uploadComplete = function(file){
	
}
Asc.upload.uploadHander.prototype.queueComplete = function(num){
	if(this.startBtn.css('display')=='none'){
		this.reStartBtn.show();
	}else{
		this.reStartBtn.hide();
	}
	
}
/*
	getSize 转换文件大小 来自u.115.com
*/
Asc.upload.uploadHander.prototype.getSize = function(bytes){
	bytes = parseInt(Number(bytes));
	var unit = new Array('B', 'KB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB',
	'DB', 'NB');
	var extension = unit[0];
	var max = unit.length;
	for (var i = 1; ((i < max) && (bytes >= 1024)); i++) {
		bytes = bytes / 1024;
		extension = unit[i];
	}
	return Number(bytes).toFixed(2).replace(/\.00/, '') + extension;
}
/*
	getFlashVersion 获取flash版本信息 来自u.115.com
*/
Asc.upload.uploadHander.prototype.getFlashVersion = function(bytes){
	var hasFlash = 0;
    var flashVersion = 0;
    if (document.all) {
        var swf = null;
        try {
            swf = new ActiveXObject("ShockwaveFlash.ShockwaveFlash");
        } catch (e) {
        }
        if (swf) {
            hasFlash = 1;
            var VSwf = swf.GetVariable("$version");
            flashVersion = parseInt(VSwf.split(" ")[1].split(",")[0]);
        }
		swf=null;
    } else {
        if (navigator.plugins && navigator.plugins.length > 0) {
            var swf = null;
            try {
                swf = navigator.plugins['Shockwave Flash'];
            } catch (e) {
            }
            if (swf) {
                hasFlash = 1;
                var words = swf.description.split(" ");
                for (var i = 0; i < words.length; ++i) {
                    if (isNaN(parseInt(words[i]))) {
                        continue;
                    }
                    flashVersion = parseInt(words[i]);
                }
            }
        }
    }
    return {
        f : hasFlash,
        v : flashVersion
    };
}




/*
	--------------------
	无刷新文件上传
	bcpxqz@126.com
	--------------------
*/

Asc.upload.uploadSingle = function(obj){
	this.defaults = {
		method:'POST',
		name:'file',
		type:'.*',
		callback:function(){},
		num:1
	}
	this.option = $.extend({},this.defaults,obj);
	this.init();

}
/*
	初始化
*/

Asc.upload.uploadSingle.prototype.init = function(){
	this.input = this.createInput();
	this.el = $('#'+this.option.el);
	this.el.addClass('comp-ajax-upload');
	this.btn = $('<div class="comp-ajax-btn"> <span class="bt2"><span><button type="submit">上传</button></span></span> </div>');
	this.el.append(this.input);
	this.el.append(this.btn);
	this.bind(this.btn.find('button:first'),'click',this.startUpload,this);
}
/*
	上传
*/
Asc.upload.uploadSingle.prototype.startUpload = function(){
	this.el.find('.comp-ajax-tiptext').remove();
	this.files=[];
	var inputlist = this.input.find('input');
	var arr = [];
	for(var i=0,len=inputlist.length;i<len;i++){
		if($(inputlist[i]).val()!=''){
			var filename = $(inputlist[i]).val().replace(/.*\\/,'');
			if(this.checkExt(filename)){
				arr.push(inputlist[i]);
			}else{
				this.el.append('<div class="comp-ajax-tiptext s1">'+filename+' 格式不对，未上传</div>');
			}
		}
	}
	this.files=arr;
	if(this.files.length>0){
		this.inputDisabled();
		this.uploading(0);	
	}else{
		this.el.append('<div class="comp-ajax-tiptext s1">请选择上传文件</div>');
	}
}
/*
	uploading 上传单个文件
*/
Asc.upload.uploadSingle.prototype.uploading = function(index){
	var file = this.files[index] || false;
	if(file){	
			file = $(file);
			var filename = file.val().replace(/.*\\/,'');
			this.loading = $('<div class="comp-ajax-loading"> <span class="comp-ajax-tip">'+filename+' 正在上传，请稍后。</span><a href="javascript:void(0);">取消</a></div>');
			this.bind(this.loading.find('a:first'),'click',this.cancelUpload,this);
			var id = (new Date()).getTime();

			this.iframe = this.createIframe(id);
			this.form = this.createForm(id);

			this.el.append(this.iframe);
			this.el.append(this.loading);
			this.el.append(this.form);
			var tempfile = file.clone();
			file.before(tempfile);
			file.attr('id',file.attr('name')+(new Date()).getTime());
			file.attr('disabled',false);
			this.form.append(file);
			this.loading.show();
			this.btn.hide();
			this.form.submit();
			this.bind(this.iframe,'load',this.load,this,[index,filename]);
	}else{
		this.el.append('<div class="comp-ajax-tiptext s1">结束上传 <a href="javascript:void(0);" class="comp-ajax-rest">重新上传</a></div>');
		var rest = $(this.el.find('.comp-ajax-rest')[0]);
		this.bind(rest,'click',function(){this.reset()},this);
		this.option.callback.call(this,this);
		return false;
	}
}
/*
	cancelUpload取消上传
*/
Asc.upload.uploadSingle.prototype.cancelUpload = function(){
	var self = this;
	setTimeout(function(){
		self.iframe.remove();
	},0);
	this.input.remove();
	this.loading.remove();
	this.form.remove();
	this.btn.show();
	this.input = this.createInput();
	this.el.prepend(this.input);
}
/*
	重置
*/
Asc.upload.uploadSingle.prototype.reset = function(){
	this.input.remove();
	this.el.find('.comp-ajax-tiptext').remove();
	this.el.find('.comp-ajax-loading').remove();
	this.btn.show();
	this.input = this.createInput();
	this.el.prepend(this.input);
}
/*
	上传状态
*/
Asc.upload.uploadSingle.prototype.load = function(e,index,filename){
	var iframe = this.iframe[0];
	var res='';
	if(iframe.contentWindow){
		res = $(iframe.contentWindow.document.body).text();
	}else{
		res = $(iframe.contentDocument.document.body).text();
	}
	if(res=='ok'){
		var self = this;
		this.loading.text(filename+' 上传成功！');
		setTimeout(function(){
			$(self.el.find('iframe')[0]).remove();
		},0)
		this.form.remove();
		index+=1;
		this.uploading(index);
	}else{
		var self = this;
		setTimeout(function(){
			$(self.el.find('iframe')[0]).remove();
		},0)
		this.form.remove();
		this.loading.text(filename+' '+res);
		index+=1;
		this.uploading(index);
	}
}
/*
	验证
*/
Asc.upload.uploadSingle.prototype.checkExt = function(filename){
	var arr = filename.split('.');
	var type  = arr[arr.length-1];
	var reg = new RegExp(this.option.type);
	if(!type.match(reg)){
		return false;
	}
	return true;
}
/*
	函数绑定
*/
Asc.upload.uploadSingle.prototype.bind = function(el,ename,fn,scope,arr){
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
	创建input
*/
Asc.upload.uploadSingle.prototype.createInput = function(){
//	if(this.option.num>0){
//		if(!/\[\]/.test(this.option.name)){
//			this.option.name+='[]';
//		}
//	}
	var str='<ul class="comp-ajax-list">';
	for(var i=0,len=this.option.num;i<len;i++){
		str+='<li><input type="file" id="'+this.option.name+i+'" name="'+this.option.name+'" class="comp-ajax-input" /></li>'
	}
	str+='</ul>';
	return $(str);
}
/*
	禁用input
*/
Asc.upload.uploadSingle.prototype.inputDisabled = function(){
	this.input.find('input').each(function(i,n){
		$(n).attr('disabled','true');
	})
}
/*
	启用input
*/
Asc.upload.uploadSingle.prototype.inputAbled = function(){
	this.input.find('input').each(function(i,n){
		$(n).attr('disabled','false');
	})
}
/*
	createForm 创建form
*/
Asc.upload.uploadSingle.prototype.createForm = function(id){
	var form = $('<form action="'+this.option.action+'" method="'+this.option.method+'" enctype="multipart/form-data" id="'+this.option.el+'f'+id+'" target="'+this.option.el+'i'+id+'" style="display:none;"></form>');
	return form;
}
/*
	createIframe 创建Iframe
*/
Asc.upload.uploadSingle.prototype.createIframe = function(id){
	var iframe = $('<iframe  style="display:none;" src="javascript:false;" id="'+this.option.el+'i'+id+'" name="'+this.option.el+'i'+id+'"></iframe>');
	return iframe;
}
