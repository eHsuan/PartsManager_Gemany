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

                // 原本讀 IP，現在改讀 Windows 印表機名稱
                // 例如從設定檔讀取： string printerName = "cab EOS5/300";
                string printerName = GlobalSettings.PrinterName;

                if (string.IsNullOrEmpty(printerName))
                {
                    throw new Exception("印表機名稱未設定");
                }

                // JScript for cab EOS5
                // m m : Unit in millimeters
                // J : Start job
                // T x,y,rotation,font,size;text
                // B x,y,rotation,barcode_type,height,narrow_bar_width;data
                // A 1 : Print 1 quantity

                //string jscript = 
                //    "m m\n" +
                //    "J\n" +
                //    "S l1;0,0,20,23,60\n" +
                //    $"T 5,5,0,3,pt 12;\"品名: {name}\"\n" +
                //    $"T 5,12,0,3,pt 12;\"規格: {spec}\"\n" +
                //    $"T 5,19,0,3,pt 12;\"儲位: {location}\"\n" +
                //    $"B 5,26,0,CODE128,8,0.3;\"{partNo}\"\n" +
                //    $"T 5,36,0,3,pt 10;\"{partNo}\"\n" +
                //    "A 1\n";

                string safeName = name?.Replace("\r", "").Replace("\n", " ") ?? "";
                string safeSpec = spec?.Replace("\r", "").Replace("\n", " ") ?? "";
                string safeLocation = location?.Replace("\r", "").Replace("\n", " ") ?? "";
                string safePartNo = partNo?.Replace("\r", "").Replace("\n", " ") ?? "";

                string jscript =
                    "m m\n" +
                    "J\n" +
                    "S l1;0,0,20,23,60\n" +
                    $"T 4,2,0,3,pt 6;{safeName}\n" +
                    // -- 上半部：條碼 (從最左邊 X=4 開始，高度設為 7mm，線條 0.25) --
                    $"B 4,3,0,CODE128,7,0.25;{safePartNo}\n" +

                    // -- 下半部：文字 (分成左右兩塊來顯示，避免擠在一起) --
                    $"T 3,13,0,3,pt 6;{safePartNo}\n" +
                    
                    $"T 30,13,0,3,pt 6;Lagerplatz:{safeLocation}\n" +
                    $"T 3,16,0,3,pt 6;{safeSpec}\n" +
                    "A 1\n";

                //using (var client = new TcpClient())
                //{
                //    // 2 seconds timeout for connection
                //    var connectTask = client.ConnectAsync(ip, port);
                //    if (await Task.WhenAny(connectTask, Task.Delay(2000)) != connectTask)
                //    {
                //        throw new Exception("連線標籤機超時，請確認 IP 與網路連線狀態。");
                //    }

                //    using (var stream = client.GetStream())
                //    {
                //        byte[] data = Encoding.UTF8.GetBytes(jscript);
                //        await stream.WriteAsync(data, 0, data.Length);
                //    }
                //}

                // 3. 透過 USB (Windows Spooler) 傳送指令
                // 為了不卡住 UI，包裝成非同步執行
                bool printResult = await Task.Run(() =>
                {
                    return RawPrinterHelper.SendStringToPrinter(printerName, jscript);
                });

                if (!printResult)
                {
                    throw new Exception("傳送至印表機失敗，請確認 USB 連線與印表機名稱是否正確。");
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
