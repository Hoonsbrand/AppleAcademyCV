<%@ page language="java" contentType="text/html; charset=UTF-8"
    pageEncoding="UTF-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core" %>
<jsp:include page="/layout/header.jsp">
	<jsp:param name="title" value="Find id" />
</jsp:include>
<c:set var="message">
	<c:choose>
		<c:when test="${requestScope.id != null }">
			고객님의 ID는 다음과 같습니다.<br>${requestScope.id }
		</c:when>
		<c:otherwise>
			찾으시는 ID는 존재하지 않습니다.
		</c:otherwise>
	</c:choose>
</c:set>
<h4>${message }</h4>
<br>
<input type="button" value="메인으로" onclick="location.href='/myMall/index.jsp'">
<input type="button" value="로그인 페에지 이동" onclick="location.href='/myMall/login/loginView.jsp'">

<jsp:include page="/layout/footer.jsp" />