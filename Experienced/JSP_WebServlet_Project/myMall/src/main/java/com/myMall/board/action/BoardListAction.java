package com.myMall.board.action;

import java.util.ArrayList;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.myMall.dao.BoardDao;
import com.myMall.dto.BoardDto;

public class BoardListAction implements Action {
	@Override
	public ActionForward execute(HttpServletRequest request, HttpServletResponse response) throws Exception {
		int page = Integer.parseInt(request.getParameter("page"));
		BoardDao dao = BoardDao.getInstance(); // 싱글턴 패턴, 하나의 객체로 공유
		ArrayList<BoardDto> list = dao.getList(page);
		int totalpages = dao.getTotalPages();
		ActionForward actionForward = new ActionForward();
		request.setAttribute("list", list);
		request.setAttribute("totalPages", totalpages);
		request.setAttribute("currentPage", page);
		actionForward.setNextPath("BoardListView.do");
		actionForward.setRedirect(false);
		return actionForward;
	}

}
