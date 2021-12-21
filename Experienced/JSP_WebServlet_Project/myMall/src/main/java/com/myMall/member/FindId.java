package com.myMall.member;

import java.io.IOException;

import javax.servlet.RequestDispatcher;
import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.myMall.dao.MemberDao;

@WebServlet("/findid/FindIdLogic.jsp")
public class FindId extends HttpServlet {

	protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		request.setCharacterEncoding("UTF-8");
		//String email = request.getParameter("user_email");
		String email = request.getParameter("user_Email1") + "@" + request.getParameter("user_Email2");
		MemberDao dao = new MemberDao();
		String id = dao.findIdbyEmail(email);
		
		if(id != null) {
			int index = id.length() - 2; // 아이디 전체 길이에서 2개를 자름
			String tmpId = id.substring(0, 2); // 0 ~ 2 까지 문자열 발췌
			
			for(int i=0; i<index; ++i) {
				tmpId += "*"; // 2번 인덱스 후 나머지 문자열은 *로 대체
			}
			id = tmpId; // 덮어씌우기
			request.setAttribute("id", id);
		}
		RequestDispatcher rd = request.getRequestDispatcher("findIdResultView.jsp");
		rd.forward(request, response);
	}

	protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		doGet(request, response);
	}

}
