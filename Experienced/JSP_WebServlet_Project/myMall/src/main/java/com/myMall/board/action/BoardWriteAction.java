package com.myMall.board.action;

import java.io.File;

import javax.servlet.ServletContext;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import javax.servlet.http.HttpSession;

import com.myMall.dao.BoardDao;
import com.myMall.dto.BoardDto;
import com.oreilly.servlet.MultipartRequest;
import com.oreilly.servlet.multipart.DefaultFileRenamePolicy;

public class BoardWriteAction implements Action {

	@Override
	public ActionForward execute(HttpServletRequest request, HttpServletResponse response) throws Exception {

		String fileName1 = null;
		File file1 = null;
		long fileSize1 = 0;
		
		// 실제 폴더 경로 출력
		String realPath = request.getServletContext().getRealPath("/storage");
		System.out.println("storage 폴더의 실제 경로 = " + realPath);
		
		// 업로드
		// DefaultFileRenamePolicy
		// : 자료실 파일과 이름이 중복되는 경우, 업로드 할 파일 이름에 숫자를 덧붙여서 저장하도록 한다.
		MultipartRequest mr = new MultipartRequest(
									request,			// 실제 파라미터가 있는 request 객체
									realPath,			// 파라미터(파일)을 저장 할 실제경로
									5 * 1024 * 1024,	// 제한 용량 (5 * 1024 * 1024 = 5MB)
									"UTF-8",			// 인코딩 형식
									new DefaultFileRenamePolicy() // 중복 이름 정책
								);
		
		// 파일의 원본 이름
		String originalFileName1 = mr.getOriginalFileName("user_file1"); // user_file1 = 파리미터 이름
		
		System.out.println("originalFileName1 : " + originalFileName1);
		
		if (originalFileName1 != null) {
			// 업로드된 파일의 새 이름(중복이면 숫자 붙임, 아니면 원래 이름 그대로 사용)
			fileName1 = mr.getFilesystemName("user_file1");
			
			// 해당 파일을 File형 객체로 받아옴
			file1 = mr.getFile("user_file1");
			
			// 파일의 크기
			fileSize1 = file1.length();
		}
		
		request.setAttribute("originalFileName1", originalFileName1);
		request.setAttribute("fileName1", fileName1);
		request.setAttribute("fileSize1", fileSize1);
		
		request.getRequestDispatcher("/file/fileUploadResult.jsp").forward(request, response);

		
		String writer = null;
		String nickname = null;
		BoardDto dto = null;
		BoardDao dao = null;
		boolean result = false;
	
		ActionForward actionForward = new ActionForward();
		HttpSession session = request.getSession();
		
		request.setCharacterEncoding("UTF-8");
		writer = (String)session.getAttribute("currentId");
		nickname = (String)session.getAttribute("currentNickname");
		
		if(writer != null) {
			dto = new BoardDto();
			dto.setTitle(request.getParameter("board_title"));
			dto.setContent(request.getParameter("board_content"));
			dto.setWriter(writer); 
			dto.setNickname(nickname);
			
			dao = BoardDao.getInstance();
			result = dao.insert(dto);
		}
		request.setAttribute("state", "write");
		request.setAttribute("result", result);
		actionForward.setNextPath("Result.do");
		actionForward.setRedirect(false);
		return actionForward;
	}

}
