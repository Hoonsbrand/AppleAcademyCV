<%@ page language="java" contentType="text/html; charset=UTF-8"
    pageEncoding="UTF-8"%>

<jsp:include page="/layout/header.jsp">
	<jsp:param name="title" value="Downloads"/>
</jsp:include>

	<h2>1번 파일 정보</h2>
	원래의 파일이름: ${originalFileName1} <br>
	새 이름 : ${fileName1} <br>
	파일 크기 : ${fileSize1} byte <br>
	
	<br>
	
	<h2>2번 파일 정보</h2>
	원래의 파일이름 : ${originalFileName2}
	새 이름 : ${fileName2} <br>
	파일 크기 : ${fileSize2} byte <br>
	
<jsp:include page="/layout/footer.jsp"/>