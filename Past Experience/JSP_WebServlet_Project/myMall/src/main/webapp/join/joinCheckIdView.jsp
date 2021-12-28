<%@ page language="java" contentType="text/html; charset=UTF-8"
    pageEncoding="UTF-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core" %>
<!DOCTYPE html>
<html>
<head>
	<meta http-equiv="Content-Type" content="text.html; charset=UTF-8">
	<title>아이디 중복 확인</title>
	<script type="text/javascript" src="script.js"></script>
</head>
<body>
	<form method="post" action="joinCheckIdLogic.jsp">
	<c:choose>
		<c:when test = "${param.result }">
			&quot;${ param.id }&quot;(은)는 사용 중 입니다. <br><br>
			ID <input type="text" name="id">
			<input type="submit" value="중복체크">
		</c:when>
		<c:otherwise>
			사용 가능한 아이디입니다. <br><br>
			<input type="button" value="사용" onclick="checkIdFormClose('${ param.id }')">
		</c:otherwise>
	</c:choose>
	</form>
</body>
</html>