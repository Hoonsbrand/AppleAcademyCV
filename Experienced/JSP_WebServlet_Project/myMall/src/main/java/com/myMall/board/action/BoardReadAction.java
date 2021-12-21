package com.myMall.board.action;

import java.util.ArrayList;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

import com.myMall.dao.BoardDao;
import com.myMall.dao.CommentDao;
import com.myMall.dto.BoardDto;
import com.myMall.dto.CommentDto;


public class BoardReadAction implements Action{

	@Override
	public ActionForward execute(HttpServletRequest request, HttpServletResponse response) throws Exception {
		ActionForward actionForward = null;
		BoardDao boardDao = BoardDao.getInstance();
		BoardDto boardDto = null;
		int boardNum = Integer.parseInt( request.getParameter("brdNo")); 
		CommentDao commentDao = CommentDao.getInstance();
		ArrayList<CommentDto> commentList = commentDao.getList(boardNum); 
		String attName = "brd_" + boardNum; 
		HttpSession session = request.getSession();
		
		if(session.getAttribute(attName) == null) {
			boardDao.updateHit(boardNum); 
			session.setAttribute(attName,  "Y");
		}
		boardDto = boardDao.select(boardNum); 
		request.setAttribute("brdDto", boardDto);
		request.setAttribute("commentList", commentList);
		
		
		actionForward = new ActionForward();
		actionForward.setRedirect(false);
		actionForward.setNextPath("BoardReadView.do");
		return actionForward;
	}

}
