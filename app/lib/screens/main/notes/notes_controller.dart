import 'package:deni_hackathon/api-response/notes_get_response.dart';
import 'package:deni_hackathon/api/api_main.dart';
import 'package:deni_hackathon/screens/main/notes/add_note_screen.dart';
import 'package:get/get.dart';

class NotesController extends GetxController {
  /* ------------------------------- Properties ------------------------------- */
  String? query; // User's query
  String? tagFilter; // Tag filter ex: (All, Important, etc)
  List<Notes>? noteList; // Variable to store & display user's notes

  /* --------------------------------- Methods -------------------------------- */
  @override
  void onInit() {
    super.onInit();
  }

  @override
  void dispose() {
    super.dispose();
  }

  // Update Search Query
  void updateSearchQuery(String? searchQuery) async {
    query = searchQuery;
    getNote(tagFilter, query);
    update();
  }

  // Update Tag Filter
  void updateTagFilter(String? tag) async {
    tagFilter = tag;
    getNote(tagFilter, query);
    update();
  }

  // Get notes
  Future<void> getNote(String? tagId, String? searchQuery) async {
    NotesGetResponse temp = await apiMain.getNotes(
      tagId: tagId,
      search: searchQuery,
    );
    noteList = temp.notes;
  }

  /* ------------------------------- Navigation ------------------------------- */
  void navigateToAddNoteScreen() {
    Get.to(() => const AddNoteScreen());
  }
}
