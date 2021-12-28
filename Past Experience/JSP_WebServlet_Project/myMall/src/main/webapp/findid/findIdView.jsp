<%@ page language="java" contentType="text/html; charset=UTF-8"
    pageEncoding="UTF-8"%>
<jsp:include page="/layout/header.jsp">
	<jsp:param name="title" value="Find id"/>
</jsp:include>
<form action="FindIdLogic.jsp" method="post">
	<table border="1">
		<caption><h3>아이디 찾기</h3></caption>
		<tr>
			<td><!-- <input type="text" name="user_email" placeholder="이메일을 입력하세요."> -->
				<input type="text" name="user_Email1" size="7">
					<select name="user_Email2">
						<option value="naver.com">naver.com</option>
						<option value="gmail.com">gmail.com</option>
						<option value="daum.net">daum.net</option>
					</select>
			</td>
		</tr>
		<tr>
			<td align="cetner"><input type="submit" value="아이디 찾기"></td>
		</tr>
	</table>
</form>
<jsp:include page="/layout/footer.jsp"/>