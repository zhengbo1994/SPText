import requests
import re
import os
filename = 'video\\'
if not os.path.exists(filename):
    os.mkdir(filename)
url = 'https://music.163.com/discover/toplist?id=3778678'
headers = {
    'User-Agent': 'Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/81.0.4044.138 Safari/537.36'
}
response = requests.get(url=url, headers=headers)
# print(response.text)
result = re.findall('<li><a href="/song\?id=(\d+)">(.*?)</a></li>', response.text)
for music, title in result:
    music_url = f'http://music.163.com/song/media/outer/url?id={music}.mp3'
    music_content = requests.get(url=music_url, headers=headers).content
    with open(filename + title + '.mp3', mode='wb') as f:
        f.write(music_content)
        print(title)


