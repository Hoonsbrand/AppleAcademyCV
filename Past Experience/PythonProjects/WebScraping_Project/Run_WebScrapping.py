import requests
from bs4 import BeautifulSoup
import re

def create_soup(url):
    headers = {"user-agent":"Mozilla/5.0 (Macintosh; Intel Mac OS X 11_2_3) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.4389.128 Safari/537.36"}
    res = requests.get(url, headers=headers)
    res.raise_for_status()    
    soup = BeautifulSoup(res.text, "lxml")
    return soup

def print_news(index, title, link):
    print("{}. {}".format(index+1, title))
    print("  (링크 : {})".format(link))

def scrape_it_news():
    print("[IT 뉴스]")
    url = "https://news.naver.com/main/list.nhn?mode=LS2D&mid=shm&sid1=105&sid2=230"
    soup = create_soup(url)
    news_list = soup.find("ul", attrs={"class":"type06_headline"}).find_all("li", limit=3) # 3개까지
    for index, news in enumerate(news_list):
        a_idx = 0
        img = news.find("img")
        if img:
            a_idx = 1 # img 태그가 있으면 1번째 img 태그의 정보를 사용 

        a_tag = news.find_all("a")[a_idx]
        title = a_tag.get_text().strip()
        link = a_tag["href"]
        print_news(index, title, link)
    print()

def scrape_english():
    print("[오늘의 영어 회화]")
    url = "https://www.hackers.co.kr/?c=s_eng/eng_contents/I_others_english&keywd=haceng_submain_lnb_eng_I_others_english&logger_kw=haceng_submain_lnb_eng_I_others_english#;"
    soup = create_soup(url)
    sentences = soup.find_all("div", attrs={"id":re.compile("^conv_kor_t")})
    print("\n(영어 지문)")
    for sentence in sentences[len(sentences)//2:]: # 8문장이 있다고 가정할 때, index 기준 4~7 까지 잘라서 가져옴, len(sentences)//2: -> 8나누기 2를 한것 부터 끝까지
        print(sentence.get_text().strip())
    
    print("\n(한글 지문)")
    for sentence in sentences[:len(sentences)//2]:  # 8문장이 있다고 가정할 때, index 기준 0~3 까지 잘라서 가져옴
        print(sentence.get_text().strip())
    print()

if __name__ == "__main__":
    scrape_it_news()
    scrape_english() 
