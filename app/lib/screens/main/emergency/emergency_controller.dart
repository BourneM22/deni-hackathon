import 'package:audioplayers/audioplayers.dart';
import 'package:deni_hackathon/constants/assets_constants.dart';
import 'package:flutter/services.dart';
import 'package:get/get.dart';
import 'package:vibration/vibration.dart';

import '../../../utils/logger.dart';

class EmergencyController extends GetxController {
  bool _isVibrating = false;


  final AudioPlayer audioPlayer = AudioPlayer();

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

    playEmergencyAudio(); // Play emergency audio
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

    audioPlayer.dispose(); // Dispose of the audio player
    audioPlayer.stop(); // Stop any ongoing audio playback
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

  void playEmergencyAudio() async {
    try {
      // Load the audio file from assets
      await audioPlayer.setSource(AssetSource('audio/emergency.mp3'));
      // Play the audio
      await audioPlayer.resume();
    } catch (e) {
      log.e("Error playing audio: $e");
    }
  }
}