package com.myMall.member;

import java.io.IOException;
import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

import com.myMall.dao.MemberDao;
import com.myMall.dto.MemberDto;

@WebServlet("/mypage/modifyLogic.jsp")
public class ModifyLogic extends HttpServlet {

	protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		String password;
		String rePassword;
		String nickname;
		String email;
		MemberDto dto = null;
		HttpSession session = null;
		int no;
		
		request.setCharacterEncoding("UTF-8");
		password = request.getParameter("user_password");
		rePassword = request.getParameter("user_repassword");
		
		if(!password.equals(rePassword)) {
			request.setAttribute("result", false);
		}
		else {
			session = request.getSession();
			no = Integer.parseInt(request.getParameter("user_no"));
			nickname = request.getParameter("user_nickname");
			email = request.getParameter("user_email");
			dto = new MemberDto();
			dto.setNo(no);
			dto.setNickname(nickname);
			dto.setEmail(email);
			dto.setPassword(password);
			if(new MemberDao().modify(dto)) {
				request.setAttribute("result", true);
				session.setAttribute("currentNickname", dto.getNickname());
			} else {
				request.setAttribute("result", false);
			}
			request.getRequestDispatcher("modifyResultView.jsp").forward(request, response);
		}
	}

	protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		// TODO Auto-generated method stub
		doGet(request, response);
	}

}
