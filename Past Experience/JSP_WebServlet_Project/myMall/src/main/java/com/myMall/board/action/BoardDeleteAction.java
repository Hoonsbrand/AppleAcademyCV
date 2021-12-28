package com.myMall.board.action;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.myMall.dao.BoardDao;

public class BoardDeleteAction implements Action {

	@Override
	public ActionForward execute(HttpServletRequest request, HttpServletResponse response) throws Exception {
		
		int num = Integer.parseInt(request.getParameter("brdNo"));
		BoardDao dao = BoardDao.getInstance();
		boolean result = dao.delete(num);
		
		request.setAttribute("state", "delete");
		request.setAttribute("result", result);
		ActionForward actionForward = new ActionForward();
		actionForward.setNextPath("Result.do");
		actionForward.setRedirect(false);
		return actionForward;
	}

}
