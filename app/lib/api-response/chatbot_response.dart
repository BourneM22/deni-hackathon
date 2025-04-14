class ChatbotResponse {
  String? response;

  ChatbotResponse({
    this.response,
  });

  ChatbotResponse.fromJson(Map<String, dynamic> json) {
    response = json['response'] ?? '';
  }

  Map<String, dynamic> toJson() {
    final Map<String, dynamic> data = <String, dynamic>{};
    data['response'] = response;
    return data;
  }
}