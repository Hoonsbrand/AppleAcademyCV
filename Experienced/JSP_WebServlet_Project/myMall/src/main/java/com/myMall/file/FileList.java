package com.myMall.file;

import java.io.File;
import java.io.IOException;

import javax.servlet.ServletException;
import javax.servlet.annotation.WebServlet;
import javax.servlet.http.HttpServlet;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

@WebServlet("/file/FileList")
public class FileList extends HttpServlet {

	protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		String storagePath = request.getServletContext().getRealPath("/storage");
		File storageDirectory = new File(storagePath);
		File[] files = storageDirectory.listFiles();
		request.setAttribute("files", files);
		
		request.getRequestDispatcher("fileListView.jsp").forward(request, response);
		
	}

	protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		// TODO Auto-generated method stub
		doGet(request, response);
	}

}
