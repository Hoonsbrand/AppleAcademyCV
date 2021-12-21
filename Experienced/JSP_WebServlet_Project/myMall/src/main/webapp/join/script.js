function checkPassword(){
	var pw1 = joinForm.user_password.value;
	var pw2 = joinForm.user_repassword.value;
	if(pw1 != pw2){
		alert('두 비밀번호가 일치하지 않습니다.');
		joinForm.user_password.value = "";
		joinForm.user_repassword.value = "";
	}
	else{
		joinForm.submit();
	}
}

function checkId(){
	var id = joinForm.user_id.value;
	if(id == ""){
		alert("아이디를 먼저 입력하세요.");
	} else{
		window.open("joinCheckIdLogic.jsp?id=" + id, "", "width=350 height=100 left=400 top=350");
	}
}

function checkIdFormClose(sId){
	opener.joinForm.user_id.value = sId;
	window.close();
}