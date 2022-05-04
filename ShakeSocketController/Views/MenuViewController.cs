using ShakeSocketController.Controller;
using ShakeSocketController.Model;
using ShakeSocketController.Properties;
using ShakeSocketController.Utils;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ShakeSocketController.Views
{
    /// <summary>
    /// 任务栏图标菜单控制器
    /// </summary>
    public class MenuViewController
    {
        private readonly TransactionController _controller;

        private NotifyIcon taskBarIcon;                 //任务栏图标
        private ContextMenuStrip taskBarMenu;           //任务栏图标的右键菜单
        private ToolStripMenuItem sscItem;              //SSC服务模式选项
        private ToolStripMenuItem disabledItem;         //禁用
        private ToolStripMenuItem listenOnlyItem;       //仅监听
        private ToolStripMenuItem bcAndListenItem;      //自动（监听#广播）
        private ToolStripMenuItem allowCtrlItem;        //允许控制
        private ToolStripMenuItem autoStartupItem;      //开机启动
        private ToolStripMenuItem settingItem;          //设置
        private ToolStripMenuItem helpItem;             //帮助选项
        private ToolStripMenuItem showLogItem;          //显示日志
        private ToolStripMenuItem aboutItem;            //关于
        private ToolStripMenuItem quitItem;             //退出

        private DeviceListForm deviceListForm;          //配置连接窗口

        public MenuViewController(TransactionController controller)
        {
            //保存对controller的引用
            _controller = controller;
            //加载菜单
            LoadMenu();
            //初始化任务栏图标
            taskBarIcon = new NotifyIcon();
            UpdateIconAndText();
            taskBarIcon.Visible = true;
            taskBarIcon.ContextMenuStrip = taskBarMenu;
            taskBarIcon.MouseClick += TaskBarIcon_MouseClick;
            taskBarIcon.MouseDoubleClick += TaskBarIcon_MouseDoubleClick;

            //订阅controller的事件
            _controller.SSCStateChanged += Controller_SSCStateChanged;

            //将程序配置加载更新到菜单中
            LoadConifg();

            //处理默认配置相关响应
            if (_controller.CurConfig.HadLoadFailed)
            {
                //告知用户APP配置加载异常
                ShowBalloonTip("将暂时使用默认配置运行，建议立即检查配置文件。",
                    "程序的配置文件加载失败", ToolTipIcon.Warning);
            }
            if (_controller.CurConfig.IsDefault)
            {
                //首次运行，显示配置连接窗口
                //ShowDeviceListForm();
            }
        }

        private static ToolStripMenuItem CreateMenuItem(string text, EventHandler onClick,
            Image icon = null)
        {
            return new ToolStripMenuItem(text, icon, onClick);
        }

        private static ToolStripMenuItem CreateMenuGroup(string text, ToolStripItem[] items,
            Image icon = null)
        {
            return new ToolStripMenuItem(text, icon, items);
        }

        /// <summary>
        /// 加载任务栏图标右键菜单
        /// </summary>
        private void LoadMenu()
        {
            taskBarMenu = new ContextMenuStrip();
            taskBarMenu.Items.Add(this.sscItem = CreateMenuGroup("SSC", new ToolStripItem[] {
                this.disabledItem = CreateMenuItem("禁用", new EventHandler(Disabled_Click)),
                this.listenOnlyItem = CreateMenuItem("仅监听", new EventHandler(ListenOnly_Click)),
                this.bcAndListenItem = CreateMenuItem("自动（监听#广播）", new EventHandler(BcAndListen_Click)),
            }));
            taskBarMenu.Items.Add(this.allowCtrlItem = CreateMenuItem("允许控制", new EventHandler(AllowCtrl_Click)));
            taskBarMenu.Items.Add("-");
            taskBarMenu.Items.Add(this.autoStartupItem = CreateMenuItem("开机启动", new EventHandler(AutoStartup_Click)));
            taskBarMenu.Items.Add(this.settingItem = CreateMenuItem("设置…", new EventHandler(Setting_Click)));
            taskBarMenu.Items.Add("-");
            taskBarMenu.Items.Add(this.helpItem = CreateMenuGroup("帮助", new ToolStripItem[] {
                this.showLogItem = CreateMenuItem("显示日志…", new EventHandler(ShowLog_Click)),
                this.aboutItem = CreateMenuItem("关于…", new EventHandler(About_Click)),
            }));
            taskBarMenu.Items.Add("-");
            taskBarMenu.Items.Add(this.quitItem = CreateMenuItem("退出", new EventHandler(Quit_Click)));
        }

        /// <summary>
        /// 加载程序配置
        /// </summary>
        private void LoadConifg()
        {
            AppConfig config = _controller.CurConfig;
            UpdateSSCState(config);
            allowCtrlItem.Checked = config.AllowCtrlOperation;
            allowCtrlItem.CheckOnClick = true;
            autoStartupItem.Checked = AutoStartup.CheckStatus();
            autoStartupItem.CheckOnClick = true;
        }

        /// <summary>
        /// 更新SSC服务模式菜单的选中项
        /// </summary>
        private void UpdateSSCState(AppConfig config)
        {
            disabledItem.Checked = !config.IsSSCEnabled;
            listenOnlyItem.Checked = config.IsSSCEnabled && !config.IsBCEnabled;
            bcAndListenItem.Checked = config.IsBCEnabled && config.IsBCEnabled;
        }

        /// <summary>
        /// 保存修改后的程序配置
        /// </summary>
        private void SaveNewConfig(AppConfig config)
        {
            //通过controller执行保存操作
            if (!_controller.SaveConfig(config))
            {
                //保存失败，告知用户
                MessageBox.Show($"配置已变更，但保存失败！{Environment.NewLine}注意：SSC依然会执行您所选的操作。",
                    "配置保存失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        /// <summary>
        /// 更新任务栏图标
        /// </summary>
        private void UpdateIconAndText()
        {
            int iconSize;           //图标尺寸
            string iconStateStr;    //图标类型
            string text;            //图标提示文本

            Graphics graphics = Graphics.FromHwnd(IntPtr.Zero);
            //获取屏幕DPI用以判断使用什么尺寸的图标
            int dpi = (int)graphics.DpiX;
            graphics.Dispose(); //及时释放资源

            if (dpi < 97)
            {
                // dpi = 96;
                iconSize = 64;
            }
            else if (dpi < 121)
            {
                // dpi = 120;
                iconSize = 96;
            }
            else
            {
                // dpi > 120
                iconSize = 128;
            }

            AppConfig config = _controller.CurConfig;
            if (!config.IsSSCEnabled)
            {
                //禁用
                iconStateStr = "OFF";
                text = "已禁用";
            }
            else if (!config.IsBCEnabled)
            {
                //仅监听
                iconStateStr = "ON";
                text = "监听中";
            }
            else if (_controller.IsBCStop)
            {
                //自动_仅监听
                iconStateStr = "ON_Listen";
                text = "监听中";
            }
            else
            {
                //自动_监听#广播
                iconStateStr = "ON_All";
                text = "监听#广播";
            }

            //设置图标
            taskBarIcon.Icon = Icon.FromHandle(((Bitmap)Resources.ResourceManager.GetObject(
                $"Ctrl{iconStateStr}{iconSize}", Resources.Culture)).GetHicon());

            //设置提示文本
            // TODO: 如果当前已连接某个设备，则在提示文本里换行加上“已连接：NickName”
            text = $"{SysUtil.GetAssemblyTitle()} {SysUtil.GetVersionStr(4)}{Environment.NewLine}" +
                $"SSC：{text}";
            if (text.Length > 63)
            {
                //图标提示文本最大长度为63，超出长度则截断
                text = text.Substring(0, 63 - 3) + "...";
            }
            taskBarIcon.Text = text;
        }

        /// <summary>
        /// 显示一条任务栏通知
        /// </summary>
        /// <param name="text">通知内容</param>
        /// <param name="title">通知标题</param>
        /// <param name="tipIcon">通知图标</param>
        public void ShowBalloonTip(string text, string title = null,
            ToolTipIcon tipIcon = ToolTipIcon.Info)
        {
            taskBarIcon.ShowBalloonTip(5000, title ?? SysUtil.GetAssemblyTitle(), text, tipIcon);
        }

        #region Event_Handlers

        /// <summary>
        /// 全局控制器_SSC控制状态变更事件
        /// </summary>
        private void Controller_SSCStateChanged(object sender, EventArgs e)
        {
            //更新任务栏图标和其提示文本
            UpdateIconAndText();
        }

        /// <summary>
        /// 退出_点击事件
        /// </summary>
        private void Quit_Click(object sender, EventArgs e)
        {
            //关闭controller
            _controller.Exit();
            //关闭任务栏图标
            taskBarIcon.Visible = false;
            taskBarIcon.Dispose();
            //退出程序
            Application.Exit();
        }

        /// <summary>
        /// 禁用_点击事件
        /// </summary>
        private void Disabled_Click(object sender, EventArgs e)
        {
            //修改程序配置
            AppConfig config = _controller.CurConfig;
            config.IsBCEnabled = false;
            config.IsSSCEnabled = false;
            //更新菜单
            UpdateSSCState(config);
            //保存配置
            SaveNewConfig(config);
            //执行禁用操作
            _controller.StopSSC();
        }

        /// <summary>
        /// 仅监听_点击事件
        /// </summary>
        private void ListenOnly_Click(object sender, EventArgs e)
        {
            //修改程序配置
            AppConfig config = _controller.CurConfig;
            config.IsBCEnabled = false;
            config.IsSSCEnabled = true;
            //更新菜单
            UpdateSSCState(config);
            //保存配置
            SaveNewConfig(config);
            //执行启动操作
            _controller.StartSSC();
        }

        /// <summary>
        /// 自动（监听#广播）_点击事件
        /// </summary>
        private void BcAndListen_Click(object sender, EventArgs e)
        {
            //修改程序配置
            AppConfig config = _controller.CurConfig;
            config.IsSSCEnabled = true;
            config.IsBCEnabled = true;
            //更新菜单
            UpdateSSCState(config);
            //保存配置
            SaveNewConfig(config);
            //执行启动操作
            _controller.StartSSC();
        }

        /// <summary>
        /// 允许控制_点击事件
        /// </summary>
        private void AllowCtrl_Click(object sender, EventArgs e)
        {
            //修改程序配置
            _controller.CurConfig.AllowCtrlOperation = allowCtrlItem.Checked;
            //保存配置
            SaveNewConfig(_controller.CurConfig);
        }

        /// <summary>
        /// 开机启动_点击事件
        /// </summary>
        private void AutoStartup_Click(object sender, EventArgs e)
        {
            //修改开机启动配置
            if (!AutoStartup.Toggle(autoStartupItem.Checked))
            {
                //切换失败，通知用户
                MessageBox.Show("注册表修改失败！", "SSC开机启动状态切换失败",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            //更新实际状态
            autoStartupItem.Checked = AutoStartup.CheckStatus();
        }

        /// <summary>
        /// 设置_点击事件
        /// </summary>
        private void Setting_Click(object sender, EventArgs e)
        {
            if (File.Exists(AppConfig.CONFIG_FILE_NAME))
            {
                //文件存在，目前仅实现打开资源管理器
                Process.Start("explorer.exe", @"/select, " + AppConfig.CONFIG_FILE_NAME);
            }
            else
            {
                ShowBalloonTip("未找到程序配置文件");
            }
        }

        /// <summary>
        /// 显示日志_点击事件
        /// </summary>
        private void ShowLog_Click(object sender, EventArgs e)
        {
            if (File.Exists(Logging.LogFilePath))
            {
                //文件存在，目前仅实现打开资源管理器
                Process.Start("explorer.exe", @"/select, " + Logging.LogFilePath);
            }
            else
            {
                ShowBalloonTip("未找到日志文件");
            }
        }

        /// <summary>
        /// 关于_点击事件
        /// </summary>
        private void About_Click(object sender, EventArgs e)
        {
            //打开项目地址
            Process.Start("https://github.com/ChrysanHua/ShakeSocketController-Windows");
        }

        /// <summary>
        /// 任务栏图标的鼠标单击事件
        /// </summary>
        private void TaskBarIcon_MouseClick(object sender, MouseEventArgs e)
        {
            //只处理鼠标中键点击
            if (e.Button == MouseButtons.Middle)
            {
                //显示日志，直接调用同一个点击事件
                ShowLog_Click(sender, e);
            }
        }

        /// <summary>
        /// 任务栏图标的鼠标双击事件
        /// </summary>
        private void TaskBarIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //只处理鼠标左键双击
            if (e.Button == MouseButtons.Left)
            {
                // TODO: 显示配置连接窗口
                //ShowDeviceListForm();
            }
        }

        #endregion

        private void ShowDeviceListForm()
        {
            if (deviceListForm != null)
            {
                deviceListForm.Activate();
            }
            else
            {
                deviceListForm = new DeviceListForm(_controller);
                deviceListForm.Show();
                deviceListForm.Activate();
                deviceListForm.FormClosed += DeviceListForm_FormClosed;
            }
        }

        private void DeviceListForm_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            deviceListForm.Dispose();
            deviceListForm = null;

            _controller.Exit();
            Application.Exit();
        }
    }
}
