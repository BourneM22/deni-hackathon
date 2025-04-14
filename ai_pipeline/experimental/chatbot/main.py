from flask import Flask, request, jsonify
from transformers import GPT2LMHeadModel, GPT2Tokenizer

# Load pre-trained model and tokenizer
model_name = "gpt2"
model = GPT2LMHeadModel.from_pretrained(model_name)
tokenizer = GPT2Tokenizer.from_pretrained(model_name)

# Initialize Flask app
app = Flask(__name__)

def generate_response(prompt):
    # Modify the prompt to instruct the model to avoid repeating the question
    prompt_with_instruction = f"Just answer the question directly, do not repeat it: {prompt}"

    input_ids = tokenizer.encode(prompt_with_instruction, return_tensors="pt")

    # Generate response from the model
    output = model.generate(
        input_ids,
        max_length=100,
        num_return_sequences=1,
        no_repeat_ngram_size=2,  # Ensure no repetition of n-grams
        temperature=0.7,         # Control the randomness of the output
        top_p=0.9,              # Control the diversity of the output
        top_k=50,               # Limit the number of highest probability tokens to sample from
        pad_token_id=tokenizer.eos_token_id,  # Ensure that the model does not generate invalid tokens
        eos_token_id=tokenizer.eos_token_id   # Ensure the model stops generating once it hits the EOS token
    )

    response = tokenizer.decode(output[0], skip_special_tokens=True)
    
    return response

@app.route('/chatbot', methods=['POST'])
def chatbot():
    data = request.json
    user_prompt = data.get('prompt')
    
    if not user_prompt:
        return jsonify({"error": "No message provided"}), 400

    # Generate a response from the GPT-2 model
    response = generate_response(user_prompt)
    
    # Return response as a clean JSON object
    return jsonify({"response": response})

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)  # You can change the port if needed
