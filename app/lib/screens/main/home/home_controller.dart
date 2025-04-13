import 'dart:io';

import 'package:audioplayers/audioplayers.dart';
import 'package:deni_hackathon/routes/route_name.dart';
import 'package:flutter/material.dart';
import 'package:flutter_sound/flutter_sound.dart';
import 'package:fluttertoast/fluttertoast.dart';
import 'package:get/get.dart';
import 'package:http/http.dart' as http;
import 'package:path_provider/path_provider.dart';
import 'package:permission_handler/permission_handler.dart';

import '../../../api-response/soundboard_get_response.dart';
import '../../../api/api_main.dart';
import '../../../constants/colors_constants.dart';
import '../../../utils/logger.dart';
import '../../../widgets/deni_style.dart';
import '../main_controller.dart';

class HomeController extends GetxController {
  List<Soundboard> soundboardList = [];
  List<Chat> chatList = [];
  bool isLoading = true;
  bool isError = false;

  final ScrollController scrollController = ScrollController();

  bool isRecording = false;
  final FlutterSoundRecorder recorder = FlutterSoundRecorder();
  String? recordedFilePath;

  bool isExpanded = false;
  bool isEditMode = false;
  Soundboard? selectedSoundboard;
  final AudioPlayer audioPlayer = AudioPlayer();

  @override
  void onInit() {
    isLoading = true;
    isError = false;
    super.onInit();
    getSoundboard();
    initChatList();
    initializeRecorder();
  }

  @override
  void dispose() {
    recorder.closeRecorder();
    audioPlayer.dispose();
    super.dispose();
  }

  Future<void> initializeRecorder() async {
    await recorder.openRecorder();
    await requestPermissions();
  }

  Future<void> requestPermissions() async {
  // Request microphone permission
  final microphoneStatus = await Permission.microphone.request();
  if (microphoneStatus.isDenied) {
    // Show a dialog or prompt to ask for permission
    await _showPermissionDialog(
      "Microphone Permission",
      "This app requires microphone access to record audio. Please grant the permission.",
    );
    await Permission.microphone.request(); // Re-request permission
  }

  // Request storage permission
  final storageStatus = await Permission.storage.request();
  await Permission.audio.request(); // Re-request permission for audio
  await Permission.manageExternalStorage.request(); // Re-request permission for audio
  // if (storageStatus.isDenied) {
  //   // Show a dialog or prompt to ask for permission
  //   await _showPermissionDialog(
  //     "Storage Permission",
  //     "This app requires storage access to save audio files. Please grant the permission.",
  //   );
  //   log.d('a');
  //   await Permission.storage.request();
  //   log.d('z');
  // }

  // Handle permanently denied permissions
  if (microphoneStatus.isPermanentlyDenied || storageStatus.isPermanentlyDenied) {
    await _showPermissionDialog(
      "Permissions Required",
      "Microphone and storage permissions are permanently denied. Please enable them in the app settings.",
      openSettings: true,
    );
  }
}

// Helper method to show a permission dialog
Future<void> _showPermissionDialog(String title, String message, {bool openSettings = false}) async {
  await Get.dialog(
    AlertDialog(
      title: Text(title),
      content: Text(message),
      actions: [
        TextButton(
          onPressed: () {
            if (openSettings) {
              openAppSettings(); // Open app settings if required
            }
            Get.back(); // Close the dialog
          },
          child: Text("OK"),
        ),
      ],
    ),
  );
}

  Future<void> getSoundboard() async {
    try {
      final response = await apiMain.getSoundboard();
      soundboardList = response.soundboards!;
      isLoading = false;
      update();
    } catch (e) {
      log.e(e);
      isError = true;
      update();
    } finally {
      isLoading = false;
      update();
    }
  }

  void onClickProfile() {
    Get.toNamed(ProfileRoute.profile);
  }

  Future<void> onStartRecording() async {
    try {
      isRecording = true;
      update();

      final directory = await getTemporaryDirectory();
      recordedFilePath = '${directory.path}/recording.wav';

      await recorder.startRecorder(
        toFile: recordedFilePath,
        codec: Codec.pcm16WAV,
      );
    } catch (e) {
      log.e("Error starting recording: $e");
    }
  }

