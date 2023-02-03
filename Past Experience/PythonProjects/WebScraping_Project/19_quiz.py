# 내가 한 퀴즈 ==================================================================

# from selenium import webdriver
# browser = webdriver.Chrome("./chromedriver")
import requests
from bs4 import BeautifulSoup
import re
# browser.maximize_window()

url = "https://search.daum.net/search?nil_suggest=sugsch&w=tot&DA=GIQ&sq=%EC%86%A1%ED%8C%8C+%ED%97%AC%EB%A6%AC&o=1&sugo=15&q=%EC%86%A1%ED%8C%8C+%ED%97%AC%EB%A6%AC%EC%98%A4%EC%8B%9C%ED%8B%B0"
res = requests.get(url)
res.raise_for_status()
soup = BeautifulSoup(res.text, "lxml")

p = re.compile("col") 
# movies = soup.find_all("div", attrs = {"class":["ImZGtf mpg5gc", "Vpfmgd"]})
houses = soup.find_all("td", attrs = {"class":p})
cnt = 0
num = 1
for house in houses:
    title = house.find("div", attrs={"class":"txt_ac"}).get_text()
    if cnt == 0:
        print(f"======================== 매물{num} ========================")
        print(f"거래 : {title}")
    elif cnt == 1:
        print(f"면적 : {title}")
    elif cnt == 2:
        print(f"가격 : {title}")
    elif cnt == 3:
        print(f"동 : {title}")
    elif cnt == 4:
        print(f"층 : {title}")
        num += 1

    cnt += 1

    if cnt == 5:
        cnt = 0
  

# 나도코딩 ==================================================================

# import requests
# from bs4 import BeautifulSoup
# url = "https://search.daum.net/search?nil_suggest=sugsch&w=tot&DA=GIQ&sq=%EC%86%A1%ED%8C%8C+%ED%97%AC%EB%A6%AC&o=1&sugo=15&q=%EC%86%A1%ED%8C%8C+%ED%97%AC%EB%A6%AC%EC%98%A4%EC%8B%9C%ED%8B%B0"
# res = requests.get(url)
# res.raise_for_status()
# soup = BeautifulSoup(res.text, "lxml")

# # with open("quiz.html", "w", encoding="utf8") as f:
# #     f.write(soup.prettify())

# data_rows = soup.find("table", attrs={"class":"tbl"}).find("tbody").find_all("tr")
# for index, row in enumerate(data_rows):
#     columns = row.find_all("td")

#     print(" ================== 매물 {} ==================".format(index + 1))
#     print(" 거래 : ", columns[0].get_text())
#     print(" 면적 : ", columns[1].get_text())
#     print(" 가격 : ", columns[2].get_text())
#     print(" 동 : ", columns[3].get_text())
#     print(" 층 : ", columns[4].get_text())

