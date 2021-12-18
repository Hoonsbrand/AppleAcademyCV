from tkinter import *

root = Tk()
root.title("Nado GUI")
root.geometry("640x480")

txt = Text(root, width = 30, height = 5)
txt.pack()

txt.insert(END, "글자를 입력하세요")

e = Entry(root, width = 30) # 한줄만가능
e.pack() 
e.insert(END, "한 줄만 입력해요") # END 대신 0 도 가능

def btncmd():
    # 내용 출력
    print(txt.get("1.0", END)) # 처음부터 끝까지에 있는 모든 텍스트 값을 가져옴 1.0 = 첫번째 라인, 0 번째 인덱스로부터(column) 
    print(e.get())

    # 내용 삭제
    txt.delete("1.0", END)
    e.delete(0, END)


btn = Button(root, text="클릭", command=btncmd)
btn.pack()

root.mainloop()