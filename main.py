import sqlite3
import requests
from bs4 import BeautifulSoup


headers = {
    'User-Agent' : 'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/106.0.0.0 Safari/537.36'
}
req = requests.get('https://www.rbc.ru/business/', headers)
with open("BD.html", 'w', encoding="utf-8") as file:
    file.write(req.text)
with open('BD.html', encoding="utf-8") as file:
    src = file.read()
soup = BeautifulSoup(src, 'lxml')
articles = soup.find_all('div', class_='item__wrap l-col-center')
prog_url = []
for i in articles:
    project = i.find('a').get('href')
    prog_url.append(project)
a = []
for i in prog_url:
    req = requests.get(i, headers)
    soup = BeautifulSoup(req.text, 'lxml')
    Name_prog = soup.find('h1', class_="article__header__title-in js-slide-title").text
    Text_begin = soup.find('div', class_='article__text__overview').text
    Text = soup.find('p').get_text()
    img = soup.find('div', class_='article__main-image__wrap').find('img').get('src')
    a = (Name_prog, Text_begin, Text, img)
    conn = sqlite3.connect('new.db')
    cur = conn.cursor()

    cur.execute("""CREATE TABLE IF NOT EXISTS users(
                zagalovok TEXT,
                short_text TEXT,
                txt TEXT,
                photo TEXT);
                """)

    cur.execute(f"""INSERT INTO users(zagalovok, short_text, txt, photo)
    VALUES('{Name_prog}', '{Text_begin}', '{Text}', '{img}');""")

    conn.commit()

    cur.close()








