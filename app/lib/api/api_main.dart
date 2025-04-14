
import 'package:deni_hackathon/api-response/notes_get_response.dart';
import 'package:deni_hackathon/api-response/soundboard_get_response.dart';

import 'api_service.dart';

final apiMain = ApiMain();

class ApiMain {
  final baseUrl = 'http://10.0.2.2:5055/api';

  /* ----------------------------- SOUNDBOARD API ----------------------------- */
  Future<SoundboardGetResponse> getSoundboard() async {
    try {
      final jsonResponse = await
        apiService
          .requestGet('$baseUrl/soundboards');

      return SoundboardGetResponse.fromJson({'data': jsonResponse});
    }
    catch(e) {
      throw Exception('Failed to fetch soundboard: $e');
    }
  }

  Future<dynamic> getSoundboardAudio(String soundboardId) async {
    try {
      final jsonResponse = await
        apiService
          .requestFileGet('$baseUrl/soundboards/$soundboardId/audio');

      return jsonResponse;
    }
    catch(e) {
      throw Exception('Failed to fetch soundboard audio: $e');
    }
  }

  Future<bool> createSoundboard(Map<String, dynamic> data) async {
    try {
      await
        apiService
          .requestPost('$baseUrl/soundboards', data);

      return true;
    }
    catch(e) {
      throw Exception('Failed to create soundboard: $e');
    }
  }

  Future<bool> updateSoundboard(String soundboardId, Map<String, dynamic> data) async {
    try {
      await
        apiService
          .requestPut('$baseUrl/soundboards/$soundboardId', data);

      return true;
    }
    catch(e) {
      throw Exception('Failed to update soundboard: $e');
    }
  }

  Future<bool> deleteSoundboard(String soundboardId) async {
    try {
      await
        apiService
          .requestDelete('$baseUrl/soundboards/$soundboardId');

      return true;
    }
    catch(e) {
      throw Exception('Failed to delete soundboard: $e');
    }
  }

  /* -------------------------------- NOTES API ------------------------------- */

  // GET notes by tagId & title/content similarity to query (search)
  Future<NotesGetResponse> getNotes({
    String? tagId,
    String? search
  })
  async {
    try {
      // Pre-process query params
      final queryParams = <String, String>{};

      if (tagId != null && tagId.isNotEmpty) {
        queryParams['tagId'] = tagId;
      }
      if (search != null && search.isNotEmpty){
        queryParams['search'] = search;
      }

      // Format uri & request data
      final uri = Uri
        .parse('$baseUrl/notes')
        .replace(queryParameters: queryParams);

      final jsonResponse = await apiService
        .requestGet(uri.toString());

      // Return response
      return NotesGetResponse.fromJson(
        {'data': jsonResponse}
      );
    }
    catch(e) {
      throw Exception('Failed to fetch notes: $e');
    }
  }

  // GET note by id
  Future<NotesGetResponse> getNoteById({
    String? noteId
  })
  async {
    try {
      final jsonResponse = await apiService
        .requestGet("$baseUrl/notes/$noteId");

      // Return response
      return NotesGetResponse.fromJson(
        {'data': jsonResponse}
      );
    }
    catch(e) {
      throw Exception('Failed to fetch notes: $e');
    }
  }

  // POST note
  Future<bool> createNote(Map<String, dynamic> data) async {
    try {
      await
        apiService
          .requestPost('$baseUrl/notes', data);

      return true;
    }
    catch(e) {
      throw Exception('Failed to create note: $e');
    }
  }

  // PUT note by id
  Future<bool> updateNote(String noteId, Map<String, dynamic> data) async {
    try {
      await
        apiService
          .requestPut('$baseUrl/notes/$noteId', data);

      return true;
    }
    catch(e) {
      throw Exception('Failed to update note: $e');
    }
  }

  // DELETE note by id
  Future<bool> deleteNote(String noteId) async {
    try {
      await
        apiService
          .requestDelete('$baseUrl/notes/$noteId');

      return true;
    }
    catch(e) {
      throw Exception('Failed to delete note: $e');
    }
  }
}