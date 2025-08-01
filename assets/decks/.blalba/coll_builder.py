import os
import json
from dotenv import load_dotenv
from utils import first_char_up

load_dotenv()

major_arcana = [
    # MAJOR ARCANA (22 CARDS)
    "the fool", "the magician", "the high priestess", "the empress", "the emperor", "the hierophant", "the lovers",
    "the chariot", "strength", "the hermit", "wheel of fortune", "justice", "the hanged man", "death", "temperance",
    "the devil", "the tower", "the star", "the moon", "the sun", "judgment", "the world",
    
    # MINOR ARCANA (56 CARDS)        
]

elements = ["wands", "cups", "swords", "pentacles"]
seq = ["ace", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "page", "knight", "queen", "king"]

deck = [] 
count = 1
flag_el = 0
flag_seq = 0
for i in range(78):
    if i < 22:
        deck.append(major_arcana[i])
    else:
        deck.append(f"{seq[flag_seq]} of {elements[flag_el]}")
        flag_seq += 1
        if flag_seq == 14:
            flag_seq = 0
            flag_el += 1        

decknames = [i for i in os.listdir("../") if i != ".blalba"]
# decknames = []

if len(decknames) < 1:
    print("E: DECKS ARE MISSING")
    raise None

deck_size = 78
data = None
path = "./deck.json"
is_load = False
source = os.getenv("SOURCE")

for i in decknames:
    deckName = first_char_up(i)
    
    if os.path.exists(path) and os.path.getsize(path) > 0:
        with open(path, "r") as f:
            data = json.load(f)    
            is_load = True
        
    if is_load and deckName in data["listDeck"].keys():
        print("W: Overwrite existing deck")
        is_load = True
    else:
        with open(path, "w") as f:
            if is_load == False:
                data = {
                    "numDeck": '0', 
                    "listDeck": {
                        deckName: {}
                    }
                }
            data["numDeck"] = int(data["numDeck"]) + 1
            new_cards = [{"id":i, "name": deck[i], "image": f"{source}{deckName}/{i}.jpg"} for i in range(deck_size)]      
            data["listDeck"][deckName] = new_cards
        
            json.dump(data, f, ensure_ascii=True, indent=4)