import 'package:deni_hackathon/constants/assets_constants.dart';
import 'package:deni_hackathon/constants/colors_constants.dart';
import 'package:deni_hackathon/screens/main/main_controller.dart';
import 'package:deni_hackathon/widgets/deni_style.dart';
import 'package:flutter/material.dart';
import 'package:flutter_svg/svg.dart';
import 'package:get/get_state_manager/src/simple/get_state.dart';

import 'calendar/calendar_screen.dart';
import 'home/home_screen.dart';
import 'soundboard/soundboard_screen.dart';
import 'vision/vision_screen.dart';

class MainScreen extends StatelessWidget {
  const MainScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return GetBuilder<MainController>(
      init: MainController(),
      builder: (MainController controller) {
        var content;
        if (controller.currentBottomNavIndex == 0) {
          content = const HomeScreen();
        } else if (controller.currentBottomNavIndex == 1) {
          content = const CalendarScreen();
        } else if (controller.currentBottomNavIndex == 2) {
          content = const VisionScreen();
        } else if (controller.currentBottomNavIndex == 3) {
          content = const SoundboardScreen();
        } else {
          content = const HomeScreen();
        }
        return Scaffold(
          backgroundColor: Colors.white,
          body: content,
          // floatingActionButton: Column(
          //   mainAxisSize: MainAxisSize.min,
          //   children: [
          //     FloatingActionButton.small(
          //       heroTag: "vision",
          //       onPressed: () {
          //         // Vision button tapped
          //       },
          //       backgroundColor: const Color.fromARGB(255, 210, 178, 168),
          //       child: Icon(Icons.remove_red_eye),
          //     ),
          //     SizedBox(height: 8),
          //     FloatingActionButton(
          //       heroTag: "main",
          //       onPressed: () {
          //         // Main FAB tapped
          //       },
          //       backgroundColor: const Color.fromARGB(255, 212, 183, 178),
          //       child: Icon(Icons.add),
          //     ),
          //   ],
          // ),
          // floatingActionButtonLocation:
          //     FloatingActionButtonLocation.centerDocked,
          bottomNavigationBar: BottomAppBar(
            shape: CircularNotchedRectangle(),
            notchMargin: 8,
            color: ColorsConstants.whiteColor,
            height: 90,
            child: Row(
              mainAxisAlignment: MainAxisAlignment.spaceAround,
              children: <Widget>[
                _buildNavItem(controller, AssetConstants.homeIcon, "Home", 0),
                _buildNavItem(controller, AssetConstants.calendarIcon, "Calendar", 1),
                GestureDetector(
                  onTap: () => (),
                  child: Image.asset(
                    AssetConstants.emergencyButton,
                  ),
                ),
                _buildNavItem(controller, AssetConstants.visionIcon, "Vision", 2),
                _buildNavItem(controller, AssetConstants.notesIcon, "Notes", 3),
              ],
            ),
          ),
        );
      },
    );
  }

  Widget _buildNavItem(
    MainController controller,
    String iconPath,
    String label,
    int index,
  ) {
    final isSelected = controller.currentBottomNavIndex == index;
    return GestureDetector(
      onTap: () => (controller.changeBottomNavIndex(index)),
      child: Padding(
        padding: const EdgeInsets.symmetric(vertical: 10, horizontal: 8),
        child: Column(
          mainAxisSize: MainAxisSize.min,
          children: [
            SvgPicture.asset(
              iconPath,
              color: isSelected ? ColorsConstants.darkerPrimaryColor : ColorsConstants.mediumGreyColor,
              height: 24,
            ),
            Text(
              label,
              style: deniStyle(
                fontSize: 14,
                fontFamily: 'Afacad',
                color: isSelected ? ColorsConstants.darkerPrimaryColor : ColorsConstants.mediumGreyColor,
              ),
            ),
          ],
        ),
      ),
    );
  }
}
