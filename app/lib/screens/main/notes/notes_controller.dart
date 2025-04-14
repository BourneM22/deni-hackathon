import 'package:deni_hackathon/api-response/notes_get_response.dart';
import 'package:deni_hackathon/api/api_main.dart';
import 'package:deni_hackathon/screens/main/notes/add_note_screen.dart';
import 'package:flutter/widgets.dart';
import 'package:fluttertoast/fluttertoast.dart';
import 'package:get/get.dart';

import '../../../constants/colors_constants.dart';
import '../../../utils/logger.dart';

class NotesController extends GetxController {
  /* ------------------------------- Properties ------------------------------- */
  String? query;                        // User's query
  String? tagFilter;                    // Tag filter ex: (All, Important, etc)
  List<Notes> noteList = List.empty();  // Variable to store & display user's notes

  final TextEditingController titleTFController = TextEditingController();
  final TextEditingController contentTFController = TextEditingController(); 

  /* --------------------------------- Methods -------------------------------- */
  @override
  void onInit() {
    super.onInit();

    // Initial pull
    getNote(tagFilter, query);
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

    noteList = temp.notes ?? [];
    update();
  }

  void navigateToAddNoteScreen() async {
    final result = await Get.to(() => const AddNoteScreen());
  if (result == true) {
    // Reload the notes if a new note was added
    getNote(tagFilter, query);
  }
  }

  Future<void> onSaveNotes() async {
    try {
      final data = {
        'title': titleTFController.text,
        'content': contentTFController.text,
      };
      await apiMain.createNote(data);

      Get.back(result: true); // Close the AddNoteScreen and return true
      Fluttertoast.showToast(
        msg: "Note created successfully!",
        toastLength: Toast.LENGTH_LONG,
        gravity: ToastGravity.CENTER,
        backgroundColor: ColorsConstants.blackToastColor,
        textColor: ColorsConstants.trueWhiteColor,
        fontSize: 12.0,
      );
    } catch (e) {
      log.e(e);
      Fluttertoast.showToast(
        msg: "Something went wrong!",
        toastLength: Toast.LENGTH_LONG,
        gravity: ToastGravity.CENTER,
        backgroundColor: ColorsConstants.blackToastColor,
        textColor: ColorsConstants.trueWhiteColor,
        fontSize: 12.0,
      );
    }
  }
}
