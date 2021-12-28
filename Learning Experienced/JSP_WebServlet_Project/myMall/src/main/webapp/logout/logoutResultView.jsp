<%@ page language="java" contentType="text/html; charset=UTF-8"
    pageEncoding="UTF-8"%>
<jsp:include page="/layout/header.jsp">
	<jsp:param name="title" value="Logged Out"/>
</jsp:include>

<h2>로그아웃을 완료하였습니다.</h2>    
<input type="button" value="메인으로" onclick="location.href='/myMall/index.jsp'">

<jsp:include page="/layout/footer.jsp"/>
