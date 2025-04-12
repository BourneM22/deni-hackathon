import 'dart:convert';

import 'package:http/http.dart' as http;
import 'package:shared_preferences/shared_preferences.dart';

final apiService = ApiService();

class ApiService {
  Future<dynamic> requestGet(String url) async {
    try {
      final prefs = await SharedPreferences.getInstance();
      final token = prefs.getString('token') ?? '';

      final response = await http.get(
        Uri.parse(url),
        headers: {
          'Content-Type': 'application/json',
          'Accept': 'application/json',
          'Authorization': 'Bearer $token',
        },
      );
      final decodedResponse = json.decode(response.body);

      // Check if the response is a Map or a List
      if (decodedResponse is Map<String, dynamic>) {
        return decodedResponse;
      } else if (decodedResponse is List<dynamic>) {
        return decodedResponse;
      } else {
        throw Exception('Unexpected response format');
      }
    } catch (e) {
      throw Exception('Failed to fetch data: $e');
    }
  }

  Future<dynamic> requestPost(String url, Map<String, dynamic> body) async {
    try {
      final prefs = await SharedPreferences.getInstance();
      final token = prefs.getString('token') ?? '';

      final response = await http.post(
        Uri.parse(url),
        headers: {
          'Content-Type': 'application/json',
          'Accept': 'application/json',
          'Authorization': 'Bearer $token',
        },
        body: json.encode(body),
      );

      final decodedResponse = json.decode(response.body);

      // Check if the response is a Map or a List
      if (decodedResponse is Map<String, dynamic>) {
        return decodedResponse;
      } else if (decodedResponse is List<dynamic>) {
        return decodedResponse;
      } else {
        throw Exception('Unexpected response format');
      }
    } catch (e) {
      throw Exception('Failed to post data: $e');
    }
  }

  Future<dynamic> requestPut(String url, Map<String, dynamic> body) async {
    try {
      final prefs = await SharedPreferences.getInstance();
      final token = prefs.getString('token') ?? '';

      final response = await http.put(
        Uri.parse(url),
        headers: {
          'Content-Type': 'application/json',
          'Accept': 'application/json',
          'Authorization': 'Bearer $token',
        },
        body: json.encode(body),
      );

      final decodedResponse = json.decode(response.body);

      // Check if the response is a Map or a List
      if (decodedResponse is Map<String, dynamic>) {
        return decodedResponse;
      } else if (decodedResponse is List<dynamic>) {
        return decodedResponse;
      } else {
        throw Exception('Unexpected response format');
      }
    } catch (e) {
      throw Exception('Failed to update data: $e');
    }
  }

  Future<dynamic> requestDelete(String url) async {
    try {
      final prefs = await SharedPreferences.getInstance();
      final token = prefs.getString('token') ?? '';

      final response = await http.delete(
        Uri.parse(url),
        headers: {
          'Content-Type': 'application/json',
          'Accept': 'application/json',
          'Authorization': 'Bearer $token',
        },
      );

      final decodedResponse = json.decode(response.body);

      // Check if the response is a Map or a List
      if (decodedResponse is Map<String, dynamic>) {
        return decodedResponse;
      } else if (decodedResponse is List<dynamic>) {
        return decodedResponse;
      } else {
        throw Exception('Unexpected response format');
      }
    } catch (e) {
      throw Exception('Failed to delete data: $e');
    }
  }
}
