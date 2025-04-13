import 'package:deni_hackathon/api/api_deni.dart';
import 'package:deni_hackathon/screens/main/home/home_controller.dart';
import 'package:flutter/widgets.dart';
import 'package:fluttertoast/fluttertoast.dart';
import 'package:get/get.dart';

import '../../constants/colors_constants.dart';
import '../../utils/logger.dart';


class DeniController extends GetxController {
  
  List<Chat> chatList = [];
  TextEditingController messageController = TextEditingController();

  @override
  void onInit() {
    super.onInit();
    initChatList();
  }

  @override
  void dispose() {
    super.dispose();
  }

  void initChatList() {
    chatList = [];
    chatList.add(
      Chat(message: "Hello Bourne, I’m Hans! Nice to meet you! I’m deaf, but I’m using this cool new app called Deni to help me communicate!", isUser: false),
    );
    update();
  }

  Future<void> onSendMessage() async {
    try {
      String message = messageController.text.trim();
      if (message.isEmpty) return;
      chatList.add(Chat(message: message, isUser: true));
      update();
      messageController.clear();

      final data = {
        'prompt': message,
      };

      final response = await apiDeni.sendToChatbot(data);

      chatList.add(
        Chat(
          message: response.response,
          isUser: false,
        ),
      );
      update();
    } catch (e) {
      log.e(e);
      Fluttertoast.showToast(
        msg: "Something went wrong!",
        toastLength: Toast.LENGTH_SHORT,
        gravity: ToastGravity.BOTTOM,
        backgroundColor: ColorsConstants.blackToastColor,
        textColor: ColorsConstants.trueWhiteColor,
        fontSize: 16.0,
      );
    }
    
  }

  void onClearChat() {
    initChatList();
  }

}