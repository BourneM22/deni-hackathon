import 'package:flutter/services.dart';
import 'package:get/get.dart';
import 'package:vibration/vibration.dart';

class EmergencyController extends GetxController {
  bool _isVibrating = false;

  @override
  void onInit() {
    super.onInit();

    // Lock orientation to landscape and hide system UI
    SystemChrome.setPreferredOrientations([
      DeviceOrientation.landscapeLeft,
      DeviceOrientation.landscapeRight,
    ]);
    SystemChrome.setEnabledSystemUIMode(SystemUiMode.immersiveSticky);

    // Start vibration
    _startVibration();
  }

  @override
  void dispose() {
    // Stop vibration
    _stopVibration();

    // Restore orientation and system UI
    SystemChrome.setPreferredOrientations([
      DeviceOrientation.portraitUp,
      DeviceOrientation.portraitDown,
    ]);
    SystemChrome.setEnabledSystemUIMode(SystemUiMode.edgeToEdge);

    super.dispose();
  }

  void _startVibration() async {
    _isVibrating = true;

    // Check if the device supports vibration
    if (await Vibration.hasVibrator()) {
      while (_isVibrating) {
        // Vibrate for 500ms with a 1-second pause
        Vibration.vibrate(duration: 500);
        await Future.delayed(const Duration(seconds: 1));
      }
    }
  }

  void _stopVibration() {
    _isVibrating = false;
    Vibration.cancel(); // Stop any ongoing vibration
  }
}