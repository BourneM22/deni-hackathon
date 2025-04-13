
import '../api-response/chatbot_response.dart';
import 'api_service.dart';

final apiDeni = ApiDeni();

class ApiDeni {
  final baseUrl = 'http://192.168.18.10:5055/api';

  Future<ChatbotResponse> sendToChatbot(Map<String, dynamic> data) async {
    try {
      final jsonResponse = await apiService.requestPost('$baseUrl/chatbot', data);
      return ChatbotResponse.fromJson(jsonResponse);
    } catch (e) {
      throw Exception('Failed to send message to chatbot: $e');
    }
  }
}