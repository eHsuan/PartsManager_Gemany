using System;
using System.Windows.Forms;

namespace PartsManager.Client
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // --- 單一實例檢查 (Mutex 防呆) ---
            using (var mutex = new System.Threading.Mutex(true, "Global\\PartsManager.Client.Instance", out bool createdNew))
            {
                if (!createdNew)
                {
                    // 如果已有實例在執行，顯示在地化警告後退出
                    MessageBox.Show(PartsManager.Shared.Resources.LocalizationService.GetString("Msg_InstanceRunning"), 
                        PartsManager.Shared.Resources.LocalizationService.GetString("Msg_SystemWarning"), 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // 初始化語系
                string lang = GlobalSettings.Language;
                PartsManager.Shared.Resources.LocalizationService.SetLanguage(lang);

                while (true)
                {
                    UserSession.Clear(); // 每次迴圈重置 Session
                    LoginForm login = new LoginForm();
                    if (login.ShowDialog() == DialogResult.OK)
                    {
                        Application.Run(new MainForm());
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}

