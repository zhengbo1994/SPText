db.gl.insert({_id : 1, name:'乾隆',like:'狗',age:20,gender:true})
db.gl.insert({_id : 2, name:'魏璎珞',like:'猫',age:18,gender:false})
db.gl.insert({_id : 3, name:'令贵妃',like:'猫',age:28,gender:false})
db.gl.insert({_id : 4, name:'高贵妃',like:'鸡',age:45,gender:true})
db.gl.insert({_id : 5, name:'小李子',like:'荷兰鼠',age:16,gender:true})
db.gl.insert({_id : 6, name:'小张子',like:'狗',age:45,gender:true})
db.gl.insert({_id : 7, name:'富察皇后',like:'鸡',age:60,gender:true})


create table stu(
id int auto_increment primary key not null,
name varchar(10) not null,
birthday datetime,
gender bit default 1,
isDelete bit default 0
);

create table subjects(
id int auto_increment primary key not null,
title varchar(10) not null);

科目表
(0,"语文"),
(0,"数学"),
(0,"英语"),
(0,"科学"),



(1,"小明","2008-01-01",0,0,"北京",90),
(2,"小红","2007-01-01",1,0,"上海",80),
(3,"小兰","2006-01-01",1,0,"广州",100),
(4,"小王","2005-01-01",0,0,"深圳",20),
(5,"老王","2009-01-01",0,0,null,30),
(6,"老刘","2004-01-01",0,0,null,40),
(7,"小丽","2003-01-01",1,0,"东莞",50),
(8,"小芳","2002-01-01",1,0,"福建",60),
(9,"小粒","2001-01-01",0,0,"福州",70),


 create table stu(
     id int auto_increment primary key,
     name varchar(10) not null,
     birthday datetime,
     gender bit default 0,
     isdelete bit default 0,
     address varchar(100),
     score int
     );


create table scores(
id int primary key auto_increment,
stuid int,
subid int,
score decimal(5,2),
foreign key(stuid) references stu(id),
foreign key(subid) references subjects(id)
);


CREATE TABLE `stu` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `name` varchar(10) NOT NULL,
  `birthday` datetime DEFAULT NULL,
  `gender` bit(1) DEFAULT b'0',
  `isdelete` bit(1) DEFAULT b'0',
  `hometown` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`id`)
)

insert into scores values(0,1,1,80),(0,2,2,60),(0,2,3,70),(0,3,1,90),(0,4,4,60),(0,5,2,75);



select stu.sname,subjects.stitle,scores.score
from scores
inner join left stu on scores.stuid=stu.id
inner join subjects on scores.subid=subjects.id;



 select city.* from areas as city
     inner join areas as province on city.pid=province.id
     where province.title="广东省"







