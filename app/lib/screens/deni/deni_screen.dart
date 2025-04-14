import 'package:deni_hackathon/constants/assets_constants.dart';
import 'package:deni_hackathon/screens/deni/deni_controller.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/svg.dart';
import 'package:get/get.dart';

import '../../../constants/colors_constants.dart';
import '../../../widgets/deni_style.dart';

class DeniScreen extends StatelessWidget {
  const DeniScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return GetBuilder<DeniController>(
      init: DeniController(),
      builder: (DeniController controller) {
        return Scaffold(
          backgroundColor: ColorsConstants.baseColor,
          resizeToAvoidBottomInset: true,
          appBar: AppBar(
            backgroundColor: ColorsConstants.baseColor,
            title: ShaderMask(
              shaderCallback:
                  (bounds) => LinearGradient(
                    colors: [Color(0xFF411D1D), Color(0xFFA74B4B)],
                    begin: Alignment.topLeft,
                    end: Alignment.bottomRight,
                  ).createShader(bounds),
              child: Text(
                'DeniGPT',
                style: deniStyle(
                  fontSize: 16,
                  fontWeight: FontWeight.bold,
                  color: Colors.white,
                ),
              ),
            ),
            leading: Padding(
              padding: const EdgeInsets.only(left: 16.0),
              child: IconButton(
                icon: SvgPicture.asset(
                  AssetConstants.backIcon,
                  height: 24,
                  color: ColorsConstants.mediumGreyColor,
                ),
                onPressed: () {
                  Navigator.pop(context);
                },
              ),
            ),
            actions: [
              InkWell(
                onTap: () {
                  controller.onClearChat();
                },
                child: Padding(
                  padding: EdgeInsets.only(right: 16),
                  child: SvgPicture.asset(
                    AssetConstants.deleteIcon,
                    color: ColorsConstants.mediumGreyColor,
                    height: 28,
                    width: 28,
                  ),
                ),
              ),
            ],
          ),
          bottomNavigationBar: Padding(
            padding: EdgeInsets.only(
              left: 16.0,
              right: 16.0,
              bottom: MediaQuery.of(context).viewInsets.bottom + 16.0, // Adjust for keyboard
            ),
            child: Row(
              children: [
                Flexible(
                  child: Container(
                    padding: const EdgeInsets.symmetric(horizontal: 16),
                    decoration: BoxDecoration(
                      color: Colors.white,
                      borderRadius: BorderRadius.circular(24),
                      boxShadow: [
                        BoxShadow(
                          color: Colors.black12,
                          blurRadius: 4,
                          offset: Offset(0, 2),
                        ),
                      ],
                    ),
                    child: TextField(
                      controller: controller.messageController,
                      decoration: const InputDecoration(
                        hintText: "Tell the AI what you want",
                        border: InputBorder.none,
                      ),
                    ),
                  ),
                ),
                const SizedBox(width: 8),
                Container(
                  width: 48,
                  height: 48,
                  decoration: BoxDecoration(
                    shape: BoxShape.circle,
                    gradient: const LinearGradient(
                      colors: [Color(0xFF6C2727), Color(0xFF000000)],
                      begin: Alignment.topCenter,
                      end: Alignment.bottomCenter,
                    ),
                    boxShadow: [
                      BoxShadow(
                        color: Colors.black26,
                        blurRadius: 4,
                        offset: Offset(0, 2),
                      ),
                    ],
                  ),
                  child: IconButton(
                    icon: const Icon(Icons.send, color: Colors.white),
                    onPressed: () {
                     controller.onSendMessage();
                    },
                  ),
                ),
              ],
            ),
          ),
          body: RefreshIndicator(
            onRefresh: () async {
              controller.onInit();
            },
            child: ListView.builder(
              padding: const EdgeInsets.all(
                16,
              ), // spacing for bottomNavigationBar
              itemCount: controller.chatList.length,
              itemBuilder: (context, index) {
                final chat = controller.chatList[index];
                return Row(
                  mainAxisAlignment:
                      chat.isUser == true
                          ? MainAxisAlignment.end
                          : MainAxisAlignment.start,
                  children: [
                    Flexible(
                      child: Container(
                        margin: const EdgeInsets.only(bottom: 8),
                        padding:
                            chat.isUser == true
                                ? const EdgeInsets.symmetric(
                                  vertical: 8,
                                  horizontal: 16,
                                )
                                : EdgeInsets.zero,
                        decoration: BoxDecoration(
                          color:
                              chat.isUser == true
                                  ? ColorsConstants.darkGreyColor
                                  : ColorsConstants.baseColor,
                          borderRadius: BorderRadius.circular(24),
                        ),
                        child: Text(
                          chat.message!,
                          style: deniStyle(
                            fontWeight: FontWeight.bold,
                            color:
                                chat.isUser == true
                                    ? ColorsConstants.whiteColor
                                    : ColorsConstants.darkGreyColor,
                          ),
                        ),
                      ),
                    ),
                  ],
                );
              },
            ),
          ),
        );
      },
    );
  }
}
