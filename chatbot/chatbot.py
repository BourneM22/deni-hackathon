from transformers import GPT2LMHeadModel, GPT2Tokenizer

# Load pre-trained model and tokenizer
model_name = "gpt2"
model = GPT2LMHeadModel.from_pretrained(model_name)
tokenizer = GPT2Tokenizer.from_pretrained(model_name)

def generate_response(prompt):
    input_ids = tokenizer.encode(prompt, return_tensors="pt")

    # Generate response from the fine-tuned model
    output = model.generate(input_ids, max_length=100, num_return_sequences=1, no_repeat_ngram_size=2, temperature=0.7)
    response = tokenizer.decode(output[0], skip_special_tokens=True)
    
    return response

# Test the fine-tuned model
prompt = "what is yellow?"
print(generate_response(prompt))