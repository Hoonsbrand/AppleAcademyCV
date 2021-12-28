package com.myMall.board.action;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.myMall.dao.CommentDao;

public class CommentDeleteAction implements Action {

	@Override
	public ActionForward execute(HttpServletRequest request, HttpServletResponse response) throws Exception {
		int commentNum = Integer.parseInt(request.getParameter("commentNum"));
		int boardNum = Integer.parseInt(request.getParameter("boardNum"));
		CommentDao dao = CommentDao.getInstance();
		
		boolean result = dao.delete(commentNum);
		
		request.setAttribute("result", result);
		request.setAttribute("state", "delete");
		request.setAttribute("parentNum", boardNum);
		
		ActionForward actionForward = new ActionForward();
		actionForward.setNextPath("CommentResult.do");
		actionForward.setRedirect(false);
		return actionForward;
	}

}
