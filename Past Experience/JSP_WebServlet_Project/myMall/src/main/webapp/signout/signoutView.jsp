<%@ page language="java" contentType="text/html; charset=UTF-8"
    pageEncoding="UTF-8"%>
<jsp:include page="/layout/header.jsp">
	<jsp:param name="title" value="Signout"/>
</jsp:include>
<form action = "signoutLogic.jsp" method = "post">
	<table border = "1">
		<caption><h3>회원 탈퇴</h3></caption>
		<tr>
			<td><input type = "text" name = "user_id" placeholder = "아이디를 입력하세요."></td>
		</tr>
		<tr>
			<td><input type = "password" name = "user_password" placeholder = "비밀번호를 입력하세요."></td>
		</tr>
		<tr>
			<td align = "center"><input type = "submit" value = "탈퇴"></td>
		</tr>
	</table>
</form>
<jsp:include page="/layout/footer.jsp" />