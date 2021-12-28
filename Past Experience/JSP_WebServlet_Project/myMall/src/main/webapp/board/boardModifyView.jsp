<%@ page language="java" contentType="text/html; charset=UTF-8"
    pageEncoding="UTF-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core" %>
<c:set var="dto" value="${requestScope.brdDto }"/>
<c:remove var="brdDto" scope="request"/>

<jsp:include page="/layout/header.jsp">
	<jsp:param name="title" value="Board Modify"/>
</jsp:include>

<form action="BoardModifyAction.do" method="post">
	<input type="hidden" name="board_num" value="${dto.num }">
	<table border="1">
	<caption><h2>게시글 수정</h2></caption>
	<tr>
		<th>제목</th>
		<td align="center">
			<input type="text" name="board_title" size="72" value="${dto.title }" required>
		</td>
	</tr>
	<tr>
		<td colspan="2" align="center">
			<textarea name="board_content" rows="20" cols="80">${dto.content }</textarea>
		</td>
	</tr>
	<tr>
		<td colspan="2" align="center">
			<input type="submit" value="글수정">
			<input type="reset" value="초기화">
		</td>
	</tr>
	</table>
</form>
<jsp:include page="/layout/footer.jsp"/>
