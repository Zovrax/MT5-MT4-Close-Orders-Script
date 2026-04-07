// #if !defined(AFX_ACCOUNTEDITINGDLG_H__C31AD041_A78D_40D2_8FF2_4F9896B29BF4__INCLUDED_)
// #define AFX_ACCOUNTEDITINGDLG_H__C31AD041_A78D_40D2_8FF2_4F9896B29BF4__INCLUDED_

// #if _MSC_VER > 1000
// #pragma once
// #endif // _MSC_VER > 1000
// // AccountEditingDlg.h : header file
// #include "Dialog.h"

// /////////////////////////////////////////////////////////////////////////////
// // AccountEditingDlg dialog

// class AccountEditingDlg : public Dialog
// {
// // Construction
// public:
//     AccountEditingDlg( CWnd * parent, bool isAdd, winplus::Mixed * accountFields );

// // Dialog Data
//     //{{AFX_DATA(AccountEditingDlg)
//     enum { IDD = IDD_ACCOUNT_EDITING };
//     //}}AFX_DATA


// // Overrides
//     // ClassWizard generated virtual function overrides
//     //{{AFX_VIRTUAL(AccountEditingDlg)
//     protected:
//     virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
//     //}}AFX_VIRTUAL

// // Implementation
// protected:
//     CWnd * m_pWndParent;
//     bool m_isAdd;
//     winplus::Mixed * m_accountFields;

//     AccountCateArray m_cates; // �洢������Ϣ
//     CUIntArray m_cateIds2;
//     CUIntArray m_typeSafeRanks;
//     int m_cateIndex;
//     int GetSafeRankByCateId( int cateId ) const;
//     // Generated message map functions
//     //{{AFX_MSG(AccountEditingDlg)
//     virtual BOOL OnInitDialog();
//     virtual void OnOK();
//     afx_msg void OnSelChangeComboCates();
//     afx_msg void OnShowWindow( BOOL bShow, UINT nStatus );
//     //}}AFX_MSG
//     DECLARE_MESSAGE_MAP()
// };

// //{{AFX_INSERT_LOCATION}}
// // Microsoft Visual C++ will insert additional declarations immediately before the previous line.

// #endif // !defined(AFX_ACCOUNTEDITINGDLG_H__C31AD041_A78D_40D2_8FF2_4F9896B29BF4__INCLUDED_)
