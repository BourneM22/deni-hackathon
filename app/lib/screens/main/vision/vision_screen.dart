import 'package:deni_hackathon/screens/main/vision/vision_controller.dart';
import 'package:flutter/material.dart';
import 'package:get/get.dart';

class VisionScreen extends StatelessWidget {
  const VisionScreen({super.key});

  @override
  Widget build(BuildContext context) {
    return GetBuilder<VisionController>(
      init: VisionController(),
      builder: (VisionController controller) {
        return Scaffold(
          appBar: AppBar(
            title: const Text('Vision Screen'),
          ),
          body: Center(
            child: Text(
              'This is the Vision Screen',
            ),
          ),
        );
      }
    );
  }
}