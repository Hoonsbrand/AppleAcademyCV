package com.myMall.file;

import java.io.BufferedInputStream;
import java.io.BufferedOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.IOException;
import java.net.URLEncoder;

import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

@WebServlet("/file/Download")
public class Download extends HttpServlet {
	
	protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		// get fileName
		String fileName = request.getParameter("fileName");
		
		// storage
		String realFolder = request.getServletContext().getRealPath("/storage");
		
		File file = new File(realFolder, fileName);
		
		// response
		// Content - Disposition:attachment;fileName= ??
		// Content-Length: 
		fileName="attachment;fileName="+new String(URLEncoder.encode(fileName, "UTF-8")).replaceAll("\\+", " ");
		response.setHeader("Content-Disposition", fileName);
		// Content_disposition 은 HTTP response body 에 오는 컨텐츠의 기질/성향 등을 알려줌.
		// filename을 같이 알려주면 다운로드 받으라는 명령이 되기도 함.
		response.setHeader("Content-Length", String.valueOf(file.length()));
		// 요청과 응답 메시지의 본문 크기를 바이트 단위로 표시해줍니다. 메시지 크기에 따라 자동으로 만들어집니다.
		
		BufferedInputStream bis = new BufferedInputStream(new FileInputStream(file));
		// BufferedInputStream  : byte단위로 파일을 읽어 올때 사용하는 버퍼 스트림 입니다.
		// BufferedInputStream은 파일을 읽어올때 8192byte의 버퍼를 두고 작업을 하기 때문데
		// 속도가 굉장히 빨라 집니다.파일을 주로 다루는 프로그램을 만들때는 필수 입니다.
		BufferedOutputStream bos = new BufferedOutputStream(response.getOutputStream());
		// BufferedOutputStream   : byte단위로 파일을 기록 할때 사용하는 버퍼 스트림 입니다.
		
		byte[] b = new byte[(int)file.length()];
		bis.read(b, 0, b.length);
		bos.write(b);
		
		bis.close();
		bos.close();
	}

	protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		// TODO Auto-generated method stub
		doGet(request, response);
	}

}
