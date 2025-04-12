import 'package:deni_hackathon/api-response/soundboard_get_response.dart';
import 'package:deni_hackathon/constants/colors_constants.dart';
import 'package:deni_hackathon/widgets/deni_style.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

class MainController extends GetxController {
  int currentBottomNavIndex = 0;
  bool isEditMode = false;
  Soundboard? selectedSoundboard;
  @override
  void onInit() {
    super.onInit();
  }

  @override
  void dispose() {
    super.dispose();
  }

  void changeBottomNavIndex(int index) {
    currentBottomNavIndex = index;
    update();
  }

  void enterEditMode(dynamic soundboard) {
    isEditMode = true;
    selectedSoundboard = soundboard;
    update();
  }

  void exitEditMode() {
    isEditMode = false;
    selectedSoundboard = null;
    update();
  }

  void editSoundboard() {
    exitEditMode();
  }

  void moveSoundboard() {
    exitEditMode();
  }

  void deleteSoundboard(BuildContext context) {
    showGeneralDialog(
      context: context,
      barrierDismissible: true,
      barrierLabel: "Dismiss",
      barrierColor: Colors.black.withOpacity(0.4), // dim background
      transitionDuration: const Duration(milliseconds: 300),
      pageBuilder: (context, animation, secondaryAnimation) {
        return Align(
          alignment: Alignment.bottomCenter,
          child: Padding(
            padding: const EdgeInsets.all(16.0),
            child: Material(
              borderRadius: BorderRadius.circular(12),
              color: ColorsConstants.whiteColor,
              child: Padding(
                padding: const EdgeInsets.symmetric(vertical: 12.0),
                child: Column(
                  mainAxisSize: MainAxisSize.min,
                  crossAxisAlignment: CrossAxisAlignment.start,
                  children: [
                    Padding(
                      padding: const EdgeInsets.symmetric(horizontal: 16.0),
                      child: Text(
                        'Delete soundboard?',
                        style: deniStyle(
                          fontSize: 20,
                          fontFamily: 'Afacad',
                          fontWeight: FontWeight.bold,
                          color: ColorsConstants.darkGreyColor,
                        ),
                      ),
                    ),
                    Divider(color: Colors.grey, thickness: 1),
                    Row(
                      children: [
                        Expanded(
                          child: Padding(
                            padding: const EdgeInsets.only(left: 16.0),
                            child: ElevatedButton(
                              style: ElevatedButton.styleFrom(
                                backgroundColor: ColorsConstants.lightRedColor,
                                shape: RoundedRectangleBorder(
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
                        SizedBox(width: 8),
                        Expanded(
                          child: Padding(
                            padding: const EdgeInsets.only(right: 16.0),
                            child: ElevatedButton(
                              style: ElevatedButton.styleFrom(
                                backgroundColor:
                                    ColorsConstants.lightGreenColor,
                                shape: RoundedRectangleBorder(
                                  borderRadius: BorderRadius.all(
                                    Radius.circular(8),
                                  ),
                                ),
                              ),
                              onPressed: () {
                                // Perform delete action
                                Navigator.pop(context);
                              },
                              child: Text(
                                'Delete',
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

    // exitEditMode();
  }
}
