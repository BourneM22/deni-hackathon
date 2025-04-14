import 'package:deni_hackathon/api-response/notes_get_response.dart';
import 'package:deni_hackathon/screens/main/notes/add_note_screen.dart';
import 'package:get/get.dart';

class NotesController extends GetxController {
  /* ------------------------------- Properties ------------------------------- */
  String? query;
  String? tagFilter;
  List<Notes>? noteList;

  /* --------------------------------- Methods -------------------------------- */
  @override
  void onInit() {
    super.onInit();
  }

  @override
  void dispose() {
    super.dispose();
  }

  void getNote() {

  }

  void navigateToAddNoteScreen() {
    Get.to(() => const AddNoteScreen());
  }
}
