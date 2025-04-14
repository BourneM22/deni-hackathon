import 'package:deni_hackathon/screens/main/notes/add_note_screen.dart';
import 'package:get/get.dart';

class NotesController extends GetxController {
  @override
  void onInit() {
    super.onInit();
  }

  @override
  void dispose() {
    super.dispose();
  }

  void getNote() {}

  void addNewNote() {
    Get.to(() => const AddNoteScreen());
  }
}
