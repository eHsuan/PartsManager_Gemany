using PartsManager.Api.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Diagnostics;
using Microsoft.Win32;

// --- 1. Serilog 設定 (Log to File) ---
string logPath = Path.Combine(AppContext.BaseDirectory, "logs", "parts-api-.txt");
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File(logPath, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
    .CreateLogger();

try
{
    // --- 0. 先行初始化語系設定 (為了讓防呆警告顯示正確語系) ---
    string configPath = Path.Combine(AppContext.BaseDirectory, "config.ini");
    var ini = new PartsManager.Shared.IniHelper(configPath);
    string lang = ini.Read("System", "Language", "zh-TW");
    PartsManager.Shared.Resources.LocalizationService.SetLanguage(lang);

    // --- 檢查是否僅執行資料庫遷移 (用於安裝程式強制更新資料庫) ---
    bool migrateOnly = args.Contains("--migrate-only");

    // --- 單一實例檢查 (Mutex 防呆) ---
    using var mutex = new Mutex(true, "Global\\PartsManager.Api.ServerInstance", out bool createdNew);
    if (!createdNew && !migrateOnly)
    {
        // 僅在一般啟動模式下顯示警告；如果是安裝程式呼叫的遷移模式，則安靜退出或跳過
        MessageBox.Show(PartsManager.Shared.Resources.LocalizationService.GetString("Msg_InstanceRunning"), 
            PartsManager.Shared.Resources.LocalizationService.GetString("Msg_SystemWarning"), 
            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return;
    }

    var builder = WebApplication.CreateBuilder(args);
    builder.Configuration.AddIniFile("config.ini", optional: true, reloadOnChange: true);
    builder.Host.UseSerilog();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddControllers();


    builder.Services.AddSingleton<PartsManager.Api.Services.GoogleDriveService>();
    builder.Services.AddHostedService<PartsManager.Api.Services.BackupBackgroundService>();
    builder.Services.AddScoped<PartsManager.Api.Services.IStockService, PartsManager.Api.Services.StockService>();

    // --- 4. 再次確認語系 (從 Configuration 讀取，以防 runtime 變更) ---
    // lang = builder.Configuration["System:Language"] ?? "zh-TW";
    // PartsManager.Shared.Resources.LocalizationService.SetLanguage(lang);

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

    var app = builder.Build();

    // --- 自動執行資料庫遷移 (Apply Migrations) ---
    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
        Log.Information("Database migration completed.");
        
        if (migrateOnly)
        {
            Log.Information("Migration-only mode: Exiting successfully.");
            return; // 遷移完成後直接退出
        }
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.MapControllers();

    // --- 2. 自動啟動註冊 (Registry Auto-run) ---
    try
    {
        string? appPath = Process.GetCurrentProcess().MainModule?.FileName;
        using RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
        if (key != null && !string.IsNullOrEmpty(appPath))
        {
            key.SetValue("PartsManagerApi", $"\"{appPath}\"");
        }
    }
    catch (Exception ex)
    {
        Log.Error(ex, "無法設定自動啟動");
    }

    // --- 3. 系統托盤 (Tray Icon) 控制邏輯 ---
    var cts = new CancellationTokenSource();
    Thread trayThread = new Thread(() =>
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        string version = typeof(Program).Assembly.GetName().Version?.ToString(3) ?? "1.1.0";
        var notifyIcon = new NotifyIcon
        {
            Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath),
            Text = $"{PartsManager.Shared.Resources.LocalizationService.GetString("App_Title")} (Backend) v{version}",
            Visible = true
        };

        var contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add(PartsManager.Shared.Resources.LocalizationService.GetString("Tray_OpenLogs"), null, (s, e) => {
            try 
            { 
                string logFolder = Path.Combine(AppContext.BaseDirectory, "logs");
                if (!Directory.Exists(logFolder)) Directory.CreateDirectory(logFolder);
                Process.Start(new ProcessStartInfo("explorer.exe", logFolder) { UseShellExecute = true }); 
            }
            catch { }
        });
        contextMenu.Items.Add(new ToolStripSeparator());
        contextMenu.Items.Add(PartsManager.Shared.Resources.LocalizationService.GetString("Tray_Exit"), null, (s, e) => {
            if (MessageBox.Show(PartsManager.Shared.Resources.LocalizationService.GetString("Tray_WarnClose"), 
                PartsManager.Shared.Resources.LocalizationService.GetString("Tray_WarnTitle"), MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                notifyIcon.Visible = false;
                cts.Cancel();
                Application.Exit();
            }
        });

        notifyIcon.ContextMenuStrip = contextMenu;
        Application.Run();
    });

    trayThread.SetApartmentState(ApartmentState.STA);
    trayThread.Start();

    Log.Information("PartsManager API Server 啟動中...");
    app.RunAsync(cts.Token).Wait();
}
catch (Exception ex)
{
    Log.Fatal(ex, "API Server 發生致命錯誤而停止");
}
finally
{
    Log.CloseAndFlush();
}

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
