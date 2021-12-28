package com.myMall.member;

import java.io.IOException;

import javax.servlet.RequestDispatcher;
import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

import com.myMall.dao.MemberDao;

@WebServlet("/signout/signoutLogic.jsp")
public class Signout extends HttpServlet {

	protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		request.setCharacterEncoding("UTF-8");
		String id = request.getParameter("user_id");
		String password = request.getParameter("user_password");
		
		MemberDao dao = new MemberDao();
		boolean result = dao.delete(id, password);
		
		request.setAttribute("result", result);
		RequestDispatcher rd = request.getRequestDispatcher("signoutResultView.jsp");
		HttpSession session = request.getSession(); // 세션요청
		session.invalidate();
		rd.forward(request, response);
	}

	protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		doGet(request, response);
	}

}
