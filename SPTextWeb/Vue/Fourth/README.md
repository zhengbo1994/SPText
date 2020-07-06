### Vue.js 
## 一、过渡(动画)

### 1. 简介
    Vue 在插入、更新或者移除 DOM 时，提供多种不同方式的应用过渡效果
    本质上还是使用CSS3动画：transition、animation

### 2. 基本用法
    使用transition组件，将要执行动画的元素包含在该组件内
        <transition>
            运动的元素
        </transition>       
过滤的CSS类名：6个
@before-enter ： 动画进入之前 
@enter ：动画进入 
@after-enter： 动画进入之后
@before-leave ：动画即将之前 
@leave ：动画离开 
@after-leave：动画离开之后

### 3. 钩子函数
    8个  
代码演示：10.html

### 4. 结合第三方动画库animate.css一起使用
    <transition enter-active-class="animated fadeInLeft" leave-active-class="animated fadeOutRight">
        <p v-show="flag">软谋</p>
    </transition>  

https://www.dowebok.com/demo/2014/98/

代码演示： 11.html
### 5. 多元素动画
    <transition-group>    
代码演示： 12.html

### 6. 练习
    多元素动画    
代码演示： 13.html

## 二、 组件component

### 1. 什么是组件？
    组件（Component）是 Vue.js 最强大的功能之一。组件可以扩展 HTML 元素，封装可重用的代码
    组件是自定义元素（对象）

### 2. 定义组件的方式    
    方式1：先创建组件构造器，然后由组件构造器创建组件
    方式2：直接创建组件

代码演示：01.html

### 3. 组件的分类
    分类：全局组件、局部组件
代码演示：02.html

### 4. 引用模板
    将组件内容放到模板<template>中并引用
代码演示：03.html
### 5. 动态组件
    <component :is="">组件
        多个组件使用同一个挂载点，然后动态的在它们之间切换    
    
    <keep-alive>组件    
代码演示：04.html

## 三、 组件间数据传递
    
### 1. 父子组件
    在一个组件内部定义另一个组件，称为父子组件
    子组件只能在父组件内部使用
    默认情况下，子组件无法访问父组件中的数据，每个组件实例的作用域是独立的

### 2. 组件间数据传递 （通信）

#### 2.1 子组件访问父组件的数据
    a)在调用子组件时，绑定想要获取的父组件中的数据
    b)在子组件内部，使用props选项声明获取的数据，即接收来自父组件的数据
    总结：父组件通过props向下传递数据给子组件
    注：组件中的数据共有三种形式：data、props、computed

代码演示：04.html

#### 2.2 父组件访问子组件的数据
    a)在子组件中使用vm.$emit(事件名,数据)触发一个自定义事件，事件名自定义
    b)父组件在使用子组件的地方监听子组件触发的事件，并在父组件中定义方法，用来获取数据
    总结：子组件通过events给父组件发送消息，实际上就是子组件把自己的数据发送到父组件
代码演示：05-2.html

