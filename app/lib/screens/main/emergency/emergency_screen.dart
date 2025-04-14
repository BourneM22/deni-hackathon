import 'package:deni_hackathon/constants/colors_constants.dart';
import 'package:deni_hackathon/screens/main/emergency/emergency_controller.dart';
import 'package:flutter/material.dart';
import 'package:get/get_state_manager/src/simple/get_state.dart';

class EmergencyScreen extends StatelessWidget {
  const EmergencyScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return GetBuilder<EmergencyController>(
      init: EmergencyController(),
      builder: (EmergencyController controller) {
        return Scaffold(
          backgroundColor: ColorsConstants.redColor,
          body: Center(
            child: Padding(
              padding: const EdgeInsets.only(left: 80, right: 40),
              child: Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  Text(
                    'HELP',
                    style: TextStyle(
                      color: ColorsConstants.trueWhiteColor,
                      fontSize: 200,
                      fontWeight: FontWeight.bold,
                    ),
                  ),
                  InkWell(
                    onTap: () {
                      controller.dispose();
                      Navigator.pop(context);
                    },
                    child: Container(
                      decoration: BoxDecoration(
                        color: ColorsConstants.trueWhiteColor,
                        borderRadius: BorderRadius.circular(100),
                      ),
                      child: Icon(
                        Icons.close,
                        size: 50,
                        color: ColorsConstants.redColor,
                      ),
                    ),
                  ),
                ],
              ),
            ),
          ),
        );
      },
    );
  }
}
