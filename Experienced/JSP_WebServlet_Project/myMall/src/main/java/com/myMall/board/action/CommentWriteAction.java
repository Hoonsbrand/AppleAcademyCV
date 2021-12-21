package com.myMall.board.action;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

import com.myMall.dao.CommentDao;
import com.myMall.dto.CommentDto;

public class CommentWriteAction implements Action {

	@Override
	public ActionForward execute(HttpServletRequest request, HttpServletResponse response) throws Exception {
		request.setCharacterEncoding("UTF-8");
		int parentNum = Integer.parseInt(request.getParameter("parent_num"));
		String comment = request.getParameter("user_comment");
		HttpSession session = request.getSession();
		CommentDao dao = CommentDao.getInstance();
		CommentDto dto = new CommentDto();
		
		dto.setId((String)session.getAttribute("currentId"));
		dto.setNickname((String)session.getAttribute("currentNickname"));
		dto.setComment(comment);
		dto.setParentNum(parentNum);
		
		boolean result = dao.insert(dto);
		request.setAttribute("result", result);
		request.setAttribute("state", "write");
		request.setAttribute("parentNum", parentNum);
		ActionForward actionForward = new ActionForward();
		actionForward.setNextPath("CommentResult.do");
		actionForward.setRedirect(false);
		return actionForward;
	}

}
