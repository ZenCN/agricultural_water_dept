*******************常见错误***********************
IIS 调用Microsoft.Office.Interop.Word.Documents.Open 返回为null

解决方案：
1.将应用程序池中的标识设置为"管理员"
2.运行"dcomcnfg"命令，打开系统组件服务
3.点击 组件服务 >> 计算机 >> 我的电脑 >> DCOM配置 >> 找到Microsoft Word
 （注册服务的名字可能不一样，如安装Office 2013之后，注册服务Office Word的名字是 Microsoft Document Explorer 、Office Excel的名字是 Microsoft Excel Application
  所以最好将凡是名字包含 "Microsoft Excel" 的服务都加上以下权限，其他Document、PowerPoint的一样
  如果在组件服务中就看不到Microsoft Offfice Word，则需要重新安装Office）
4.右键Microsoft Word选择属性，打开属性对话框
5.选择 "标识" 选项卡，然后将 "运行此应用程序的用户账户" 更改为 "交互式用户"
6.选择 "安全" 选项卡，分别在 "启动和激活权限" 和 "访问权限" 组中选择 "自定义" 再点击"编辑"
7.在弹出的对话框中添加 ASP.NET、IUSER、IIS_USERS、Authenticated Users 等账户
8.点击 "确定" 即可