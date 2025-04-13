import 'package:deni_hackathon/constants/assets_constants.dart';
import 'package:deni_hackathon/constants/colors_constants.dart';
import 'package:deni_hackathon/screens/main/home/home_controller.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/svg.dart';
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
            title: controller.isRecording == true ?
            InkWell(
              onTap: () {
                controller.onStopRecording();
              },
              child: Container(
                padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 4),
                decoration: BoxDecoration(
                  color: ColorsConstants.lightRedColor,
                  borderRadius: BorderRadius.circular(20),
                ),
                child: Text(
                  'Listening',
                  style: deniStyle(
                    color: Colors.red,
                    fontWeight: FontWeight.bold,
                    fontSize: 20,
                  ),
                ),
              ),
            ) : 
            InkWell(
              onTap: () {
                controller.onStartRecording();
              },
              child: Container(
                padding: const EdgeInsets.symmetric(horizontal: 16, vertical: 4),
                decoration: BoxDecoration(
                  color: ColorsConstants.lightGreyColor,
                  borderRadius: BorderRadius.circular(20),
                ),
                child: Text(
                  'Paused',
                  style: deniStyle(
                    fontSize: 16,
                    fontWeight: FontWeight.bold,
                    color: ColorsConstants.trueBlackColor,
                  ),
                ),
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
            leading: InkWell(
              onTap: () {
                controller.onClickProfile();
              },
              child: Padding(
                padding: EdgeInsets.only(left: 16),
                child: Icon(
                  Icons.person_outline,
                  color: ColorsConstants.mediumGreyColor,
                  size: 28,
                ),
              ),
            ),
          ),
          floatingActionButton: Stack(
            alignment: Alignment.bottomRight,
            children: [
              // Add Button (unchanged)
              Positioned(
                bottom: 0,
                right: 64, // Adjust position to the left of the chevron button
                child: Container(
                  margin: const EdgeInsets.only(bottom: 12),
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
              ),
              // First additional button with animation
              AnimatedPositioned(
                duration: const Duration(milliseconds: 300),
                curve: Curves.easeInOut,
                bottom: controller.isExpanded ? 75 : -75,
                // right: 16,
                child: AnimatedOpacity(
                  duration: const Duration(milliseconds: 300),
                  opacity: controller.isExpanded ? 1.0 : 0.0,
                  child: Material(
                    color: Colors.transparent,
                    child: InkWell(
                      borderRadius: BorderRadius.circular(16),
                      onTap: () {
                        controller.onClickAskDeni();
                      },
                      child: Container(
                        padding: const EdgeInsets.symmetric(
                          horizontal: 16,
                          vertical: 12,
                        ),
                        decoration: BoxDecoration(
                          borderRadius: BorderRadius.circular(32),
                          gradient: LinearGradient(
                            begin: Alignment.topCenter,
                            end: Alignment.bottomCenter,
                            colors: [Color(0xFFA74B4B), Color(0xFF411D1D)],
                          ),
                        ),
                        child: Row(
                          mainAxisSize: MainAxisSize.min,
                          children: [
                            SvgPicture.asset(
                              AssetConstants.askDeniIcon,
                              color: ColorsConstants.trueWhiteColor,
                            ),
                            const SizedBox(width: 8),
                            Text(
                              'Ask Deni',
                              style: deniStyle(
                                fontSize: 16,
                                fontWeight: FontWeight.bold,
                                fontFamily: 'Afacad',
                                color: ColorsConstants.trueWhiteColor,
                              ),
                            ),
                          ],
                        ),
                      ),
                    ),
                  ),
                ),
              ),
              // Second additional button with animation
              AnimatedPositioned(
                duration: const Duration(milliseconds: 300),
                curve: Curves.easeInOut,
                bottom: controller.isExpanded ? 140 : -140,
                // right: 16,
                child: AnimatedOpacity(
                  duration: const Duration(milliseconds: 300),
                  opacity: controller.isExpanded ? 1.0 : 0.0,
                  child: Material(
                    color: Colors.transparent,
                    child: InkWell(
                      borderRadius: BorderRadius.circular(16),
                      onTap: () {
                        print("First button clicked");
                      },
                      child: Container(
                        padding: const EdgeInsets.symmetric(
                          horizontal: 16,
                          vertical: 12,
                        ),
                        decoration: BoxDecoration(
                          borderRadius: BorderRadius.circular(32),
                          gradient: LinearGradient(
                            begin: Alignment.topCenter,
                            end: Alignment.bottomCenter,
                            colors: [Color(0xFFA74B4B), Color(0xFF411D1D)],
                          ),
                        ),
                        child: Row(
                          mainAxisSize: MainAxisSize.min,
                          children: [
                            SvgPicture.asset(
                              AssetConstants.ttsIcon,
                              color: ColorsConstants.trueWhiteColor,
                            ),
                            const SizedBox(width: 8),
                            Text(
                              'Text-to-Speech',
                              style: deniStyle(
                                fontSize: 16,
                                fontWeight: FontWeight.bold,
                                fontFamily: 'Afacad',
                                color: ColorsConstants.trueWhiteColor,
                              ),
                            ),
                          ],
                        ),
                      ),
                    ),
                  ),
                ),
              ),
              // Chevron Button
              Positioned(
                bottom: 0,
                // right: 16,
                child: Container(
                  margin: const EdgeInsets.only(bottom: 12),
                  decoration: BoxDecoration(
                    shape: BoxShape.circle,
                    gradient: LinearGradient(
                      begin: Alignment.topCenter,
                      end: Alignment.bottomCenter,
                      colors: [Color(0xFFAB4141), Color(0xFF5E0A0A)],
                    ),
                  ),
                  child: FloatingActionButton(
                    onPressed: () {
                      controller.toggleExpandButtons();
                    },
                    backgroundColor: Colors.transparent,
                    elevation: 0,
                    child: Transform.rotate(
                      angle: controller.isExpanded ? -1.5708 : 1.5708,
                      child: const Icon(
                        Icons.chevron_left,
                        color: ColorsConstants.trueWhiteColor,
                      ),
                    ),
                  ),
                ),
              ),
            ],
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
                                            if (soundboard ==
                                                controller.selectedSoundboard) {
                                              controller
                                                  .onClearSelectedSoundboard();
                                            } else {
                                              controller.onLongPressSoundboard(
                                                soundboard,
                                              );
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
                                              color:
                                                  soundboard ==
                                                          controller
                                                              .selectedSoundboard
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
