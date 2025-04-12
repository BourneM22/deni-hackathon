
import 'package:deni_hackathon/api-response/soundboard_get_response.dart';

import 'api_service.dart';

final apiMain = ApiMain();

class ApiMain {
  final baseUrl = 'http://10.0.2.2:5055/api';

  Future<SoundboardGetResponse> getSoundboard() async {
    try {
      final jsonResponse = await apiService.requestGet('$baseUrl/soundboards');

      return SoundboardGetResponse.fromJson(jsonResponse);
    } catch (e) {
      throw Exception('Failed to fetch soundboard: $e');
    }
  }

  Future<bool> createSoundboard(Map<String, dynamic> data) async {
    try {
      final jsonResponse = await apiService.requestPost('$baseUrl/soundboards', data);

      return jsonResponse['success'] as bool;
    } catch (e) {
      throw Exception('Failed to create soundboard: $e');
    }
  }
}