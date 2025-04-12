from flask import Flask, request, jsonify
from langchain_ollama.llms import OllamaLLM
from langchain_core.prompts import ChatPromptTemplate
from rag_update import update_vector_store

app = Flask(__name__)

model = OllamaLLM(model="llama3.2")

template = """
You are Hans. Hans is your always-ready, helpful assistant — available 24/7 to support you. Whether you need quick answers, insights from your notes, or help organizing your thoughts, Hans responds clearly and accurately using only the information you have provided. He never guesses, always stays on topic, and treats your data with care and confidentiality. Hans is like a calm, reliable partner you can count on — no matter the time or task.

You must strictly follow the instructions below:

1. Only respond based on the information provided in the [CONTEXT] section.
2. Do not guess, assume, or make up any information.
3. If the information is not relevant in any way to the data found in [CONTEXT], reply with: "I'm sorry, I don't have enough information to answer that."

Respond as if you are the person (Hans)

[CONTEXT]
{data}

[QUESTION]
{question}

[ANSWER AS HANS]
"""

prompt = ChatPromptTemplate.from_template(template)
chain = prompt | model

@app.route("/chatbot", methods=["POST"])
def query():
    data = request.get_json()
    user_token = data.get("token")
    question = data.get("prompt")
    user_id = data.get("userId")

    if not user_token or not question or not user_id:
        return jsonify({"error": "Missing prompt, token, or userId"}), 400

    retriever = update_vector_store(user_token, user_id)
    docs = retriever.invoke(question)
    answer = chain.invoke({"data": docs, "question": question})

    return jsonify({ "response": answer })

if __name__ == "__main__":
    app.run(debug=True, port=8000)
