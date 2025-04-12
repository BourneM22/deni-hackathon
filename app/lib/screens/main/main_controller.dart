import 'package:get/get.dart';


class MainController extends GetxController {
  
  int currentBottomNavIndex = 0;

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

}