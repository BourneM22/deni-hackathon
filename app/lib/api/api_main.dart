
import 'package:deni_hackathon/api-response/soundboard_get_response.dart';

import 'api_service.dart';

final apiMain = ApiMain();

class ApiMain {
  final baseUrl = 'http://10.0.2.2:5055/api';

  Future<SoundboardGetResponse> getSoundboard() async {
    try {
      final jsonResponse = await apiService.requestGet('$baseUrl/soundboards');
      return SoundboardGetResponse.fromJson({'data': jsonResponse});
    } catch (e) {
      throw Exception('Failed to fetch soundboard: $e');
    }
  }

  Future<dynamic> getSoundboardAudio(String soundboardId) async {
    try {
      final jsonResponse = await apiService.requestFileGet('$baseUrl/soundboards/$soundboardId/audio');
      return jsonResponse;
    } catch (e) {
      throw Exception('Failed to fetch soundboard audio: $e');
    }
  }

  Future<bool> createSoundboard(Map<String, dynamic> data) async {
    try {
      await apiService.requestPost('$baseUrl/soundboards', data);

      return true;
    } catch (e) {
      throw Exception('Failed to create soundboard: $e');
    }
  }

  Future<bool> updateSoundboard(String soundboardId, Map<String, dynamic> data) async {
    try {
      await apiService.requestPut('$baseUrl/soundboards/$soundboardId', data);

      return true;
    } catch (e) {
      throw Exception('Failed to update soundboard: $e');
    }
  }

  Future<bool> deleteSoundboard(String soundboardId) async {
    try {
      await apiService.requestDelete('$baseUrl/soundboards/$soundboardId');

      return true;
    } catch (e) {
      throw Exception('Failed to delete soundboard: $e');
    }
  }
}