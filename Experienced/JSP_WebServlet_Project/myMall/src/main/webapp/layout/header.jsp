<%@ page language="java" contentType="text/html; charset=UTF-8"
    pageEncoding="UTF-8"%>
<%@ taglib prefix="c" uri="http://java.sun.com/jsp/jstl/core" %>
<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<title>Hoon's shopping mall</title>
<link rel="stylesheet" type="text/css" href="/myMall/layout/layout.css">
</head>
<body>
	<div align="center">
		<div class="header" align="center">
			<c:choose>
				<c:when test="${sessionScope.currentNickname == null }">
					<a href="/myMall/login/loginView.jsp">login</a> |
					<a href="/myMall/join/joinView.jsp">join</a> |
				</c:when>
				<c:otherwise>
					${sessionScope.currentNickname }님 |
					<a href="/myMall/login/logoutLogic.jsp">logout</a> |
					<a href="/myMall/mypage/mypageView.jsp">my page</a> |
				</c:otherwise>				
			</c:choose>
			<a href="/myMall/board/BoardList.do?page=1">board</a> |
			<a href="/myMall/file/FileList">downloads</a> 
			<c:choose>
				<c:when test="${sessionScope.currentNickname != null }">
					| <a href="/myMall/signout/signoutView.jsp">signout</a>
				</c:when>
			</c:choose>			
		</div>
		<div class="main" align="center">

