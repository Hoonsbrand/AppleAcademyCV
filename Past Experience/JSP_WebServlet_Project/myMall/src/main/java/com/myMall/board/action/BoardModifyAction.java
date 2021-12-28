package com.myMall.board.action;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.myMall.dao.BoardDao;

public class BoardModifyAction implements Action {
	@Override
	public ActionForward execute(HttpServletRequest request, HttpServletResponse response) throws Exception {
		String newTitle = null;
		String newContent = null;
		BoardDao dao = null;
		int num = 0;
		boolean result = false;
		ActionForward actionForward = null;
		
		request.setCharacterEncoding("UTF-8");
		
		newTitle = request.getParameter("board_title");
		newContent = request.getParameter("board_content");
		num = Integer.parseInt(request.getParameter("board_num"));
		dao = BoardDao.getInstance();
		result = dao.update(num, newTitle, newContent);
		request.setAttribute("state", "modify");
		request.setAttribute("result", result);
		
		actionForward = new ActionForward();
		actionForward.setNextPath("Result.do");
		actionForward.setRedirect(false);
		
		return actionForward;
	}

}
