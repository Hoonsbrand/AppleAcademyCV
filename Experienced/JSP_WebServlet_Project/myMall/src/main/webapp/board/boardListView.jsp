<%@ page language="java" contentType="text/html; charset=UTF-8"
    pageEncoding="UTF-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core" %>
<jsp:include page="/layout/header.jsp">
	<jsp:param name="title" value="Board List" />
</jsp:include>

<c:choose>
	<c:when test="${requestScope.list == null }"> <%-- 게시글이 하나도 없을 때 --%>
		<H2>게시글이 없습니다.</H2>
	</c:when>
	<c:otherwise>
		<caption><h2>게시판</h2></caption>
		<table border="1" width="100%">
			<tr align="center">
				<th>글번호</th>
				<th width="47%">글 제목</th>
				<th>작성자</th>
				<th>조회수</th>
				<th>등록시간</th>
			</tr>	
			<c:forEach var="dto" items="${requestScope.list }">
			<tr align="center">
				<th>${dto.num }</th>
				<th width="47%" align="left"><a href="BoardRead.do?brdNo=${dto.num }">${dto.title }</a></th>
				<th>${dto.nickname }</th>
				<th>${dto.hit }</th>
				<th>${dto.regdate }</th>
			</tr>
			</c:forEach>
			<tr>
				<td colspan="5" align="center">
					<c:if test="${currentPage > 3 }">
						<a href="BoardList.do?page=${currentPage - 3 }">[이전]</a>
					</c:if>
					<c:if test="${currentPage > 2 }">
						<a href="BoardList.do?page=${currentPage-2 }">[${currentPage-2 }]</a>
					</c:if>
					<c:if test="${currentPage > 1 }">
						<a href="BoardList.do?page=${currentPage-1 }">[${currentPage-1 }]</a>
					</c:if>
					${currentPage }
					<c:if test="${currentPage < totalPages }">
						<a href="BoardList.do?page=${currentPage + 1 }">[${currentPage+1 }]</a>
					</c:if>
					<c:if test="${currentPage + 1 < totalPages }">
						<a href="BoardList.do?page=${currentPage + 2 }">[${currentPage+2 }]</a>
					</c:if>
					<c:if test="${currentPage + 2 < totalPages }">
						<a href="BoardList.do?page=${currentPage + 3 }">[다음]</a>
					</c:if>
				</td>	
			</tr>
		</table>
	</c:otherwise>
</c:choose>
<c:if test="${sessionScope.currentId == 'admin' }"> <%-- 현재 로그인 한 상태인 경우 --%>
	<div align="center">
		<input type="Button" value="글쓰기" onclick="location.href='BoardWriteForm.do'">
	</div>
</c:if>
<jsp:include page="/layout/footer.jsp"/>