import requests
from langchain_ollama import OllamaEmbeddings
from langchain_chroma import Chroma
from langchain_core.documents import Document
import hashlib
import os

API_URL = "http://localhost:5055/api/notes"
DB_BASE_DIR = "./chrome_langchain_db"
EMBED_MODEL = "mxbai-embed-large"

embeddings = OllamaEmbeddings(model=EMBED_MODEL)

def get_vector_store_for_user(user_id):
    user_db_dir = os.path.join(DB_BASE_DIR, user_id)
    return Chroma(
        collection_name=f"notes_{user_id}",
        persist_directory=user_db_dir,
        embedding_function=embeddings
    )

def update_vector_store(jwt_token: str, user_id: str):
    vector_store = get_vector_store_for_user(user_id)

    headers = { "Authorization": f"Bearer {jwt_token}" }
    response = requests.get(API_URL, headers=headers)
    response.raise_for_status()

    notes = response.json()

    new_docs, new_ids = [], []

    for note in notes:
        doc_id = hashlib.md5(
            f"{note['title']}_{note['content']}_{note['modifiedDateTime']}".encode()
        ).hexdigest()

        existing = vector_store.get(ids=[doc_id])
        if existing['documents']:
            continue

        content = f"{note['title']} {note['content']} Modified: {note['modifiedDateTime']}"
        new_docs.append(Document(page_content=content, metadata={}, id=doc_id))
        new_ids.append(doc_id)

    if new_docs:
        vector_store.add_documents(documents=new_docs, ids=new_ids)
        print(f"✅ Added {len(new_docs)} notes for user: {user_id}")
    else:
        print(f"ℹ️ No new notes for user: {user_id}")

    return vector_store.as_retriever(search_kwargs={"k": 5})
