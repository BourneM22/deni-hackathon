import 'package:deni_hackathon/constants/colors_constants.dart';
import 'package:deni_hackathon/screens/main/home/home_controller.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

import '../../../widgets/deni_style.dart';

class HomeScreen extends StatelessWidget {
  const HomeScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return GetBuilder<HomeController>(
      init: HomeController(),
      builder: (HomeController controller) {
        return Scaffold(
          backgroundColor: ColorsConstants.baseColor,
          appBar: AppBar(
            automaticallyImplyLeading: false,
            backgroundColor: ColorsConstants.baseColor,
            elevation: 0,
            centerTitle: true,
            title: Text(
              'Listening',
              style: deniStyle(color: Colors.red, fontWeight: FontWeight.bold),
            ),
            actions: const [
              Padding(
                padding: EdgeInsets.only(right: 16),
                child: Icon(Icons.delete_outline, color: Colors.black),
              ),
            ],
            leading: const Padding(
              padding: EdgeInsets.only(left: 16),
              child: Icon(Icons.person_outline, color: Colors.black),
            ),
          ),
          floatingActionButton: Container(
            decoration: BoxDecoration(
              shape: BoxShape.circle,
              gradient: LinearGradient(
                begin: Alignment.topCenter,
                end: Alignment.bottomCenter,
                colors: [Color(0xFF6C2727), Color(0xFF000000)],
              ),
            ),
            child: FloatingActionButton(
              onPressed: () {
                controller.createSoundboard();
              },
              backgroundColor: Colors.transparent,
              elevation: 0,
              child: const Icon(
                Icons.add,
                color: ColorsConstants.trueWhiteColor,
              ),
            ),
          ),
          floatingActionButtonLocation: FloatingActionButtonLocation.endDocked,
          body: RefreshIndicator(
            onRefresh: () async {
              controller.onInit();
            },
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                SizedBox(
                  height: MediaQuery.of(context).size.height * 0.4,
                  child: SingleChildScrollView(
                    physics: const AlwaysScrollableScrollPhysics(),
                    controller: controller.scrollController,
                    child: Padding(
                      padding: EdgeInsets.all(16.0),
                      child: Column(
                        children:
                            controller.chatList.map((chat) {
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
                                              ? EdgeInsets.symmetric(
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
                                                  : ColorsConstants
                                                      .darkGreyColor,
                                        ),
                                      ),
                                    ),
                                  ),
                                ],
                              );
                            }).toList(),
                      ),
                    ),
                  ),
                ),
                Expanded(
                  child: Container(
                    padding: const EdgeInsets.only(top: 8),
                    decoration: BoxDecoration(
                      color: ColorsConstants.whiteColor,
                      borderRadius: const BorderRadius.only(
                        topLeft: Radius.circular(20),
                        topRight: Radius.circular(20),
                      ),
                    ),
                    child: Column(
                      children: [
                        Padding(
                          padding: const EdgeInsets.symmetric(horizontal: 16.0),
                          child: Row(
                            mainAxisAlignment: MainAxisAlignment.start,
                            children: [
                              Container(
                                padding: const EdgeInsets.symmetric(
                                  vertical: 4,
                                  horizontal: 12,
                                ),
                                decoration: BoxDecoration(
                                  color: ColorsConstants.darkerWhiteColor,
                                  borderRadius: BorderRadius.circular(20),
                                ),
                                child: Row(
                                  children: [
                                    Text('All'),
                                    Icon(
                                      Icons.arrow_drop_down,
                                      color: ColorsConstants.darkGreyColor,
                                    ),
                                  ],
                                ),
                              ),
                            ],
                          ),
                        ),
                        Divider(
                          color: ColorsConstants.darkGreyColor,
                          thickness: 1.0,
                        ),
                        Expanded(
                          child:
                              controller.isLoading
                                  ? const Center(
                                    child: CircularProgressIndicator(),
                                  )
                                  : GridView.builder(
                                    padding: const EdgeInsets.symmetric(
                                      horizontal: 16,
                                    ),
                                    gridDelegate:
                                        const SliverGridDelegateWithFixedCrossAxisCount(
                                          crossAxisCount: 2,
                                          crossAxisSpacing: 8,
                                          mainAxisSpacing: 8,
                                          childAspectRatio: 2,
                                        ),
                                    itemCount: controller.soundboardList.length,
                                    itemBuilder: (context, index) {
                                      final soundboard =
                                          controller.soundboardList[index];
                                      return InkWell(
                                        onTap: () {
                                          if (controller.isEditMode) {
                                            if (soundboard == controller.selectedSoundboard) {
                                              controller.onClearSelectedSoundboard();
                                            } else {
                                              controller.onLongPressSoundboard(soundboard);
                                            }
                                          } else {
                                            controller.onClickSoundboard(
                                              soundboard,
                                            );
                                          }
                                          
                                        },
                                        onLongPress: () {
                                          controller.onLongPressSoundboard(
                                            soundboard,
                                          );
                                        },
                                        child: Container(
                                          padding: const EdgeInsets.all(8),
                                          decoration: BoxDecoration(
                                            color:
                                                ColorsConstants
                                                    .darkerWhiteColor,
                                            borderRadius: BorderRadius.circular(
                                              10,
                                            ),
                                            border: Border.all(
                                              color: soundboard ==
                                                      controller.selectedSoundboard
                                                  ? ColorsConstants
                                                      .darkerPrimaryColor
                                                  : ColorsConstants
                                                      .darkerWhiteColor,
                                              width: 2,
                                            ),
                                          ),
                                          child: Column(
                                            crossAxisAlignment:
                                                CrossAxisAlignment.start,
                                            children: [
                                              Text(
                                                soundboard.name!,
                                                style: deniStyle(
                                                  fontWeight: FontWeight.bold,
                                                ),
                                              ),
                                              const SizedBox(height: 4),
                                              Text(
                                                soundboard.description!,
                                                style: deniStyle(fontSize: 12),
                                                maxLines: 2,
                                                overflow: TextOverflow.ellipsis,
                                              ),
                                            ],
                                          ),
                                        ),
                                      );
                                    },
                                  ),
                        ),
                      ],
                    ),
                  ),
                ),
              ],
            ),
          ),
        );
      },
    );
  }
}
