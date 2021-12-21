package com.myMall.member;

import java.io.IOException;

import javax.servlet.RequestDispatcher;
import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;


@WebServlet("/login/logoutLogic.jsp")
public class LogoutLogic extends HttpServlet {
	
	protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		// 1 : 세션을 가져옴
		HttpSession session = request.getSession(); // 세션요청
		session.invalidate();
		
		// 2 :forward를 이용해 페이지 이동(기록남음)
//		response.sendRedirect("/login/loginView.jsp");
		RequestDispatcher rd = request.getRequestDispatcher("loginView.jsp"); 
		rd.forward(request, response);
		
	}

	protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		doGet(request, response);
	}

}
