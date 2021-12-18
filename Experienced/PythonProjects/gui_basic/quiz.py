from tkinter import *

root = Tk()
root.title("제목없음 - Mac 메모장")
root.geometry("640x480") 

frame = Frame(root)

col_scrollbar = Scrollbar(frame)
col_scrollbar.pack(side="right", fill="y")

txt = Text(frame, width = 100, height = 100, yscrollcommand = col_scrollbar.set)
txt.pack()

def open_mynote():
    open_file = open("mynote.txt", "r", encoding="utf8")
    txt.delete("1.0", END)
    txt.insert(END, open_file.read())
    open_file.close()

def save_mynote():
    save_file = open("mynote.txt", "w", encoding="utf8")
    save_file.write(txt.get("1.0", END))
    save_file.close()

menu = Menu(root)

# 파일
menu_file = Menu(menu, tearoff=0)
menu_file.add_command(label="열기", command=open_mynote)
menu_file.add_command(label="저장", command=save_mynote)
menu_file.add_command(label="끝내기", command=quit)
menu.add_cascade(label="파일", menu=menu_file)

# 편집
menu_edit = Menu(menu, tearoff=0)
menu.add_cascade(label="편집", menu=menu_edit)

# 서식
menu_idx = Menu(menu, tearoff=0)
menu.add_cascade(label="서식", menu=menu_idx)

# 보기
menu_view = Menu(menu, tearoff=0)
menu.add_cascade(label="보기", menu=menu_view)

# 도움말
menu_help = Menu(menu, tearoff=0)
menu.add_cascade(label="도움말", menu=menu_help)

col_scrollbar.config(command=txt.yview)
root.config(menu=menu)
frame.pack()
root.mainloop()