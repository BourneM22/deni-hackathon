import 'dart:convert';

import 'package:http/http.dart' as http;

import '../api-response/login_response.dart';
import '../api-response/profile_response.dart';
import '../utils/logger.dart';
import 'api_service.dart';

final apiAuth = APIAuth();

class APIAuth {
  // Ini localhost untuk emulator Android
  // Port 5055 adalah port yang digunakan oleh server
  // Kalo pake device lain kemungkinan harus ganti IP address ke IP address device yang dipake
  final String baseUrl = 'http://10.0.2.2:5055/api/auth';
  final String profileBaseUrl = 'http://10.0.2.2:5055/api';

  Future<bool> signIn(data) async {
    try {
      final url = '/signin';
      final response = await http.post(
        Uri.parse('$baseUrl$url'),
        headers: {'Content-Type': 'application/json'},
        body: json.encode(data),
      );

      if (response.statusCode == 200) {
        return true;
      } else {
        return false;
      }
    } catch (e) {
      log.e('Error signing in: $e');
      return false;
    }
  }

  Future<LoginResponse> login(data) async {
    try {
      final url = '/login';
      final response = await http.post(
        Uri.parse('$baseUrl$url'),
        headers: {'Content-Type': 'application/json'},
        body: json.encode(data),
      );

      if (response.statusCode == 200) {
        return LoginResponse.fromJson(json.decode(response.body));
      } else {
        throw Exception('Failed to login');
      }
    } catch (e) {
      log.e('Error logging in: $e');
      throw Exception('Failed to login: $e');
    }
  }

  Future<ProfileResponse> getProfileData() async {
    try {
      final jsonResponse = await apiService.requestGet('$profileBaseUrl/user');

      return ProfileResponse.fromJson(jsonResponse);
    } catch (e) {
      throw Exception('Failed to update soundboard: $e');
    }
  }

  Future<bool> updateProfile(data) async {
    try {
      await apiService.requestPut('$profileBaseUrl/user', data);

      return true;
    } catch (e) {
      throw Exception('Failed to update soundboard: $e');
    }
  }
}
