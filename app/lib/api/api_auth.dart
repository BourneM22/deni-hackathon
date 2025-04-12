import 'dart:convert';

import 'package:http/http.dart' as http;

import '../api_response/login_response.dart';

final apiAuth = APIAuth();

class APIAuth {
  final String baseUrl = 'http://10.0.2.2:5055/api/auth';

  Future<bool> signIn(data) async {
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
  }

  Future<LoginResponse> login(data) async {
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
  }
}
