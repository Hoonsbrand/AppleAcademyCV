<%@ page language="java" contentType="text/html; charset=EUC-KR"
    pageEncoding="EUC-KR"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core" %>
<jsp:include page="/layout/header.jsp">
	<jsp:param name = "title" value = "Find id"/>
</jsp:include>
<c:choose>
	<c:when test = "${param.result }">
	<h3>ȸ�����Կ� ����帳�ϴ�.</h3>
	<input type="button" value="�α���" onclick="location.href='/myMall/login/loginView.jsp'">
	</c:when>
	<c:otherwise>
		<h3>ȸ�����Կ� �����Ͽ����ϴ�.</h3>
		<input type="button" value="�ڷΰ���" onclick="history.back()">
	</c:otherwise>
</c:choose>
<jsp:include page="/layout/footer.jsp"></jsp:include>