package com.myMall.dao;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.util.ArrayList;

import javax.naming.Context;
import javax.naming.InitialContext;
import javax.naming.NamingException;
import javax.sql.DataSource;

import com.myMall.dto.MemberDto;

public class MemberDao {
//	private String db_url = "jdbc:oracle:thin:@localhost:1521:orcl";
//	private String db_username = "C##myMall";
//	private String db_password = "myMall123";
	private Connection con;
	private PreparedStatement ps;
	private ResultSet rs;
	private String sql;
	static private DataSource ds; // javax.sql.DataSource Ŀ�ؼ� Ǯ
	static { 
		try {
			System.out.println("start DBCP!");
			Context context = new InitialContext(); // javax.naming.Context
			ds = (DataSource) context.lookup("java:comp/env/jdbc/oracle");
		} catch (NamingException e) {
			e.printStackTrace();
		}
	}
	
	
//	public MemberDao() {
//		try {
//			Class.forName("oracle.jdbc.driver.OracleDriver");
//			con = DriverManager.getConnection(db_url,  db_username, db_password);
//		} catch(Exception e) {
//			e.printStackTrace();
//		}
//	}
	
	public boolean insert(MemberDto dto) {
		boolean result = false;
		sql = "INSERT INTO member VALUES(member_seq.NEXTVAL, ?, ?, ?, ?, SYSDATE, ?, ?, ?)";
		try {
			con = ds.getConnection(); // ds에서 커넥션을 하나 빌려옴.
			ps = con.prepareStatement(sql);
			ps.setString(1, dto.getId());
			ps.setString(2, dto.getPassword());
			ps.setString(3, dto.getEmail());
			ps.setString(4, dto.getNickname());
			ps.setString(5, dto.getZipcode());
			ps.setString(6, dto.getAddress());
			ps.setString(7, dto.getDetailaddr());
			result = ps.executeUpdate() == 1;
		} catch(Exception e) {
			e.printStackTrace();
		} finally {
			try {
				if(ps!=null) ps.close();
				if(con!=null) con.close();
			} catch(Exception e) {
				e.printStackTrace();
			}
		}
		return result;
	}
	
	public MemberDto select(String id, String password) {
		MemberDto dto = null;
		sql = "SELECT * FROM member WHERE id = ? AND password = ?"; 
		try {
			con = ds.getConnection(); // ds에서 커넥션을 하나 빌려옴.
			ps = con.prepareStatement(sql);
			ps.setString(1, id);
			ps.setString(2, password);
			rs = ps.executeQuery();
			if(rs.next()) {
				dto = new MemberDto();
				dto.setId( rs.getString("id") );
				dto.setPassword( rs.getString("password") );
				dto.setEmail( rs.getString("email"));
				dto.setNo( rs.getInt("no") );
				dto.setNickname( rs.getString("nickname"));
				dto.setRegdate( rs.getString("regdate") );
			}
		} catch (Exception e) {
			e.printStackTrace();
		} finally {
			try {
				if(rs != null) rs.close();
				if(ps != null) ps.close();
				if(con != null) con.close();
			} catch(Exception e) {
				e.printStackTrace();
			}
		}
		return dto;
	}
	
	public boolean delete(String id, String password) {
		boolean result = false;
		sql = "DELETE FROM member WHERE id = ? AND password = ?";
		try {
			con = ds.getConnection(); // ds에서 커넥션을 하나 빌려옴
			ps = con.prepareStatement(sql);
			ps.setString(1, id);
			ps.setString(2, password);
			result = 1 == ps.executeUpdate();
		} catch(Exception e) {
			e.printStackTrace();
		} finally {
			try {
				if(rs != null) rs.close();
				if(ps != null) ps.close();
				if(con != null) con.close();
			} catch(Exception e) {
				e.printStackTrace();
			}
		}
		return result;
	}
	
	public String findIdbyEmail(String email) {
		String id = null;
		sql = "SELECT id FROM member WHERE email = ?";
		try {
			con = ds.getConnection(); // ds 에서 커넥션을 하나 빌려옴.
			ps = con.prepareStatement(sql);
			ps.setString(1, email);
			rs = ps.executeQuery();
			if(rs.next()) {
				id = rs.getString("id");
			} 
		} catch(Exception e) {
			e.printStackTrace();
		} finally {
			try {
				if(rs != null) rs.close();
				if(ps != null) ps.close();
				if(con != null) con.close();
			} catch(Exception e) {
				e.printStackTrace();
			}
		}
		return id;
	}
	
	
	public boolean isExistId(String id) {
		boolean exist = false;
		String sql = "SELECT * FROM MEMBER WHERE id = ?";
		try {
			con = ds.getConnection(); // ds에서 커넥션을 하나 빌려옴
			ps = con.prepareStatement(sql);
			ps.setString(1, id);
			rs = ps.executeQuery();
			exist = rs.next();
		} catch(Exception e) {
			e.printStackTrace();
		} finally {
			try {
				if(rs != null) rs.close();
				if(ps != null) ps.close();
				if(con != null) con.close();
			} catch(Exception e) {
				e.printStackTrace();
			}
		}
		return exist;
	}
	
	public boolean modify(MemberDto dto) {
		sql = "UPDATE member SET password = ?, email = ?, nickname = ? WHERE no = ?";
		int result = 0;
		try {
			con = ds.getConnection(); // ds 에서 커넥션을 하나 빌려옴
			ps = con.prepareStatement(sql);
			ps.setString(1, dto.getPassword());
			ps.setString(2, dto.getEmail());
			ps.setString(3, dto.getNickname());
			ps.setInt(4, dto.getNo());
			result = ps.executeUpdate();
		} catch(Exception e) {
			e.printStackTrace();
		} finally {
			try {
				if(rs != null) rs.close();
				if(ps != null) ps.close();
				if(con != null) con.close();
			} catch(Exception e) {
				e.printStackTrace();
			}
		}
		return result == 1;
	}
	
	public ArrayList<MemberDto> getMemberList(){
		ArrayList<MemberDto> list = new ArrayList<MemberDto>();
		MemberDto dto = null;
		String tmpPassword;
		int passwordIndex = 0;
		sql = "SELECT * FROM member";
		try {
			con = ds.getConnection(); // ds 에서 connection 을 하나 빌려옴
			ps = con.prepareStatement(sql);
			rs = ps.executeQuery();
			while(rs.next()) {
				dto = new MemberDto();
				
				tmpPassword = rs.getString("password"); // 패스워드 가져옴
				passwordIndex = tmpPassword.length() - 2; // 패스워드 길이를 -2
				tmpPassword = tmpPassword.substring(0,2); // 0~2 까지만 발췌
				while(passwordIndex > 0) {
					tmpPassword += "*";
					--passwordIndex;
				}
				
				dto.setNo(rs.getInt("no"));
				dto.setId(rs.getString("id"));
				dto.setNickname(rs.getString("nickname"));
				dto.setPassword(tmpPassword);
				dto.setEmail(rs.getString("email"));
				dto.setZipcode(rs.getString("zipcode"));
				dto.setAddress(rs.getString("address"));
				dto.setDetailaddr(rs.getString("detailaddr"));
				dto.setRegdate(rs.getString("regdate"));

				list.add(dto);
			}
		} catch(Exception e) {
			e.printStackTrace();
		} finally {
			try {
				if(rs != null) rs.close();
				if(ps != null) ps.close();
				if(con != null) con.close();
			} catch(Exception e) {
				e.printStackTrace();
			}
		}
		return list.isEmpty() ? null : list;
	}
	
	
	
}
