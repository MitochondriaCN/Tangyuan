-- MySQL dump 10.13  Distrib 5.7.34, for Linux (x86_64)
--
-- Host: localhost    Database: tangyuan
-- ------------------------------------------------------
-- Server version	5.7.34-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `comment_table`
--

DROP TABLE IF EXISTS `comment_table`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `comment_table` (
  `id` int(11) NOT NULL COMMENT '评论唯一ID',
  `user_id` int(11) NOT NULL COMMENT '评论用户ID',
  `post_id` int(11) NOT NULL COMMENT '评论帖子ID',
  `date` datetime NOT NULL COMMENT '评论日期',
  `likes` int(11) NOT NULL COMMENT '评论点赞数',
  `is_reply` tinyint(1) NOT NULL COMMENT '是否为回复评论',
  `reply_id` int(11) NOT NULL COMMENT '被回复评论唯一ID',
  `content` text NOT NULL COMMENT '评论内容',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `comment_table`
--

LOCK TABLES `comment_table` WRITE;
/*!40000 ALTER TABLE `comment_table` DISABLE KEYS */;
INSERT INTO `comment_table` VALUES (1,1,3,'2023-07-15 12:59:41',16,0,0,'写得好>_<'),(2,1,3,'2023-07-15 14:45:10',2,0,0,'就是有点烂-_-');
/*!40000 ALTER TABLE `comment_table` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `post_table`
--

DROP TABLE IF EXISTS `post_table`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `post_table` (
  `id` int(11) NOT NULL COMMENT '帖子唯一ID',
  `author_id` int(11) NOT NULL COMMENT '作者ID',
  `post_date` datetime(6) NOT NULL COMMENT '发布日期',
  `likes` int(11) NOT NULL COMMENT '点赞数',
  `views` int(11) NOT NULL COMMENT '阅读数',
  `content` text NOT NULL COMMENT '正文（以XML）',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `post_table`
--

LOCK TABLES `post_table` WRITE;
/*!40000 ALTER TABLE `post_table` DISABLE KEYS */;
INSERT INTO `post_table` VALUES (2,1,'2023-07-09 11:28:14.000000',256,265,'<TangyuanPost Type=\"Post\">\r\n<Title>四人行，末日豪劫</Title>\r\n<ImageGallery>\r\n<Image>https://s2.loli.net/2023/07/09/SsrBAbuPlUdZ2iJ.png</Image>\r\n</ImageGallery>\r\n<P>双十二更新的末日豪劫系列任务相信又点燃了各位玩家对于gta5的热情，不过由于这个任务难度比以往的抢劫难度更大，所以相信卡关成了各位普遍要面对的现象。那么就由我给大家带来一篇完整的末日豪劫攻略。</P>\r\n<P>末日豪劫算是各位玩家翘首期盼两年多才姗姗来迟的新抢劫任务。</P>\r\n<P>抢劫任务当然得要房产才能开启， 但是让你直接一分钱不花就可以在原有公寓开任务又不是坑钱星的风格，于是就有了“设施”这种东西。再次强调，我对“设施”这个翻译十分不满意，哪怕叫基地也好啊，或者其实这个才是真正的地堡。</P>\r\n</TangyuanPost>'),(3,1,'2023-07-13 08:45:12.000000',129,8876,'<TangyuanPost Type=\"Post\">\r\n    <Title>《蓝氧时代》第一章</Title>\r\n    <ImageGallery>    <Image>https://cdn.pixabay.com/photo/2023/07/08/11/01/czech-republic-8114188_1280.jpg</Image>\r\n    </ImageGallery>\r\n    <P>当我走进五中校门的时候，我心里顿时高呼：傻逼的小学生活终于他妈的结束了！那是报名的日子，当天风和日丽、万里无云，我拿着报告单四处找报名处在哪。然后我看到在教学楼门前排了四列人，我猜那应该就是报名处。我拿着报告单走到其中一列人后面排，前面站着的是一个女生，个子不高，但挺瘦，梳个双马尾；我暗想这可能是我将来的同学，弄不好搞成对象。但我正这么想着，就看见她和前面那个人高马大的体育生一样的男生当众亲了一嘴。我心里直呼“他妈的”。</P>\r\n    <P>队列走得很快，没过几分钟我就来到了门前。门前摆了一张长桌子，桌子后面坐着两个女老师，看起来正在中年油腻期。其中一个要过我的报名单，看了几眼，然后交给另外一个，后者在上面画了笔什么，然后盖了一个章，就把报名单给我，打发我走。走出队列一看，她画的是一个“7”，盖的是学校的公章。我纳闷这是什么意思，但旋即全校广播通知：所有拿到已盖章报名单的学生到广场集合。我没有多想便去了。</P>\r\n    <P>去了又是站队，这次沿着广场的长边站了二三十列队，全部面对着国旗杆方向。那我站哪个队？然后广播又通知了：请各位学生查看报名单上写的数字，在队伍前的告示牌上找到对应数字的队伍入列。我一看，每队前果然有一个告示牌上写着数字。我找到数字“7”的队列，排在最后面。定睛一看，前面站着的是那个人高马大的体育生。但是在我四处乱瞟的过程中，我发现他的女人和他隔着三列队。“各位同学！”喇叭里突然响起一个男人的声音，打断我幸灾乐祸，“欢迎大家来到五中！选择了五中，就是选择了未来！选择了五中，就是选择了成功！……”我感觉选择马上离开这个地方可能更有助于我未来的成功。“同学们，”他继续说，“现在你们报名单上写着的数字，以及你们所占的队列前面告示牌上的数字，就代表你们将来将要进入的班级！”我没能憋住，笑了出来，前面这哥们跟他女人没在一个班啊！他听见声，转过头来一脸鄙夷地看着我。考虑到今后同学一场，现在搞坏印象怕是不妙，我便收住了。那中年男人随后讲了十几分钟的话，什么入学注意事项之类的东西，我一点也没听进去，我盯着天上飞来飞去的鸟看了十多分钟。</P>\r\n    <P>那男的终于讲完了话，我终于能到我的教室去坐两分钟休息一下。我进入教学楼，根据门口的牌子找到我所在的七年级（7）班，两个“七”，搞什么七七事变。进入教室，环顾四周，教室还是跟小学的一样大，没有什么特别之处。讲台上面挂着国旗，两侧写着“厚德载物”和“明知博学”八个大字的标语。教室里空空荡荡，桌椅都在后面堆放得很整齐，等着我们新生把它们摆放到适合的位置。陆续有新同学进来，没一会，那“体育生”也进来了。他张着眼睛到处瞎转着看，跟没见过教室一样，想不通那女的怎么看上他这种吊儿郎当的人的。</P>\r\n    <P>等到班里同学到得差不多，班主任进来了。我看她应该有四十好几，身材体貌神似大门口签字盖章的老师，活脱脱一副中年女教师样貌。她进来时，我们都在互相说话，没注意到她。她毫不客气地说：“嘴都闭上！话怎么那么多！”声音凌厉而高亢，我们迅速地乖乖闭嘴了。没干过二十年老师没有这架势。然后她站在讲台上说：“各位新同学，先别忙着互相认识，先把后面的桌椅摆放整齐，七乘七，现在就开始！”看来初中办事跟小学不一样，就是利索，一句废话没有。我就到后面去帮忙摆桌子。“体育生”够猛，一人能拎俩桌子，在教室里面噌噌地走，比谁干活都快。班主任倒也没闲着，她站在教室正中央指挥交通，这桌子摆哪，那椅子放哪，全靠她一张嘴安排。</P>\r\n    <P>搬桌子的时候，我大概扫了一眼，发现班里好像是女生比男生多，不过就多几个。作为一个男人，自然是先关注将来班内择偶潜在对象的质量。大致一眼扫过去，倒真有几个出众的。其中有一个，穿着黑短裙和黑过膝袜，腿也长，身材也好，长得也还行，我看很不错。于是便果断开始试图接近，结果搞砸了，我搬着桌子碰到她腰了。她“啊”地轻叫一声——声音也好听——班主任听见了，看向我俩这边，说：“哎呀，你就不会小心点！”我脸都红了，再看她，好像不怎么在乎，摆摆手说“没事”，还对我笑笑。我心都化了，妈的，开学第一天就有这等艳福，前途不可限量！你等着，我必然把你搞到手！</P>\r\n    <P>几分钟后就搬好了桌子，班主任让我们自己找地儿坐。我刻意没有先选座位，因为我要等梦中情人先选，然后非常自然地坐在她的旁边，顺理成章地成为她的同桌。我盯着她看，她选了第二排靠窗的座位。好！我果断走去她旁边，不失风度地问了一句：“我可以坐在这里吗？”如果你看过台湾偶像剧，你就可以想象我这时的语气。她看向我，一双大眼睛扑闪了两下，我的心理防线已经溃败了三分。随后她莞尔一笑说：“可以啊！”听到这句话，我尽力克制自己的心情，让腿不要发抖，否则将直接瘫在座位上。我用扎马步的力道缓慢而优雅地降落在我的座位上，等到屁股接触椅子的那一刻，我把将来和她入洞房采取的姿势都想好了。我坐稳当，随即一阵暗香传来，令我意乱情迷。前面是一个彪形大汉，后面是“体育生”——他什么时候来的？——右边是一个个子瘦弱的男生，这三位皆非香源，故只有左侧佳人可携此般芬芳。克制，克制，再克制——这时，班主任发话了。</P>\r\n</TangyuanPost>'),(5,1,'2023-07-16 23:31:58.000000',0,0,'<TangyuanPost Type=\"Post\"><Title>我测你们码，终于可以发帖了</Title><P>发一条试试先</P></TangyuanPost>'),(6,3,'2023-07-16 23:42:07.000000',0,0,'<TangyuanPost Type=\"Post\"><Title>我测付宇轩的🐎</Title><P>回击付宇轩之测</P></TangyuanPost>'),(7,1,'2023-07-16 23:59:56.000000',0,0,'<TangyuanPost Type=\"Post\"><Title>忘做删帖了</Title><P>发了不能删，去你妈的，明天再做，睡觉😪</P></TangyuanPost>');
/*!40000 ALTER TABLE `post_table` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `school_table`
--

DROP TABLE IF EXISTS `school_table`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `school_table` (
  `id` int(11) NOT NULL COMMENT '学校ID',
  `name` tinytext NOT NULL COMMENT '学校全名',
  `theme_color` tinytext NOT NULL COMMENT '学校主题色（#xxxxxx））'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `school_table`
--

LOCK TABLES `school_table` WRITE;
/*!40000 ALTER TABLE `school_table` DISABLE KEYS */;
INSERT INTO `school_table` VALUES (1,'嘉峪关市酒钢三中','');
/*!40000 ALTER TABLE `school_table` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `user_table`
--

DROP TABLE IF EXISTS `user_table`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `user_table` (
  `id` int(11) NOT NULL COMMENT '用户唯一ID',
  `passwd` tinytext NOT NULL COMMENT '密码',
  `nickname` char(20) NOT NULL COMMENT '用户昵称',
  `phone_number` tinytext NOT NULL COMMENT '电话号码',
  `school_id` int(11) NOT NULL COMMENT '所属学校ID',
  `avatar` tinytext NOT NULL COMMENT '头像地址',
  `grade_code` tinyint(4) NOT NULL COMMENT '年级代码（年级由学校定义）',
  PRIMARY KEY (`id`),
  UNIQUE KEY `id` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COMMENT='用户信息表';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `user_table`
--

LOCK TABLES `user_table` WRITE;
/*!40000 ALTER TABLE `user_table` DISABLE KEYS */;
INSERT INTO `user_table` VALUES (1,'Fuyuxuan372819','线粒体XianlitiCN','18993791251',1,'https://s2.loli.net/2023/07/15/LmurCOa581pKnTg.png',0),(2,'12345678','野未来','15393260359',1,'https://s2.loli.net/2023/07/16/9BYSGcTiyeNOlwh.jpg',1),(3,'12345678','南木','17793798483',1,'https://s2.loli.net/2023/07/16/8xYUjk9RhF3GHVJ.jpg',1),(4,'12345678','梦Y.','15393260057',1,'https://s2.loli.net/2023/07/16/83qsAPvrhNOuxj1.jpg',1);
/*!40000 ALTER TABLE `user_table` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Dumping events for database 'tangyuan'
--

--
-- Dumping routines for database 'tangyuan'
--
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2023-07-19 11:56:15
