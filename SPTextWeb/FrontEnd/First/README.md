### Vue.js 基础部分

## 一、 Vue.js 简介

### 1. Vue.js 是什么

**Vue.js**也称为 Vue，读音/vju:/，类似 view，错误读音 v-u-e
版本： v2.6

- 是一个构建用户界面的框架
- 是一个轻量级 MVVM（Model-View-ViewModel）框架，和 angular、react 类似，其实就是所谓的数据双向绑定
- 数据驱动+组件化的前端开发（核心思想）
- 通过简单的 API 实现**响应式的数据绑定**和**组合的视图组件**
- 更容易上手、小巧

参考：[官网](https://cn.vuejs.org/)

#### MVVM框架

1.1定义：
Mvvm定义MVVM是Model-View-ViewModel的简写。即模型-视图-视图模型。
【模型】指的是后端传递的数据。
【视图】指的是所看到的页面。
【视图模型】mvvm模式的核心，它是连接view和model的桥梁。
它有两个方向：
一是将【模型】转化成【视图】，即将后端传递的数据转化成所看到的页面。实现的方式是：数据绑定。
二是将【视图】转化成【模型】，即将所看到的页面转化成后端的数据。

实现的方式是：DOM 事件监听。这两个方向都实现的，我们称之为数据的双向绑定。

Vue就是基于MVVM模式实现的一套框架，
在vue中：
Model:指的是js中的数据，如对象，数组等等。
View:指的是页面视图
viewModel:指的是vue实例化对象

1.2总结：
在MVVM的框架下视图和模型是不能直接通信的。它们通过ViewModel来通信，ViewModel通常要实现一个observer观察者，当数据发生变化，ViewModel能够监听到数据的这种变化，然后通知到对应的视图做自动更新，而当用户操作视图，ViewModel也能监听到视图的变化，然后通知数据做改动，这实际上就实现了数据的双向绑定。

并且MVVM中的View 和 ViewModel可以互相通信。MVVM流程图如下：


### 2.vue 和 angular 的区别

#### 2.1 angular

- 上手较难
- 指令以 ng-xxx 开头
- 所有属性和方法都存储在\$scope 中
- 由 google 维护

#### 2.2 vue

- 简单、易学、更轻量
- 指令以 v-xxx 开头
- HTML 代码+JSON 数据，再创建一个 vue 实例
- 由个人维护：**尤雨溪**，华人，目前就职于阿里巴巴，2014.2 开源了 vue.js 库

![尤雨溪](https://gss1.bdstatic.com/9vo3dSag_xI4khGkpoWK1HF6hhy/baike/c0%3Dbaike80%2C5%2C5%2C80%2C26/sign=d49c7e60ee1190ef15f69a8daf72f673/4afbfbedab64034f29596c8ba6c379310b551da2.jpg)

共同点：`都不兼容低版本IE`，`对 seo 搜索引擎不友好`
对比：GitHub 上 vue(156k) 的 stars 数量大约是 angular(59.6k) 的两倍多

## 二、起步

### 1. 下载核心库 vue.js

    npm init --yes
    npm install vue --save
    版本 v2.6.11 目前最新版本(2020.2.2)

    vue2.0和1.0相比，最大的变化就是引入了Virtual DOM（虚拟DOM）,页面更新效率更高，速度更快

### 2. Hello World（对比 angular）

#### 2.1 angular 实现

    js:
    	let app=angular.module('myApp',[]);
    	app.controller('MyController',['$scope',function($scope){
    		$scope.msg='Hello World';
    	}]);
    html:
    	<html ng-app="myApp">
    		<div ng-controller="MyController">
    			{{msg}}
    		</div>
    	</html>

#### 2.2 vue 实现

    js:
    	new Vue({
    		el:'#itany', //指定关联的选择器，进行挂载
    		data:{ //存储数据
    			msg:'Hello World',
    			name:'tom'
    		}
    	});
    html:
    	<div id="itany">
    		{{msg}}
    	</div>

### 3. 安装 vue-devtools 插件，便于在 chrome 中调试 vue

    直接将vue-devtools解压缩，然后将文件夹中的chrome拖放到扩展程序中（更多工具--扩展程序 ）
    或者直接在浏览器中输入 chrome://extensions/打开

    //配置是否允许vue-devtools检查代码，方便调试，生产环境中需要设置为false
        Vue.config.devtools=false;
        Vue.config.productionTip=false; //阻止vue启动时生成生产消息

## 三、 常用指令

### 1. 什么是指令？

    用来扩展html标签的功能
    angular中常用的指令：
        ng-model
        ng-repeat
        ng-click
        ng-show/ng-hide
        ng-if

### 2. vue 中常用的指令

- v-model
  双向数据绑定，一般用于表单元素
  在表单 <input>、<textarea> 及 <select> <checkbox> 元素上创建双向数据绑定。
 代码演示：02.html
- v-for
  对数组或对象进行循环操作，使用的是 v-for，不是 v-repeat
  注：在 vue1.0 中提供了隐式变量，如$index、$key
  在 vue2.0 中去除了隐式变量，已被废除
代码演示 代码演示：03.html
- v-on
  用来绑定事件，用法：v-on:事件="函数"
代码演示：04.html
- v-show/v-if  
  用来显示或隐藏元素，v-show 是通过 display 实现，v-if 是每次删除后再重新创建，与 angular 中类似
代码演示：05.html
## 四、 练习：用户管理

    使用BootStrap+Vue.js
06-练习.html

## 五、 事件和属性

### 1. 事件

#### 1.1 事件简写

    v-on:click=""
    简写方式 @click=""

#### 1.2 事件对象\$event

    包含事件相关信息，如事件源、事件类型、偏移量
    target、type、offsetx
代码演示：07.html

#### 1.3 事件冒泡

    阻止事件冒泡：
        a)原生js方式，依赖于事件对象  stopPropagation
        b)vue方式，不依赖于事件对象
            @click.stop
