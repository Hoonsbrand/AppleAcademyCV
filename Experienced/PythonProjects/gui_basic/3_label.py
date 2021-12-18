from tkinter import *

root = Tk()
root.title("Nado GUI")
root.geometry("640x480")

label1 = Label(root, text="안녕하세요")
label1.pack()

photo = PhotoImage(file="gui_basic/img.png")
label2 = Label(root, image=photo)
label2.pack()

def change():
    label1.config(text="또 만나요")

    global photo2
    photo2 = PhotoImage(file="gui_basic/img2.png") # 함수가 끝나면 이미지가 사라져버리는데(garbage collector때문에), 그걸 방지 하기위해서 변수를 전역변수로 선언해준다.
    label2.config(image=photo2)

btn = Button(root, text="클릭", command=change)
btn.pack()

root.mainloop()