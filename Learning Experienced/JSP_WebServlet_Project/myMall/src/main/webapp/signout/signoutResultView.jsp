<%@ page language="java" contentType="text/html; charset=UTF-8"
    pageEncoding="UTF-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core" %>
<jsp:include page="/layout/header.jsp">
	<jsp:param name="title" value="signout"/>
</jsp:include>

<c:set var="message">
	<c:choose>
		<c:when test="${requestScope.result }">
			회원 탈퇴를 완료하였습니다.
		</c:when>
		<c:otherwise>
			아이디, 비밀번호를 다시 확인해주세요.
		</c:otherwise>
	</c:choose>
</c:set>
<script>
	alert('${message}');
	location.href='/myMall/index.jsp';
</script>

<jsp:include page="/layout/footer.jsp"/>