  Future<void> onStopRecording() async {
    try {
      await recorder.stopRecorder();
      isRecording = false;
      update();

      if (recordedFilePath != null) {
        final file = File(recordedFilePath!);

      if (await file.exists()) {
        await sendRecordingToBackend(recordedFilePath!);
      } else {
        log.e("File not found at: $recordedFilePath");
      }
      }
    } catch (e) {
      log.e("Error stopping recording: $e");
    }
  }

  Future<void> sendRecordingToBackend(String filePath) async {
    try {
      final file = File(filePath);
      final request = http.MultipartRequest(
        'POST',
        Uri.parse('http://192.168.18.10:5055/api/speech/transcribe'),
      );
      request.files.add(await http.MultipartFile.fromPath('file', file.path));
      final response = await request.send();

      if (response.statusCode == 200) {
        log.i("File uploaded successfully.");
      } else {
        log.e("Failed to upload file. Status code: ${response.statusCode}");
      }
    } catch (e) {
      log.e("Error uploading file: $e");
    }
  }

  void onClearChat() {
    initChatList();
    update();
  }

  void toggleExpandButtons() {
    isExpanded = !isExpanded;
    update();
  }

  void onClickAskDeni() {
    Get.toNamed(DeniRoute.deni);
  }

  Future<void> createSoundboard() async {
    final TextEditingController nameController = TextEditingController();
    final TextEditingController descriptionController = TextEditingController();

    showGeneralDialog(
      context: Get.context!,
      barrierDismissible: true,
      barrierLabel: "Dismiss",
      barrierColor: Colors.black.withOpacity(0.4), // dim background
      transitionDuration: const Duration(milliseconds: 300),
      pageBuilder: (context, animation, secondaryAnimation) {
        return Align(
          alignment: Alignment.bottomCenter,
          child: Padding(
            padding: const EdgeInsets.only(left: 16.0, right: 16.0, bottom: 32.0),
            child: Material(
              borderRadius: BorderRadius.circular(12),
              color: ColorsConstants.baseColor,
              child: Padding(
                padding: const EdgeInsets.symmetric(vertical: 12.0),
                child: Column(
                  mainAxisSize: MainAxisSize.min,
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Padding(
                      padding: const EdgeInsets.symmetric(horizontal: 16.0),
                      child: TextFormField(
                        controller: nameController,
                        style: deniStyle(
                          fontSize: 20,
                          fontFamily: 'Afacad',
                          fontWeight: FontWeight.bold,
                          color: ColorsConstants.darkGreyColor,
                        ),
                        decoration: InputDecoration(
                          border: InputBorder.none,
                          hintText: 'Enter label...',
                          hintStyle: deniStyle(
                            fontSize: 20,
                            fontFamily: 'Afacad',
                            color: ColorsConstants.mediumGreyColor,
                          ),
                        ),
                      ),
                    ),
                    const Divider(color: Colors.grey, thickness: 1),
                    Padding(
                      padding: const EdgeInsets.symmetric(horizontal: 16.0),
                      child: TextFormField(
                        controller: descriptionController,
                        maxLines: 3,
                        style: deniStyle(
                          fontSize: 16,
                          fontFamily: 'Afacad',
                          color: ColorsConstants.darkGreyColor,
                        ),
                        decoration: InputDecoration(
                          border: InputBorder.none,
                          fillColor: ColorsConstants.whiteColor,
                          filled: true,
                          hintText: 'Enter content...',
                          hintStyle: deniStyle(
                            fontSize: 16,
                            fontFamily: 'Afacad',
                            color: ColorsConstants.mediumGreyColor,
                          ),
                        ),
                      ),
                    ),
                    const SizedBox(height: 16),
                    Row(
                      children: [
                        Expanded(
                          child: Padding(
                            padding: const EdgeInsets.only(left: 16.0),
                            child: ElevatedButton(
                              style: ElevatedButton.styleFrom(
                                backgroundColor: ColorsConstants.lightRedColor,
                                shape: const RoundedRectangleBorder(
                                  borderRadius: BorderRadius.all(
                                    Radius.circular(8),
                                  ),
                                ),
                              ),
                              onPressed: () => Navigator.pop(context),
                              child: Text(
                                'Cancel',
                                style: deniStyle(
                                  fontSize: 16,
                                  fontFamily: 'Afacad',
                                  color: ColorsConstants.redColor,
                                ),
                              ),
                            ),
                          ),
                        ),
                        const SizedBox(width: 8),
                        Expanded(
                          child: Padding(
                            padding: const EdgeInsets.only(right: 16.0),
                            child: ElevatedButton(
                              style: ElevatedButton.styleFrom(
                                backgroundColor:
                                    ColorsConstants.lightGreenColor,
                                shape: const RoundedRectangleBorder(
                                  borderRadius: BorderRadius.all(
                                    Radius.circular(8),
                                  ),
                                ),
                              ),
                              onPressed: () async {
                                await onCreateSoundboard(
                                  nameController.text,
                                  descriptionController.text,
                                );
                              },
                              child: Text(
                                'Save',
                                style: deniStyle(
                                  fontSize: 16,
                                  fontFamily: 'Afacad',
                                  color: ColorsConstants.greenColor,
                                ),
                              ),
                            ),
                          ),
                        ),
                      ],
                    ),
                  ],
                ),
              ),
            ),
          ),
        );
      },
      transitionBuilder: (context, animation, secondaryAnimation, child) {
        return SlideTransition(
          position: Tween<Offset>(
            begin: const Offset(0, 1),
            end: Offset.zero,
          ).animate(
            CurvedAnimation(parent: animation, curve: Curves.easeOutBack),
          ),
          child: FadeTransition(
            opacity: CurvedAnimation(parent: animation, curve: Curves.easeIn),
            child: child,
          ),
        );
      },
    );
  }

  Future<void> onCreateSoundboard(String name, String description) async {
    try {
      final data = {'name': name, 'description': description};
      await apiMain.createSoundboard(data);
      Fluttertoast.showToast(
        msg: "Soundboard updated successfully!",
        toastLength: Toast.LENGTH_LONG,
        gravity: ToastGravity.CENTER,
        backgroundColor: ColorsConstants.blackToastColor,
        textColor: ColorsConstants.trueWhiteColor,
        fontSize: 12.0,
      );
      Navigator.pop(Get.context!);
      getSoundboard();
      Get.find<MainController>().exitEditMode();
    } catch (e) {
      log.e(e);
      Fluttertoast.showToast(
        msg: "Failed to update soundboard!",
        toastLength: Toast.LENGTH_LONG,
        gravity: ToastGravity.CENTER,
        backgroundColor: ColorsConstants.blackToastColor,
        textColor: ColorsConstants.trueWhiteColor,
        fontSize: 12.0,
      );
    }
  }

  void initChatList() {
    chatList = [];
    chatList.add(
      Chat(message: "Hello I'm Bourne!\nWhat's your name?", isUser: false),
    );
    update();
  }

  Future<void> onClickSoundboard(Soundboard soundboard) async {
    try {
      // Define the file path
      final directory = await getTemporaryDirectory();
      final filePath = '${directory.path}/${soundboard.soundId}.mp3';
      final file = File(filePath);

      // Check if the file already exists
      if (await file.exists()) {
        await audioPlayer.play(DeviceFileSource(filePath));
      } else {
        // Di download ke local, if not exists
        final response = await apiMain.getSoundboardAudio(soundboard.soundId!);

        await file.writeAsBytes(response);

        await audioPlayer.play(DeviceFileSource(filePath));
      }

      chatList.add(Chat(message: soundboard.description, isUser: true));
      update();

      // Animate scroll
      Future.delayed(const Duration(milliseconds: 100), () {
        if (scrollController.hasClients) {
          scrollController.animateTo(
            scrollController.position.maxScrollExtent,
            duration: const Duration(milliseconds: 300),
            curve: Curves.easeOut,
          );
        }
      });
    } catch (e) {
      log.e(e);
      Fluttertoast.showToast(
        msg: "Failed to load soundboard!",
        toastLength: Toast.LENGTH_LONG,
        gravity: ToastGravity.CENTER,
        backgroundColor: ColorsConstants.blackToastColor,
        textColor: ColorsConstants.trueWhiteColor,
        fontSize: 12.0,
      );
    }
  }

  void onLongPressSoundboard(Soundboard soundboard) {
    Get.find<MainController>().enterEditMode(soundboard);
    isEditMode = true;
    selectedSoundboard = soundboard;
    update();
  }

  void onClearSelectedSoundboard() {
    Get.find<MainController>().exitEditMode();
    isEditMode = false;
    selectedSoundboard = null;
    update();
  }
}

class Chat {
  String? message;
  bool? isUser;

  Chat({this.message, this.isUser});
}
