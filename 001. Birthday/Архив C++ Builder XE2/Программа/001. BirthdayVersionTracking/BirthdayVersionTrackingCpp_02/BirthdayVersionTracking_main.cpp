//---------------------------------------------------------------------------

#include <vcl.h>
#pragma hdrstop

#include "BirthdayVersionTracking_main.h"
#include <Registry.hpp>
#include <stdio.h>
#include <IBServices.hpp>

int AppMajor, AppMinor, AppRelease, AppBuild;
int VerInfo[4];
//---------------------------------------------------------------------------
#pragma package(smart_init)
#pragma resource "*.dfm"
TMainForm *MainForm;
//---------------------------------------------------------------------------
__fastcall TMainForm::TMainForm(TComponent* Owner)
	: TForm(Owner)
{
  // �������� � ����������:
  TRegistry *reg = new TRegistry();

  reg->RootKey = HKEY_CURRENT_USER;
/*
  HKEY_CURRENT_USER - ������ ������� � ������� ��������� ��������� ����������
  ����� �������� ������������(������).
  �� ������ ������ ����������� ����� (Hive Keys). �� ������ ������ �������������
  ������� ��� ����� ������� (Registry Keys), �� ������� � ���������� (Subkeys) �
  �� ��������� � ����� � ��������� (Values).
*/
  reg->OpenKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run",true);  // �������� ������������� ���������� ������� � �������� �� ������

  bool regValue = reg->ValueExists(ExtractFileName(Application->ExeName));

  if(!regValue)     // �������� ������� � ������������
  {
	reg->WriteString(ExtractFileName(Application->ExeName),Application->ExeName); // �������� "Birthday.exe" ���� REG_SZ � ��������� ������
  }

  reg->CloseKey();  // �������� ���������� �������

  delete reg;
  reg = NULL;
}
//---------------------------------------------------------------------------
void __fastcall TMainForm::B_DeleteClick(TObject *Sender)
{
  // ������ �� �����������:
  TRegistry *reg = new TRegistry();

  reg->RootKey = HKEY_CURRENT_USER;
  reg->OpenKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run",true);
  reg->DeleteValue(ExtractFileName(Application->ExeName));                      // "Birthday.exe" ���� REG_SZ � ��������� ������
  reg->CloseKey();

  delete reg;
  reg = NULL;
}
//---------------------------------------------------------------------------
bool __fastcall TMainForm::GetAppVersion(char* FileName, int* VerInfo)  // ��������� ������ �����
{
  if(!FileExists(FileName))                       // ��������� ������� �����
  {
	return false;                                 // ���� ��� �-��� ���������
  }

  DWORD FSize = GetFileVersionInfoSize(FileName,NULL);  // ������ ���� � ������ �����

  if(FSize==0)
  {
	return false;                                 // ���� 0 ������� ���������
  }

  LPVOID pBlock = (char*)malloc(FSize);           // ����� ������ ��� �������� ������
  GetFileVersionInfo(FileName,NULL,FSize,pBlock); // �������� ������ ���������� � ������
  LPVOID MS;
  UINT LS;

  try
  {
	VerQueryValue(pBlock,"\\",&MS,&LS);           // ��������� ���������� �� �������
  }

  catch(...)
  {
	return false;                                 // � ������ ������ ������� ���������
  }

  VS_FIXEDFILEINFO FixedFileInfo;                 // ��������� � ����������� � ������ �����
  memmove(&FixedFileInfo, MS, LS);                // �������� ���������� � ���������

  DWORD FileVersionMS = FixedFileInfo.dwFileVersionMS;
  DWORD FileVersionLS = FixedFileInfo.dwFileVersionLS;

  VerInfo[0] = HIWORD(FileVersionMS);             // �������� ��������
  VerInfo[1] = LOWORD(FileVersionMS);             // � ����������� �� �������� ���������
  VerInfo[2] = HIWORD(FileVersionLS);
  VerInfo[3] = LOWORD(FileVersionLS);

  return true;                                    // ������� �������
}
//---------------------------------------------------------------------------
template <class T>
class GlobalMem
{
  public:
	GlobalMem(DWORD s):size(s){buf = (T) GlobalAlloc(GMEM_FIXED, size);}
	~GlobalMem(){GlobalFree(buf);}
	T operator()(){return buf;}
  private:
	T buf;
    DWORD size;
};

AnsiString __fastcall TMainForm::GetAnyAppParam(AnsiString appname,AnsiString pname)  // ��������� ������ �����
{
  //pname = ProductName,FileVersion,LegalCopyright,CompanyName � �.�.
  AnsiString ret("<Invalid parametr>");
  DWORD h;
  DWORD Size = GetFileVersionInfoSize(appname.c_str(), &h);
  if(Size == 0) return ret;

  GlobalMem<char*> buf(Size);

  if(GetFileVersionInfo(appname.c_str(), h, Size, buf()) == 0) return ret;

  char* ValueBuf;
  UINT Len;
  VerQueryValue(buf(), "\\VarFileInfo\\Translation", &(void *) ValueBuf, &Len);
  if(Len < 4) return ret;

  AnsiString CharSet = IntToHex((int)MAKELONG(*(int*) (ValueBuf + 2), *(int*) ValueBuf),8);
  AnsiString fn = "\\StringFileInfo\\" + CharSet + "\\"+pname;

  if(VerQueryValue(buf(),fn.c_str(),&(void *) ValueBuf, &Len) != 0)
  ret = ValueBuf;

  return ret;
}
//---------------------------------------------------------------------------
String __fastcall TMainForm::GetVer(String FileName)  // ��������� ������ �����
{
  AnsiString Version;
  DWORD h;

  wchar_t* WFileName = FileName.c_str();
  char* CFileName;
  sprintf(CFileName, "%S", WFileName);   // wchar_t* -> char*

  DWORD Size = GetFileVersionInfoSize(CFileName, &h);

  if(Size == 0)
  {
	Version = "����������";
	return Version;
  }

  char *buf;
  buf = (char *)GlobalAlloc(GMEM_FIXED, Size);

  if(GetFileVersionInfo(CFileName, h, Size, buf)!=0)
  {
	char *ValueBuf;
	UINT Len;
	VerQueryValue(buf, "\\VarFileInfo\\Translation", &(void *)ValueBuf, &Len);
	if(Len>=4)
	{
	  String CharSet = IntToHex((int )MAKELONG(*(int *)(ValueBuf + 2), *(int *)ValueBuf), 8);
	  AnsiString lpSubBlock = String("\\StringFileInfo\\"+CharSet+"\\FileVersion");

	  if(VerQueryValueA(buf, lpSubBlock.c_str(), &(void *)ValueBuf, &Len)) Version = ValueBuf;
	}
  }

  GlobalFree(buf);
  return Version;
}
//---------------------------------------------------------------------------
void __fastcall TMainForm::Button1Click(TObject *Sender)
{

GetAppVersion("Birthday.exe", VerInfo);
Memo1->Lines->Add(VerInfo[0]);
Memo1->Lines->Add(VerInfo[1]);
Memo1->Lines->Add(VerInfo[2]);
Memo1->Lines->Add(VerInfo[3]);


//Label1->Caption=GetVer("Birthday.exe");
//Label1->Caption = GetAnyAppParam("Birthday.exe","FileVersion");
}
//---------------------------------------------------------------------------

