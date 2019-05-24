using ShakeSocketController.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShakeSocketController.Views
{
    class MenuViewController
    {
        private TransactionController _controller;

        private DeviceListForm deviceListForm;

        public MenuViewController(TransactionController controller)
        {
            _controller = controller;

            ShowDeviceListForm();
        }

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

            _controller.Stop();
            Application.Exit();
        }
    }
}
