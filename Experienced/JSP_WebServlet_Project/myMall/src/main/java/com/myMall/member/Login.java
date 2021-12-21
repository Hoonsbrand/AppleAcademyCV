package com.myMall.member;

import java.io.IOException;

import javax.servlet.RequestDispatcher;
import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.Cookie;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

import com.myMall.dao.MemberDao;
import com.myMall.dto.MemberDto;


@WebServlet("/login/loginLogic.jsp")
public class Login extends HttpServlet {
	
	protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		request.setCharacterEncoding("UTF-8");
		String id = request.getParameter("user_id");
		String password = request.getParameter("user_password");
		
		MemberDao dao = new MemberDao();
		MemberDto dto = dao.select(id, password);
		
		boolean rememberMe = Boolean.parseBoolean(request.getParameter("remember_me")); 
		if(rememberMe) {
			Cookie cookie = new Cookie("rememberId", id);
			cookie.setMaxAge(60 * 60 * 24 * 365); // 1년
			cookie.setPath("/");
			response.addCookie(cookie);
		} else { // 체크가 안되어있다면, 기존에 있을 수 있는 쿠키를 삭제
			Cookie cookie = new Cookie("rememberId", null); // rememberId 이름의 임시쿠키 만들기
			cookie.setMaxAge(0);
			cookie.setPath("/");
			response.addCookie(cookie); // 기존 remeberId 쿠키 덮어쓰기
		}
		if(dto != null){
			//request.setAttribute("nickname", dto.getNickname());
			HttpSession session = request.getSession(); // 세션 정보를 가져옴
			session.setAttribute("currentNickname", dto.getNickname()); // 속성을 set함
			session.setAttribute("currentId", id);
		}
		
		//pageContext.forward("loginResultView.jsp");
		RequestDispatcher rd = request.getRequestDispatcher("loginResultView.jsp"); 
		rd.forward(request, response);
		// 클라이언트로부터 최초에 들어온 요청을 jsp/servlet 내에서 원하는 자원으로 요청을 넘기는 역할을 수행하거나,
		// 특정 자원에 처리를 요청하고 처리 결과를 얻어오는 기능을 수행
	}

	protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		doGet(request, response);
	}

}
