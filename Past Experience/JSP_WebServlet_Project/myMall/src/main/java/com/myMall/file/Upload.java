package com.myMall.file;

import java.io.File;
import java.io.IOException;

import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import com.oreilly.servlet.MultipartRequest;
import com.oreilly.servlet.multipart.DefaultFileRenamePolicy;

@WebServlet("/file/Upload")
public class Upload extends HttpServlet {
   
	protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		String fileName1 = null;
		String fileName2 = null; 
		File file1 = null;
		File file2 = null;
		long fileSize1 = 0;
		long fileSize2 = 0;
		
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
		String originalFileName2 = mr.getOriginalFileName("user_file2");
		
		System.out.println("originalFileName1 : " + originalFileName1);
		System.out.println("originalFileName2 : " + originalFileName2);
		
		if (originalFileName1 != null) {
			// 업로드된 파일의 새 이름(중복이면 숫자 붙임, 아니면 원래 이름 그대로 사용)
			fileName1 = mr.getFilesystemName("user_file1");
			
			// 해당 파일을 File형 객체로 받아옴
			file1 = mr.getFile("user_file1");
			
			// 파일의 크기
			fileSize1 = file1.length();
		}
		
		if(originalFileName2 != null) {
			// 업로드된 파일의 새 이름(중복이면 숫자 붙임, 아니면 원래 이름 그대로 사용)
			fileName2 = mr.getFilesystemName("user_file2");
						
			// 해당 파일을 File형 객체로 받아옴
			file2 = mr.getFile("user_file2");
					
			// 파일의 크기
			fileSize2 = file2.length();
		}
		
		request.setAttribute("originalFileName1", originalFileName1);
		request.setAttribute("fileName1", fileName1);
		request.setAttribute("fileSize1", fileSize1);
		
		request.setAttribute("originalFileName2", originalFileName2);
		request.setAttribute("fileName2", fileName2);
		request.setAttribute("fileSize2", fileSize2);
		
		request.getRequestDispatcher("/file/fileUploadResult.jsp").forward(request, response);
	}

	/**
	 * @see HttpServlet#doPost(HttpServletRequest request, HttpServletResponse response)
	 */
	protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		// TODO Auto-generated method stub
		doGet(request, response);
	}

}
