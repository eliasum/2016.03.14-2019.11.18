//---------------------------------------------------------------------------

#ifndef BirthdayVersionTracking_mainH
#define BirthdayVersionTracking_mainH
//---------------------------------------------------------------------------
#include <System.Classes.hpp>
#include <Vcl.Controls.hpp>
#include <Vcl.StdCtrls.hpp>
#include <Vcl.Forms.hpp>
#include <Vcl.ExtCtrls.hpp>
//---------------------------------------------------------------------------
class TMainForm : public TForm
{
__published:	// IDE-managed Components
	TButton *B_Delete;
	TButton *Button1;
	TLabel *Label1;
	TMemo *Memo1;
	void __fastcall B_DeleteClick(TObject *Sender);
	void __fastcall Button1Click(TObject *Sender);
private:	// User declarations
public:		// User declarations
	__fastcall TMainForm(TComponent* Owner);
	bool __fastcall TMainForm::GetAppVersion(char* FileName, int* VerInfo);
	String  __fastcall TMainForm::GetVer(String FileName);
	String  __fastcall TMainForm::GetVer2(String FileName);
	AnsiString __fastcall TMainForm::GetAnyAppParam(AnsiString appname,AnsiString pname);
};
//---------------------------------------------------------------------------
extern PACKAGE TMainForm *MainForm;
//---------------------------------------------------------------------------
#endif
