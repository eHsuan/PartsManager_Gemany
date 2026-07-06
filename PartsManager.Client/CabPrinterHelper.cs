using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PartsManager.Client
{
    public static class CabPrinterHelper
    {
        /// <summary>
        /// Sends JScript to cab EOS5 printer over TCP to print a label.
        /// </summary>
        public static async Task<bool> PrintLabelAsync(string name, string spec, string location, string partNo)
        {
            try
            {
                string ip = GlobalSettings.LabelPrinterIP;
                int port = GlobalSettings.LabelPrinterPort;

                if (string.IsNullOrEmpty(ip))
                {
                    throw new Exception("標籤機 IP 未設定 (config.ini [Printer] LabelPrinterIP)");
                }

                // JScript for cab EOS5
                // m m : Unit in millimeters
                // J : Start job
                // T x,y,rotation,font,size;text
                // B x,y,rotation,barcode_type,height,narrow_bar_width;data
                // A 1 : Print 1 quantity
                
                string jscript = 
                    "m m\n" +
                    "J\n" +
                    "S l1;0,0,68,40\n" +
                    $"T 5,5,0,3,pt 12;品名: {name}\n" +
                    $"T 5,12,0,3,pt 12;規格: {spec}\n" +
                    $"T 5,19,0,3,pt 12;儲位: {location}\n" +
                    $"B 5,26,0,CODE128,8,0.3;{partNo}\n" +
                    $"T 5,36,0,3,pt 10;{partNo}\n" +
                    "A 1\n";

                using (var client = new TcpClient())
                {
                    // 2 seconds timeout for connection
                    var connectTask = client.ConnectAsync(ip, port);
                    if (await Task.WhenAny(connectTask, Task.Delay(2000)) != connectTask)
                    {
                        throw new Exception("連線標籤機超時，請確認 IP 與網路連線狀態。");
                    }

                    using (var stream = client.GetStream())
                    {
                        byte[] data = Encoding.UTF8.GetBytes(jscript);
                        await stream.WriteAsync(data, 0, data.Length);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"標籤列印失敗: {ex.Message}");
            }
        }
    }
}
