import os
import json
import firebase_admin
import time
from dotenv import load_dotenv
from firebase_admin import credentials, firestore

load_dotenv()

cred = credentials.Certificate(os.getenv("CRED_FIREBASE"))
firebase_admin.initialize_app(cred)

firestore_coll_name = os.getenv("FIRESTORE_COLL_NAME")
json_file = "deck.json"

db = firestore.client()

if __name__ == "__main__":
    try:
        s = time.time()
        print("Start uploading")
        with open(json_file, "r") as f:
            data = json.load(f)            
        db.collection(firestore_coll_name).add(data)
        print(f"Completed in {time.time() - s:.8f}s")
    except Exception as E:
        print(E)