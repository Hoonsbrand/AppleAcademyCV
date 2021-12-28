<%@ page language="java" contentType="text/html; charset=UTF-8"
    pageEncoding="UTF-8"%>
<%@ taglib prefix="c" uri = "http://java.sun.com/jsp/jstl/core" %>

<jsp:include page="/layout/header.jsp">
	<jsp:param name="title" value="Admin Page"/>
</jsp:include>

<table border="1">
	<caption><h2> 회원 목록 </h2></caption>
	<tr align="center">
		<th width = 5%>No.</th>
		<th width = 15%>ID</th>
		<th width = 15%>NICKNAME</th>
		<th width = "20%">EMAIL</th>
		<th width = "15%">PASSWORD</th>
		<th width = "15%">ZIPCODE</th>
		<th width = "25%">ADDRESS</th>
		<th width = "5%">DETAILADDRESS</th>
		<th width = "20%">REGDATE</th>
	</tr>
	<c:forEach var="dto" items="${memberList }">
		<tr>
			<td>${dto.no }</td>
			<td>${dto.id }</td>
			<td>${dto.nickname }</td>
			<td>${dto.email }</td>
			<td>${dto.password }</td>
			<td>${dto.zipcode }</td>
			<td>${dto.address }</td>
			<td>${dto.detailaddr }</td>
			<td>${dto.regdate }</td>
		</tr>
	</c:forEach>
</table>
<input type="button" value="확인" onclick="history.back()">

<jsp:include page="/layout/footer.jsp"/>