<%@ page language="java" contentType="text/html; charset=UTF-8"
    pageEncoding="UTF-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core" %>

<jsp:include page="/layout/header.jsp">
	<jsp:param name="title" value="Downloads"/>
</jsp:include>

<div align="right">
	<input type="button" value="업로드" onclick="location.href = 'fileUpload.jsp'"> 
</div>

<table border="1">
	<caption><h2>자료실</h2></caption>
	<c:forEach var="file" items="${files }">
	<tr>
		<td width="400">
			<a href="Download?fileName=${file.name }">${file.name }</a>
		</td>
	</tr>
	</c:forEach>
</table>

<jsp:include page="/layout/footer.jsp"/>