<%@ page language="java" contentType="text/html; charset=UTF-8"
    pageEncoding="UTF-8"%>
<jsp:include page="/layout/header.jsp">
	<jsp:param name="title" value="Board Write"/>
</jsp:include>

<!-- <form action = "BoardWrite.do" method = "post">
	<table border="1">
		<caption><h2>게시글 쓰기</h2></caption>
		<tr>
			<th>제목</th>
			<td align="center">
				<input type="text" name="board_title" size="72" placeholder="글 제목을 입력하세요." required>
			</td>
		</tr>
		<tr>
			<td colspan="2" align="center">
				<textarea name="board_content" rows="20" cols="80">따뜻한 말 한마디가 모두를 미소짓게 합니다.</textarea>
			</td>
		</tr>
		<tr>
			<td colspan="2" align="center">
				<input type="submit" value="글쓰기">
				<input type="reset" value="초기화">
			</td>
		</tr>
	</table>
</form> -->
<form action="BoardWrite.do" method="post">
	<table class="table table-striped" style="text-align: center; border: 1px solid #dddddd">
		<thead>
			<tr>
				<th style="background-color: #eeeee; text-align: center;">상품 등록</th>
			</tr>
		</thead>
		<tbody>
			<tr>
				<td><input type="text" class="form-contorl" placeholder="상품명" name="board_title" required></td>
			</tr>
			<tr>
				<td><textarea class="form-control" placeholder="글 내용" name="board_content" style="height:350px; width:950px"></textarea></td>
			</tr>
		</tbody>
	</table>
	<input type="submit" class="btn-primary pull-right" value="글쓰기">
</form>

<form action="Upload" method="post" enctype="multipart/form-data">
	<tr>
		<th>사진 등록</th>
			<td><input type="File" name="user_file1"></td>
	</tr>
	<tr>
		<td colspan="1" align="center">
			<input type="submit" value="업로드">
		</td>
	</tr>
	
</form>
			
<jsp:include page="/layout/footer.jsp"/>