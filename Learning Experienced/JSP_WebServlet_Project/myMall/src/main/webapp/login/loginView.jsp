<%@ page language="java" contentType="text/html; charset=EUC-KR"
    pageEncoding="UTF-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core" %>
<jsp:include page="/layout/header.jsp">
	<jsp:param name="title" value="login"/>
</jsp:include>

<form action="loginLogic.jsp" method="post">
	<table border="1"> <!-- border = 테이블의 테두리 -->
		<caption><h3>로그인</h3></caption>  <!-- 테이블 이름 --> 
		<tr>
			<th>아이디</th>
			<td>
				<c:choose>
					<c:when test="${cookie.rememberId.value == null }">
						<input type="text" name="user_id" placeholder="ID를 입력하세요." required>
					</c:when>
					<c:otherwise>
						<input type="text" name="user_id" value="${cookie.rememberId.value }" required>
					</c:otherwise>
				</c:choose>
			</td>
		</tr>
		<tr>
			<th>비밀번호</th>
			<td><input type="password" name="user_password" placeholder="비밀번호를 입력하세요." required></td>
		</tr>
		<tr>
			<td colspan="2" align="center">
				<input type="checkbox" name="remember_me" value="true">아이디 기억하기
			</td>
		</tr>
		<tr>
			<td colspan="2" align="center">
				<input type="submit" value="LOGIN">
				<a href="/myMall/findid/findIdView.jsp">아이디 찾기</a>
			</td>
		</tr>
	</table>
</form>