<%@ page language="java" contentType="text/html; charset=UTF-8"
    pageEncoding="UTF-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core" %>

<c:set var="dto" value="${requestScope.brdDto}"/>
<c:remove var="brdDto" scope="request"/>

<jsp:include page="/layout/header.jsp">
	<jsp:param name="title" value="${dto.title }"/>
</jsp:include>
<table border="1" width="70%">
	<caption><h2>본문 보기</h2></caption>
	<tr>
		<th width="15%">제목</th>
		<td width="55%">
			${dto.title }
		</td>
		<th width="15%">조회수</th>
		<td>${dto.hit }</td>
	</tr>
	<tr>
		<td colspan="4" height="200" valign="top">
			${dto.content }
		</td>
	</tr>
	<tr>
		<td colspan="4" align="center">
			<!-- <input type = "button" value = "목록으로" onclick = "location.href='BoardList.do?page=1'"> -->
			<input type = "button" value = "목록으로" onclick = "history.back()">
			<c:if test="${sessionScope.currentId == dto.nickname }">
				<input type="button" value="수정" onclick="location.href='BoardModify.do?brdNo=${dto.num}'">
				<input type="button" value="삭제" onclick="location.href='BoardDelete.do?brdNo=${dto.num}'">
			</c:if>
		</td>
	</tr>
</table>

<form action = "CommentWrite.do" method = "post">
<input type = "hidden" name = "parent_num" value = "${dto.num }">
<table border ="1" width = "70%">
	<c:forEach var="commentDto" items="${requestScope.commentList }">
		<tr>
			<th width="25%">${commentDto.nickname }</th>
			<td width="55%">
				${commentDto.comment }
				&nbsp;&nbsp;&nbsp;
				<c:if test="${sessionScope.currentId == commentDto.id }">
					<a href="CommentDelete.do?boardNum=${commentDto.parentNum }&commentNum=${commentDto.num}">삭제</a>
				</c:if>
			<td>${commentDto.regdate }</td>
		</tr>
	</c:forEach> 
	<c:if test = "${currentId != null }">
	<tr>
		<th width = "25%">${ sessionScope.currentNickname }</th>
		<td width = "55%">
			<textarea name = "user_comment" rows = "4" placeholder = "댓글을 남겨주세요!" required></textarea>
		</td>
		<td><input type = "submit" value = "댓글 달기"></td>
	</tr>
	</c:if>
</table>
</form>
<jsp:include page="/layout/footer.jsp"/>