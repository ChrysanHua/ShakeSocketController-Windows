using ShakeSocketController.Controller;
using ShakeSocketController.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ShakeSocketController.Views
{
    /// <summary>
    /// 配置连接窗口
    /// </summary>
    public partial class DeviceListForm : Form
    {
        private readonly TransactionController _controller; //全局控制器的引用
        private List<DeviceInfo> curDeviceList;             //设备连接列表副本

        public DeviceListForm(TransactionController controller)
        {
            InitializeComponent();
            this.Font = SystemFonts.MessageBoxFont;
            listBoxDeviceList.Dock = DockStyle.Fill;
            this.PerformLayout();
            this.Icon = Program.MenuController.GetCurIconAndText(out string ignore);

            _controller = controller;
            //订阅controller的相关事件
            _controller.DeviceListChanged += Controller_DeviceListChanged;
            _controller.SSCStateChanged += Controller_SSCStateChanged;
            _controller.DeviceInfoChanged += Controller_DeviceInfoChanged;

            //初始化窗口的设备列表
            UpdateDeviceListBox(_controller.CurDeviceList);
        }

        /// <summary>
        /// 窗口显示事件
        /// </summary>
        private void DeviceListForm_Shown(object sender, EventArgs e)
        {
            DeviceInfo ctrlDevice = _controller.CurCtrlDeviceInfo;
            if (listBoxDeviceList.Items.Count > 0 && ctrlDevice != null)
            {
                //选中当前连接的项
                listBoxDeviceList.SelectedIndex = curDeviceList.IndexOf(ctrlDevice);
            }
            else
            {
                //没有选中任何项，初始化界面
                LoadDeviceInfo(null);
            }
            //设置按钮的可用性
            btnDisConnect.Enabled = _controller.IsCtrlConnected;
        }

        /// <summary>
        /// 更新窗口的设备列表
        /// </summary>
        /// <param name="newDeviceList">新设备列表，NonNull</param>
        private void UpdateDeviceListBox(List<DeviceInfo> newDeviceList)
        {
            //先清空ListBox
            listBoxDeviceList.Items.Clear();
            //更新列表副本
            curDeviceList?.Clear();
            curDeviceList = newDeviceList;
            //更新ListBox
            listBoxDeviceList.Items.AddRange(
                curDeviceList.Select(item => item.FriendlyName).ToArray());
        }

        /// <summary>
        /// 加载设备连接信息到UI上
        /// </summary>
        /// <param name="info">要加载的设备连接，传入null则重置UI</param>
        private void LoadDeviceInfo(DeviceInfo info)
        {
            if (info == null)
            {
                //重置信息
                tbNickName.ResetText();
                tbDeviceID.ResetText();
                tbDeviceName.ResetText();
                tbUserName.ResetText();
                tbIP.ResetText();
                tbClientVersion.ResetText();
                cbAutoConnect.Checked = false;
                cbDisabled.Enabled = true;
                cbDisabled.Checked = false;
                labelDeviceState.ResetText();
                btnDeviceModify.Enabled = false;
                btnDeviceDelete.Enabled = false;
            }
            else
            {
                //设置设备信息
                tbNickName.Text = info.NickName;
                tbDeviceID.Text = info.DeviceID;
                tbDeviceName.Text = info.DeviceName;
                tbUserName.Text = info.UserName;
                tbIP.Text = info.IP.ToString();
                tbClientVersion.Text = info.ClientVersion;
                //勾选框
                cbAutoConnect.Checked = info.IsAutoConnect;
                cbDisabled.Checked = info.IsDisabled;
                //状态文本
                if (info.IsDisabled)
                {
                    labelDeviceState.Text = "已禁用";
                }
                else if (info.IsConnected)
                {
                    labelDeviceState.Text = "已连接";
                }
                else if (info.HadConnected)
                {
                    labelDeviceState.Text = "已保存";
                }
                else
                {
                    labelDeviceState.Text = string.Empty;
                }
                //可用性
                btnDeviceModify.Enabled = true;
                // TODO: 已连接状态下，暂时不允许【禁用】和【删除】，后续再考虑
                btnDeviceDelete.Enabled = !info.IsConnected;
                cbDisabled.Enabled = !info.IsConnected;
            }
        }

        /// <summary>
        /// 设备连接列表选中项变更事件
        /// </summary>
        private void lbDeviceList_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBoxDeviceList.SelectedIndex;
            if (index >= 0)
            {
                //加载选中项的设备信息
                LoadDeviceInfo(curDeviceList[index]);
            }
            else
            {
                //重置信息
                LoadDeviceInfo(null);
            }
        }

        /*private void InvokeSafely(object sender, EventArgs e, EventHandler eventHandler)
        {
            //参考代码，把“以线程安全方式处理UI”的调用逻辑提取为单独的方法，但增加了代码复杂度，暂不考虑。
            Logging.Debug($"InvokeSafely: threadID is " +
                $"{Thread.CurrentThread.ManagedThreadId}, sender is {sender}");
            if (this.InvokeRequired)        //判断是否跨线程执行
            {
                this.Invoke(eventHandler, sender, e);
            }
            else if (this.IsHandleCreated)  //确保窗体句柄已创建
            {
                eventHandler.Invoke(sender, e);
            }
        }*/

        /// <summary>
        /// 全局设备连接列表元素变更事件
        /// </summary>
        private void Controller_DeviceListChanged(object sender, EventArgs e)
        {
            if (this.InvokeRequired)        //判断是否跨线程执行
            {
                this.Invoke(new EventHandler(Controller_DeviceListChanged));
            }
            else if (this.IsHandleCreated)  //确保窗体句柄已创建
            {
                //先记录当前选中项
                int oldSelectedIndex = listBoxDeviceList.SelectedIndex;
                //更新列表
                UpdateDeviceListBox(_controller.CurDeviceList);

                if (listBoxDeviceList.Items.Count > 0)
                {
                    //设置选中项
                    if (oldSelectedIndex >= 0)
                    {
                        listBoxDeviceList.SelectedIndex = Math.Min(oldSelectedIndex,
                            listBoxDeviceList.Items.Count - 1);
                    }
                    else if (_controller.CurCtrlDeviceInfo != null)
                    {
                        //更新前未选中任何项，现在自动选中当前连接的项
                        listBoxDeviceList.SelectedIndex = curDeviceList.IndexOf(
                            _controller.CurCtrlDeviceInfo);
                    }
                }
                else
                {
                    //没有设备，重置UI
                    LoadDeviceInfo(null);
                }
            }
        }

        /// <summary>
        /// SSC状态变更事件
        /// </summary>
        private void Controller_SSCStateChanged(object sender, EventArgs e)
        {
            if (this.InvokeRequired)        //判断是否跨线程执行
            {
                this.Invoke(new EventHandler(Controller_SSCStateChanged));
            }
            else if (this.IsHandleCreated)  //确保窗体句柄已创建
            {
                this.Icon = Program.MenuController.GetCurIconAndText(out string ignore);
            }
        }

        /// <summary>
        /// 单个设备连接信息元素变更事件
        /// </summary>
        private void Controller_DeviceInfoChanged(object sender, DeviceInfoEventArgs e)
        {
            //直接更新整个列表
            Controller_DeviceListChanged(sender, e);
            //如果是Ctrl连接状态变更，调整按钮的可用性
            if (e.IsConnectStateChanged)
            {
                btnDisConnect.Enabled = _controller.IsCtrlConnected;
            }
        }

        /// <summary>
        /// 保存修改按钮
        /// </summary>
        private void btnDeviceModify_Click(object sender, EventArgs e)
        {
            int index = listBoxDeviceList.SelectedIndex;
            if (index >= 0)
            {
                DeviceInfo info = curDeviceList[index];
                //执行修改
                info.NickName = tbNickName.Text.Trim();
                info.IsAutoConnect = cbAutoConnect.Checked;
                info.IsDisabled = cbDisabled.Checked;
                //通知controller
                _controller.CustomModifyDevice(info);
            }
        }

        /// <summary>
        /// 删除按钮
        /// </summary>
        private void btnDeviceDelete_Click(object sender, EventArgs e)
        {
            int index = listBoxDeviceList.SelectedIndex;
            if (index >= 0)
            {
                DeviceInfo info = curDeviceList[index];
                if (MessageBox.Show($"确定要删除【{info.NickName}】吗？", "删除确定",
                    MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    //通知controller执行删除
                    _controller.CustomDeleteDevice(info);
                }
            }
        }

        /// <summary>
        /// 断开连接按钮
        /// </summary>
        private void btnDisConnect_Click(object sender, EventArgs e)
        {
            if (_controller.IsCtrlConnected)
            {
                DeviceInfo ctrlInfo = _controller.CurCtrlDeviceInfo;
                if (MessageBox.Show($"确定要断开对【{ctrlInfo.NickName}】的连接吗？", "断开连接",
                    MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    //通知controller执行断开连接
                    _controller.DisconnectCtrl();
                }
            }
        }

        /// <summary>
        /// 关闭按钮
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 窗口关闭事件
        /// </summary>
        private void DeviceListForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //取消订阅controller的相关事件
            _controller.DeviceListChanged -= Controller_DeviceListChanged;
            _controller.SSCStateChanged -= Controller_SSCStateChanged;
            _controller.DeviceInfoChanged -= Controller_DeviceInfoChanged;
        }

    }
}
