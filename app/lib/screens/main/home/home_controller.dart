import 'package:flutter/material.dart';
import 'package:get/get.dart';

import '../../../api-response/soundboard_get_response.dart';
import '../../../api/api_main.dart';
import '../../../utils/logger.dart';
import '../main_controller.dart';

class HomeController extends GetxController {
  List<Soundboard> soundboardList = [];
  List<Chat> chatList = [];
  bool isLoading = true;
  bool isError = false;

  final ScrollController scrollController = ScrollController();

  bool isEditMode = false;
  Soundboard? selectedSoundboard;

  @override
  void onInit() {
    isLoading = true;
    isError = false;
    super.onInit();
    getSoundboard();
    initChatList();
  }

  @override
  void dispose() {
    super.dispose();
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

  Future<void> createSoundboard() async {
    try {
      final data = {
        'name': 'New Soundboard',
        'description': 'This is a new soundboard',
      };

      final response = await apiMain.createSoundboard(data);

      if (response) {
        getSoundboard();
      } else {
        isError = true;
        update();
      }
    } catch (e) {
      isError = true;
      update();
    } finally {
      isLoading = false;
      update();
    }
  }

  void initChatList() {
    chatList.add(Chat(message: "Hello I'm Bourne!\nWhat's your name?", isUser: false));
    update();
  }

  void onClickSoundboard(Soundboard soundboard) {
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
  }

  void onLongPressSoundboard(Soundboard soundboard) {
    Get.find<MainController>().enterEditMode(soundboard);
  }
}

class Chat {
  String? message;
  bool? isUser;

  Chat({this.message, this.isUser});
}
