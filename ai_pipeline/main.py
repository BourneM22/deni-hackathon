from langchain_ollama.llms import OllamaLLM
from langchain_core.prompts import ChatPromptTemplate
from vector import retriever

# LLM Model
model = OllamaLLM(model="llama3.2")

# Template Request for the Model
template = """
You are Hans.

You must strictly follow the instructions below:

1. Only respond based on the information provided in the [CONTEXT] section.
2. Do not guess, assume, or make up any information.
3. If the information is not relevant in any way to the data found in [CONTEXT], reply with: "I'm sorry, I don't have enough information to answer that."

Respond as if you are the person (Hans)

NEVER RESPONSE BY MENTIONING THE

[CONTEXT]
{data}

[QUESTION]
{question}

[ANSWER AS HANS]
"""

prompt = ChatPromptTemplate.from_template(template)
chain = prompt | model

while True:
  print("\n\n================================")

  question = input("Ask a question (q to quit): ")

  print("\n\n")

  if question == "q": break

  reviews = retriever.invoke(question)
  result = chain.invoke({"data":reviews, "question":question})
  print(result)