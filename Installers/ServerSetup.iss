[Setup]
; 建立固定的 AppId 以便偵測既有安裝
AppId={{D9049802-E27E-431C-A306-5A0832B2DA7A}
AppName=PartsManager Server
AppVersion=1.3.0
DefaultDirName={commonpf}\PartsManager\Server
DefaultGroupName=PartsManager Server
OutputDir=Output
OutputBaseFilename=PartsManager_Server_Setup_v130
Compression=lzma
SolidCompression=yes
PrivilegesRequired=admin
; 讓 Inno Setup 自動處理執行中程序的偵測與關閉
CloseApplications=yes
RestartApplications=yes
; 記憶上次安裝路徑
UsePreviousAppDir=yes

[Files]
; 程式本體：一律更新至最新版 (使用相對路徑)
Source: "..\PartsManager.Api\bin\Release\net8.0-windows\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

; 資料庫檔案：重要！使用 onlyifdestfiledoesnotexist 旗標確保不覆蓋用戶現有資料
Source: "..\DB\Parts.db"; DestDir: "{app}\DB"; Flags: onlyifdoesntexist uninsneveruninstall

[Dirs]
Name: "{app}\DB"; Permissions: users-full
Name: "{app}\Attachments"; Permissions: users-full
Name: "{app}\logs"; Permissions: users-full

[Icons]
Name: "{group}\PartsManager API Server"; Filename: "{app}\PartsManager.Api.exe"

[Code]
var
  IsUpgrade: Boolean;

// 強化版更新偵測
function CheckIsUpgrade: Boolean;
var
  Key: string;
begin
  Key := 'Software\Microsoft\Windows\CurrentVersion\Uninstall\{D9049802-E27E-431C-A306-5A0832B2DA7A}_is1';
  Result := RegKeyExists(HKLM64, Key) or 
            RegKeyExists(HKLM32, Key) or 
            RegKeyExists(HKCU64, Key) or 
            RegKeyExists(HKCU32, Key);
end;

procedure InitializeWizard;
begin
  IsUpgrade := CheckIsUpgrade;
end;

procedure CurPageChanged(CurPageID: Integer);
begin
  // Update text to English if an existing installation is detected
  if IsUpgrade then
  begin
    if CurPageID = wpWelcome then
    begin
      WizardForm.WelcomeLabel1.Caption := 'Welcome to the PartsManager Server Update Wizard';
      WizardForm.WelcomeLabel2.Caption := 'This wizard will guide you through the update process for PartsManager Server.' + #13#10#13#10 +
        'It is recommended that you close all other applications before continuing.' + #13#10#13#10 +
        'Click Next to continue, or Cancel to exit the Update Wizard.';
    end;

    if CurPageID = wpSelectDir then
    begin
      WizardForm.PageNameLabel.Caption := 'Confirm Update Directory';
      WizardForm.PageDescriptionLabel.Caption := 'Please confirm the installation path for the PartsManager Server update.';
    end;

    if CurPageID = wpReady then
    begin
      WizardForm.PageNameLabel.Caption := 'Ready to Update';
      WizardForm.PageDescriptionLabel.Caption := 'Setup is now ready to begin updating PartsManager Server on your computer.';
    end;

    if CurPageID = wpInstalling then
    begin
      WizardForm.StatusLabel.Caption := 'Updating existing files...';
    end;

    if CurPageID = wpFinished then
    begin
      WizardForm.FinishedLabel.Caption := 'PartsManager Server has been successfully updated.';
    end;
  end;
end;

// 在安裝前偵測是否有舊版正在運行並嘗試關閉
function InitializeSetup(): Boolean;
var
  ResultCode: Integer;
begin
  Result := True;
  // 強制關閉背景 API 服務，避免檔案被鎖定
  ShellExec('', 'taskkill', '/f /im PartsManager.Api.exe', '', SW_HIDE, ewWaitUntilTerminated, ResultCode);
end;

function NextButtonClick(CurPageID: Integer): Boolean;
var
  FindRec: TFindRec;
  IsEmpty: Boolean;
begin
  Result := True;
  if (CurPageID = wpSelectDir) and (not IsUpgrade) then
  begin
    if DirExists(WizardDirValue) then
    begin
      IsEmpty := True;
      if FindFirst(WizardDirValue + '\*', FindRec) then
      begin
        try
          repeat
            if (FindRec.Name <> '.') and (FindRec.Name <> '..') then
            begin
              IsEmpty := False;
              Break;
            end;
          until not FindNext(FindRec);
        finally
          FindClose(FindRec);
        end;
      end;
      
      if not IsEmpty then
      begin
        MsgBox('為了避免檔案衝突，全新安裝只能安裝在「空的資料夾」中！' + #13#10 + '請重新選擇一個不存在或空的資料夾。', mbError, MB_OK);
        Result := False;
      end;
    end;
  end;
end;

procedure CurStepChanged(CurStep: TSetupStep);
var
  ConfigPath: string;
  DBPath: string;
  AttachPath: string;
begin
  if CurStep = ssPostInstall then
  begin
    ConfigPath := ExpandConstant('{app}\config.ini');
    DBPath := ExpandConstant('{app}\DB\Parts.db');
    AttachPath := ExpandConstant('{app}\Attachments');
    
    // 更新配置檔中的路徑資訊
    SetIniString('ConnectionStrings', 'DefaultConnection', 'Data Source=' + DBPath, ConfigPath);
    SetIniString('System', 'AttachmentPath', AttachPath, ConfigPath);
  end;
end;

[Run]
; 1. Mandatory Database Migration (Always runs)
Filename: "{app}\PartsManager.Api.exe"; Parameters: "--migrate-only"; StatusMsg: "Updating database schema..."; Flags: runhidden waituntilterminated

; 2. Optional Application Launch
Filename: "{app}\PartsManager.Api.exe"; Description: "Start PartsManager API Service"; Flags: nowait postinstall
