// #pragma once
// #include "Window.h"
// #include "Functional.h"

// // AccountComprehensiveWnd
// class AccountComprehensiveWnd : public SpwWindow
// {
// public:
//     AccountComprehensiveWnd( CWnd * pParentWnd, CRect const & rect, AccountCate const & cate, AccountArray const & accounts );
//     virtual ~AccountComprehensiveWnd();

//     // ���´���һЩ����
//     void RefreshUi();
//     // ����滭�߼�����������
//     void Draw();
//     // ��Ⱦ������
//     void Render();

// protected:
//     virtual BOOL PreCreateWindow( CREATESTRUCT & cs );

// protected:
//     DECLARE_MESSAGE_MAP()

//     afx_msg int OnCreate( LPCREATESTRUCT lpCreateStruct );
//     afx_msg void OnPaint();
//     afx_msg LRESULT OnNcHitTest( CPoint point );
//     afx_msg void OnMouseMove( UINT nFlags, CPoint point );
//     afx_msg void OnTimer( UINT_PTR nIDEvent );
//     afx_msg void OnLButtonDown( UINT nFlags, CPoint point );
//     afx_msg void OnLButtonUp( UINT nFlags, CPoint point );
//     afx_msg void OnRButtonDown( UINT nFlags, CPoint point );
//     afx_msg void OnRButtonUp( UINT nFlags, CPoint point );
// private:
//     winplus::MembersWrapper<struct AccountComprehensive_Data> _self;

// };
