[Setup]
; 建立固定的 AppId 以便偵測既有安裝
AppId={{990A738B-309F-40EF-A44D-B177CF1F7D9D}
AppName=PartsManager Client
AppVersion=1.3.0
DefaultDirName={commonpf}\PartsManager\Client
DefaultGroupName=PartsManager Client
OutputDir=Output
OutputBaseFilename=PartsManager_Client_Setup_v130
Compression=lzma
SolidCompression=yes
PrivilegesRequired=admin
; 讓 Inno Setup 自動處理執行中程序的偵測與關閉
CloseApplications=yes
RestartApplications=yes
; 重要：記憶上次安裝路徑
UsePreviousAppDir=yes

[Files]
; 程式本體：一律更新至最新版 (使用相對路徑，排除 config.ini 避免被 ignoreversion 覆蓋)
Source: "..\PartsManager.Client\bin\Release\net452\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs; Excludes: "config.ini"

; 設定檔：使用 onlyifdestfiledoesnotexist 確保不覆蓋用戶現有的連線設定
Source: "..\PartsManager.Client\config.ini"; DestDir: "{app}"; Flags: onlyifdoesntexist uninsneveruninstall

[Icons]
Name: "{commonprograms}\PartsManager Client"; Filename: "{app}\PartsManager.Client.exe"
Name: "{commondesktop}\PartsManager Client"; Filename: "{app}\PartsManager.Client.exe"

[Code]
var
  ServerIPPage: TInputQueryWizardPage;
  IsUpgrade: Boolean;

// 強化版更新偵測：掃描所有 32/64 位元註冊表視圖
function CheckIsUpgrade: Boolean;
var
  Key: string;
begin
  Key := 'Software\Microsoft\Windows\CurrentVersion\Uninstall\{990A738B-309F-40EF-A44D-B177CF1F7D9D}_is1';
  Result := RegKeyExists(HKLM64, Key) or 
            RegKeyExists(HKLM32, Key) or 
            RegKeyExists(HKCU64, Key) or 
            RegKeyExists(HKCU32, Key);
end;

procedure CurPageChanged(CurPageID: Integer);
begin
  // Update text to English if an existing installation is detected
  if IsUpgrade then
  begin
    if CurPageID = wpWelcome then
    begin
      WizardForm.WelcomeLabel1.Caption := 'Welcome to the PartsManager Client Update Wizard';
      WizardForm.WelcomeLabel2.Caption := 'This wizard will guide you through the update process for PartsManager Client.' + #13#10#13#10 +
        'It is recommended that you close all other applications before continuing.' + #13#10#13#10 +
        'Click Next to continue, or Cancel to exit the Update Wizard.';
    end;
    
    if CurPageID = wpSelectDir then
    begin
      WizardForm.PageNameLabel.Caption := 'Confirm Update Directory';
      WizardForm.PageDescriptionLabel.Caption := 'Please confirm the installation path for the PartsManager Client update.';
    end;

    if CurPageID = wpReady then
    begin
      WizardForm.PageNameLabel.Caption := 'Ready to Update';
      WizardForm.PageDescriptionLabel.Caption := 'Setup is now ready to begin updating PartsManager Client on your computer.';
    end;

    if CurPageID = wpInstalling then
    begin
      WizardForm.StatusLabel.Caption := 'Updating existing files...';
    end;

    if CurPageID = wpFinished then
    begin
      WizardForm.FinishedLabel.Caption := 'PartsManager Client has been successfully updated.';
    end;
  end;
end;

// 在安裝前偵測是否有舊版正在運行並嘗試關閉
function InitializeSetup(): Boolean;
var
  ResultCode: Integer;
begin
  Result := True;
  // 強制關閉 Client 程式，避免檔案被鎖定
  ShellExec('', 'taskkill', '/f /im PartsManager.Client.exe', '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
end;

procedure InitializeWizard;
var
  OldIP: string;
  ConfigPath: string;
  PrevPath: string;
begin
  // 偵測是否為更新
  IsUpgrade := CheckIsUpgrade;

  // 1. 優先從註冊表尋找「上次真正的安裝路徑」以讀取 config.ini
  if RegQueryStringValue(HKLM64, 'Software\Microsoft\Windows\CurrentVersion\Uninstall\{990A738B-309F-40EF-A44D-B177CF1F7D9D}_is1', 'InstallLocation', PrevPath) or
     RegQueryStringValue(HKLM32, 'Software\Microsoft\Windows\CurrentVersion\Uninstall\{990A738B-309F-40EF-A44D-B177CF1F7D9D}_is1', 'InstallLocation', PrevPath) or
     RegQueryStringValue(HKCU64, 'Software\Microsoft\Windows\CurrentVersion\Uninstall\{990A738B-309F-40EF-A44D-B177CF1F7D9D}_is1', 'InstallLocation', PrevPath) or
     RegQueryStringValue(HKCU32, 'Software\Microsoft\Windows\CurrentVersion\Uninstall\{990A738B-309F-40EF-A44D-B177CF1F7D9D}_is1', 'InstallLocation', PrevPath) then
  begin
    ConfigPath := PrevPath + '\config.ini';
  end
  else
  begin
    // 2. 沒裝過，使用預設系統路徑 (!!! 絕對不要在這裡使用 {app} 常數 !!!)
    ConfigPath := ExpandConstant('{commonpf}\PartsManager\Client\config.ini');
  end;
  
  // 3. 檢查檔案是否存在，不存在就給初始預設值
  if FileExists(ConfigPath) then
    OldIP := GetIniString('Network', 'ServerIP', '127.0.0.1', ConfigPath)
  else
    OldIP := '127.0.0.1'; 

  // Create custom page to ask for Server IP
  ServerIPPage := CreateInputQueryPage(wpSelectDir,
    'Server Connection Settings', 'Please enter the API Server IP address',
    'If you do not know the Server IP, please contact your system administrator.');
  ServerIPPage.Add('Server IP:', False);
  
  // Pre-fill with old IP or default
  ServerIPPage.Values[0] := OldIP;
end;

procedure CurStepChanged(CurStep: TSetupStep);
var
  ConfigPath: string;
  ServerIP: string;
begin
  if CurStep = ssPostInstall then
  begin
    // 在這裡使用 {app} 是安全的，因為檔案已經開始寫入了
    ConfigPath := ExpandConstant('{app}\config.ini');
    ServerIP := ServerIPPage.Values[0];
    
    // 更新或建立 config.ini
    SetIniString('Network', 'ServerIP', ServerIP, ConfigPath);
    SetIniString('Network', 'ServerPort', '5000', ConfigPath);
  end;
end;

[Run]
Filename: "{app}\PartsManager.Client.exe"; Description: "Start PartsManager Client"; Flags: nowait postinstall
