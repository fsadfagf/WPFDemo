BEGIN TRAN;

DECLARE @NewAddr TABLE (Idx int IDENTITY(1,1), AddressId int);

INSERT INTO AddressInfo (ProvinceName, City, Area, DetailAddress, CreateUserId, CreateTime, Status)
OUTPUT INSERTED.AddressId INTO @NewAddr(AddressId)
VALUES
 (N'广东省', N'深圳市', N'南山区', N'科技园南路 1 号 A 栋 501', 1, GETDATE(), 0),
 (N'广东省', N'深圳市', N'福田区', N'深南大道 1008 号 12 层', 1, GETDATE(), 0),
 (N'广东省', N'广州市', N'天河区', N'天河路 228 号 3 单元', 1, GETDATE(), 0),
 (N'广东省', N'广州市', N'越秀区', N'北京路 55 号 802 室', 1, GETDATE(), 0),
 (N'北京市', N'北京市', N'朝阳区', N'建国路 88 号 SOHO 现代城 1906', 1, GETDATE(), 0),
 (N'北京市', N'北京市', N'海淀区', N'中关村南大街 5 号 2 号楼', 1, GETDATE(), 0),
 (N'上海市', N'上海市', N'浦东新区', N'世纪大道 1568 号 27 层', 1, GETDATE(), 0),
 (N'上海市', N'上海市', N'徐汇区', N'漕溪北路 88 号 1102 室', 1, GETDATE(), 0),
 (N'浙江省', N'杭州市', N'西湖区', N'文三路 90 号东部软件园 5 号楼', 1, GETDATE(), 0),
 (N'浙江省', N'杭州市', N'拱墅区', N'莫干山路 118 号 3 单元 401', 1, GETDATE(), 0),
 (N'江苏省', N'南京市', N'鼓楼区', N'中山北路 200 号 1801 室', 1, GETDATE(), 0),
 (N'江苏省', N'南京市', N'玄武区', N'珠江路 88 号新世界中心 A 座', 1, GETDATE(), 0),
 (N'四川省', N'成都市', N'武侯区', N'天府大道中段 500 号 7 栋', 1, GETDATE(), 0),
 (N'四川省', N'成都市', N'锦江区', N'红星路三段 1 号 IFS 写字楼 22 层', 1, GETDATE(), 0),
 (N'湖北省', N'武汉市', N'洪山区', N'珞瑜路 1037 号 9 栋 302', 1, GETDATE(), 0),
 (N'湖北省', N'武汉市', N'江汉区', N'建设大道 568 号新世界国贸大厦', 1, GETDATE(), 0),
 (N'陕西省', N'西安市', N'雁塔区', N'高新四路 20 号 6 号楼 1203', 1, GETDATE(), 0),
 (N'陕西省', N'西安市', N'未央区', N'未央路 130 号凯鑫大厦 15 层', 1, GETDATE(), 0),
 (N'山东省', N'青岛市', N'市南区', N'香港中路 76 号颐中皇冠假日 1008', 1, GETDATE(), 0),
 (N'福建省', N'厦门市', N'思明区', N'软件园二期观日路 22 号 501', 1, GETDATE(), 0);

INSERT INTO CustomerInfo (CustomerName, Sex, Age, Phone, AddressId, CreateUserId, CreateTime, Status)
SELECT v.CustomerName, v.Sex, v.Age, v.Phone, a.AddressId, 1, GETDATE(), 0
FROM (VALUES
 (1,  N'张伟', 1, 32, N'13900000001'),
 (2,  N'王芳', 0, 28, N'13900000002'),
 (3,  N'李娜', 0, 25, N'13900000003'),
 (4,  N'刘洋', 1, 41, N'13900000004'),
 (5,  N'陈静', 0, 36, N'13900000005'),
 (6,  N'杨帆', 1, 23, N'13900000006'),
 (7,  N'赵磊', 1, 45, N'13900000007'),
 (8,  N'黄敏', 0, 30, N'13900000008'),
 (9,  N'周涛', 1, 38, N'13900000009'),
 (10, N'吴霞', 0, 27, N'13900000010'),
 (11, N'徐强', 1, 52, N'13900000011'),
 (12, N'孙丽', 0, 29, N'13900000012'),
 (13, N'马超', 1, 34, N'13900000013'),
 (14, N'朱琳', 0, 26, N'13900000014'),
 (15, N'胡军', 1, 47, N'13900000015'),
 (16, N'郭涛', 1, 31, N'13900000016'),
 (17, N'何平', 0, 55, N'13900000017'),
 (18, N'高翔', 1, 22, N'13900000018'),
 (19, N'林静', 0, 33, N'13900000019'),
 (20, N'罗勇', 1, 39, N'13900000020')
) AS v(Idx, CustomerName, Sex, Age, Phone)
JOIN @NewAddr a ON a.Idx = v.Idx;

INSERT INTO UserInfo (Account, Password, CreateUserId, CreateTime, Status)
VALUES
 (N'user01', N'123456', 1, GETDATE(), 0),
 (N'user02', N'123456', 1, GETDATE(), 0),
 (N'user03', N'123456', 1, GETDATE(), 0),
 (N'user04', N'123456', 1, GETDATE(), 0),
 (N'user05', N'123456', 1, GETDATE(), 0),
 (N'user06', N'123456', 1, GETDATE(), 0),
 (N'user07', N'123456', 1, GETDATE(), 0),
 (N'user08', N'123456', 1, GETDATE(), 0),
 (N'user09', N'123456', 1, GETDATE(), 0),
 (N'user10', N'123456', 1, GETDATE(), 0),
 (N'user11', N'123456', 1, GETDATE(), 0),
 (N'user12', N'123456', 1, GETDATE(), 0),
 (N'user13', N'123456', 1, GETDATE(), 0),
 (N'user14', N'123456', 1, GETDATE(), 0),
 (N'user15', N'123456', 1, GETDATE(), 0),
 (N'user16', N'123456', 1, GETDATE(), 0),
 (N'user17', N'123456', 1, GETDATE(), 0),
 (N'user18', N'123456', 1, GETDATE(), 0),
 (N'user19', N'123456', 1, GETDATE(), 0),
 (N'user20', N'123456', 1, GETDATE(), 0);

COMMIT;

SELECT 'AddressInfo' AS T, COUNT(*) AS Cnt FROM AddressInfo
UNION ALL SELECT 'CustomerInfo', COUNT(*) FROM CustomerInfo
UNION ALL SELECT 'UserInfo', COUNT(*) FROM UserInfo;