代码演示：08.html
#### 1.4 事件默认行为

    阻止默认行为：
        a)原生js方式，依赖于事件对象 preventDefault
        b)vue方式，不依赖于事件对象
            @click.prevent

#### 1.5 键盘事件

    回车：@keydown.13 或@keydown.enter
    上：@keydown.38 或@keydown.up

    默认没有@keydown.a/b/c...事件，可以自定义键盘事件，也称为自定义键码或自定义键位别名
代码演示：09.html

#### 1.6 事件修饰符

在事件处理程序中调用 event.preventDefault() 或 event.stopPropagation() 是非常常见的需求
Vue.js 为 v-on 提供了事件修饰符。之前提过，修饰符是由点开头的指令后缀来表示的。

    .stop - 调用 event.stopPropagation()。 阻止冒泡行为的事件传播
    .prevent - 调用 event.preventDefault()。 阻止默认行为
    .{keyCode | keyAlias} - 只当事件是从特定键触发时才触发回调。
    .native - 监听组件根元素的原生事件。
    .once - 只触发一次回调。

<!-- 阻止单击事件继续传播 -->
<a v-on:click.stop="doThis"></a>

<!-- 提交事件不再重载页面 -->
<form v-on:submit.prevent="onSubmit"></form>

<!-- 修饰符可以串联 -->
<a v-on:click.stop.prevent="doThat"></a>

<!-- 添加事件监听器时使用事件捕获模式 -->
<!-- 即元素自身触发的事件先在此处理，然后才交由内部元素进行处理 -->
<div v-on:click.capture="doThis">...</div>

<!-- 只当在 event.target 是当前元素自身时触发处理函数 -->
<!-- 即事件不是从内部元素触发的 -->
<div v-on:click.self="doThat">...</div>

<!-- 点击事件将只会触发一次 -->
<a v-on:click.once="doThis"></a>

<!-- 滚动事件的默认行为 (即滚动行为) 将会立即触发 -->
<!-- 而不会等待 `onScroll` 完成  -->
<!-- 这其中包含 `event.preventDefault()` 的情况 -->
<div v-on:scroll.passive="onScroll">...</div>
这个 .passive 修饰符尤其能够提升移动端的性能。
按键修饰符


### 2. 属性

#### 2.1 属性绑定和属性的简写

    v-bind 用于属性绑定， v-bind:属性=""

    属性的简写：
        v-bind:src="" 简写为 :src=""
代码演示：10.html
#### 2.2 class 和 style 属性

    绑定class和style属性时语法比较复杂：
代码演示：11.html
## 六、 模板

### 1. 简介

    Vue.js使用基于HTML的模板语法，可以将DOM绑定到Vue实例中的数据
    模板就是{{}}，用来进行数据绑定，显示在页面中
    也称为Mustache语法

### 2. 数据绑定的方式

    a.双向绑定
        v-model
    b.单向绑定
        方式1：使用两对大括号{{}}，可能会出现闪烁的问题，可以使用v-cloak解决
        方式2：使用v-text、v-html

### 3. 其他指令

    v-once 数据只绑定一次
    v-pre 不编译，直接原样显示
代码演示：12.html

## 七、 过滤器

### 1. 简介

    用来过滤模型数据，在显示之前进行数据处理和筛选
    语法：{{ data | filter1(参数) | filter2(参数)}}

### 2. 关于内置过滤器

    vue1.0中内置许多过滤器，如：
        currency、uppercase、lowercase
        limitBy
        orderBy
        filterBy
    vue2.0中已经删除了所有内置过滤器，全部被废除
    如何解决：
        a.使用第三方工具库，如lodash、date-fns日期格式化、accounting.js货币格式化等
        b.使用自定义过滤器

### 3. 自定义过滤器

    分类：全局过滤器、局部过滤器
代码演示：13.html
#### 3.l 自定义全局过滤器

    使用全局方法Vue.filter(过滤器ID,过滤器函数)

#### 3.l 自定义局部过滤器
