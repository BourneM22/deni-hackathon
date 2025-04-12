import 'package:get/get.dart';

import '../../../api-response/soundboard_get_response.dart';
import '../../../api/api_main.dart';
import '../../../utils/logger.dart';


class HomeController extends GetxController {
  
  List<Soundboard> soundboardList = [];
  bool isLoading = true;
  bool isError = false;

  @override
  void onInit() {
    isLoading = true;
    isError = false;
    super.onInit();
    getSoundboard();
    // createSoundboard();
  }

  @override
  void dispose() {
    super.dispose();
  }

  Future<void> getSoundboard() async {
    try {
      final response = await apiMain.getSoundboard();
      soundboardList = response.soundboards!;
      isLoading = false;
      update();
    } catch (e) {
      log.e(e);
      isError = true;
      update();
    } finally {
      isLoading = false;
      update();
    }
  }

  // Future<void> createSoundboard() async {
  //   try {
  //     final data = {
  //       'name': 'New Soundboard',
  //       'description': 'This is a new soundboard',
  //     };

  //     final response = await apiMain.createSoundboard(data);

  //     if (response) {
  //       getSoundboard();
  //     } else {
  //       isError = true;
  //       update();
  //     }
  //   } catch (e) {
  //     isError = true;
  //     update();
  //   } finally {
  //     isLoading = false;
  //     update();
  //   }
  // }

}