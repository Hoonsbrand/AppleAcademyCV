<%@ page language="java" contentType="text/html; charset=UTF-8"
    pageEncoding="UTF-8"%>

<jsp:include page="/layout/header.jsp">
	<jsp:param name="title" value="File Upload"/>
</jsp:include>
<form action="Upload" method="post" enctype="multipart/form-data">
<!-- 파일 전송은 반드시 post 방식 사용!  -->
<table border="1">
	<caption><h2>파일 업로드</h2></caption>
	<tr>
		<th>FILE1</th>
		<td width=75%><input type="File" name="user_file1"></td>
	</tr>
	<tr>
		<th>FILE2</th>
		<td><input type="File" name="user_file2"></td>
	</tr>
	<tr>
		<td colspan="2" align="center">
			<input type="button" value="뒤로" onclick="history.back()">
			<input type="submit" value="업로드">
		</td>
	</tr>
</table>
</form>
<jsp:include page="/layout/footer.jsp"/>