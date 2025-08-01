"""
Tarot Deck Scraper
Author: hng011
Python Version: 3.12.8
Source: https://www.tarot.com/tarot/decks
"""

import os
import time
import requests
from dotenv import load_dotenv

load_dotenv()

def store(deck_name, count, extension, image_bytes):
    os.makedirs("../" + deck_name, exist_ok=True)
    path = os.path.join("../", deck_name, f"{count}.{extension}")
    
    with open(path, "wb") as f:
        f.write(image_bytes)

def main():
    endpoint = os.getenv("DECK2")
    extension = os.getenv("EXTENSION")
    try:
        for i in range(78):
            res = requests.get(endpoint + str(i) + '.' + extension)
            deck_name = "".join([x.upper() if i == 0 else x for i, x in enumerate(str(endpoint.split("/")[-3]))])
            print(f"{i} success")
            store(deck_name, i, extension, res.content)                
    except Exception as E:
        print(E)
    
if __name__ == "__main__":
    s = time.time()
    main()
    print(f"Running Time: {time.time() - s:.8f}s")

