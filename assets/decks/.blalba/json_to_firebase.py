import os
import json
import time
from dotenv import load_dotenv
import firebase_admin
from firebase_admin import credentials, firestore

load_dotenv()


firestore_coll_name = os.getenv("FIRESTORE_COLL_NAME")
json_file = "deck.json"

print("Connecting fs")
cred = credentials.Certificate(os.getenv("CRED_FIREBASE"))
firebase_admin.initialize_app(cred)
db = firestore.client()
print("connected")

if __name__ == "__main__":
    try:
        s = time.time()
        print("Start uploading")

        with open(json_file, "r") as f:
            data: dict = json.load(f)           
            
            for i in data["listDeck"]:            
                db.collection(firestore_coll_name).document(i).set({"content": data["listDeck"][i]})
                
        print(f"Completed in {time.time() - s:.8f}s")
    except Exception as E:
        print(E)